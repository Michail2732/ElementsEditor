using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElementsEditor.Gateway.PostgresDb
{
    // todo: реализовать отмену
    public class DbGateway<TElement> : IElementsGateway<TElement>
        where TElement: Element
    {
        private readonly string _connectionString;        
        private readonly DbFilterConverter _converter;
        private readonly DbTableColumnsMap _propertyMap;
        private readonly DbElementMaps _dbElementsMapper;

        public DbGateway(string connectionString,
            DbTableColumnsMap propertyMap, 
            DbElementMaps productMapLocator)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _propertyMap = propertyMap ?? throw new ArgumentNullException(nameof(propertyMap));
            _converter = new DbFilterConverter(propertyMap);
            _dbElementsMapper = productMapLocator ?? throw new ArgumentNullException(nameof(productMapLocator));
        }

        public void Add(TElement element)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string query = _dbElementsMapper.GetMap(element).CreateInsertQuery(element, _propertyMap);
                connection.Open();
                var command = new NpgsqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        public Task AddAsync(TElement element, CancellationToken ct)
        {
            return Task.Run(() => Add(element), ct);
        }

        public long GetCount(Query query)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string dbQuery = BuildDbQuery($"select count(*) from {_propertyMap.TableName}", query, false);
                var command = new NpgsqlCommand(dbQuery, connection);
                var result = command.ExecuteScalar();
                return (long)result;
            }
        }

        public Task<long> GetCountAsync(Query query, CancellationToken ct)
        {
            return Task.Run(() => GetCount(query), ct);
        }

        public TElement[] GetElements(Query query)
        {
            List<TElement> result = new List<TElement>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string dbQuery = BuildDbQuery($"select * from {_propertyMap.TableName}", query);
                var command = new NpgsqlCommand(dbQuery, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var product = _dbElementsMapper.GetMap(reader, _propertyMap)
                                                    .MapToModel(reader, _propertyMap);
                    result.Add((TElement)product);
                }
            }
            return result.ToArray();
        }

        public Task<TElement[]> GetElementsAsync(Query query, CancellationToken ct)
        {
            return Task.Run(() => GetElements(query), ct);            
        }

        public void Remove(IEnumerable<TElement> elements)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var element in elements)
                {                    
                    sb.Append("\n" + $"delete from {_propertyMap.TableName} where {_propertyMap.GetPropertyNameInDb(nameof(Element.Id)).ColumnName} = '{element.Id}';");
                }
                connection.Open();
                var transaction = connection.BeginTransaction();
                var command = new NpgsqlCommand(sb.ToString(), connection);
                command.ExecuteNonQuery();
                transaction.Commit();
            }
        }

        public Task RemoveAsync(IEnumerable<TElement> elements, CancellationToken ct)
        {
            return Task.Run(() => Remove(elements), ct);
        }

        public void SaveChanges(IEnumerable<TElement> changesElements)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {                
                StringBuilder sb = new StringBuilder();
                foreach (var changeElement in changesElements)
                {                    
                    sb.Append("\n" + _dbElementsMapper.GetMap(changeElement).CreateUpdateQuery(changeElement, _propertyMap));                 
                }
                connection.Open();
                var transaction = connection.BeginTransaction();
                var command = new NpgsqlCommand(sb.ToString(), connection);
                command.ExecuteNonQuery();
                transaction.Commit();
            }
        }        

        public Task SaveChangesAsync(IEnumerable<TElement> changesElements, CancellationToken ct)
        {
            return new Task(() => SaveChanges(changesElements));
        }        

        private string BuildDbQuery(string baseDbQuery, Query query, bool withPagination = true)
        {
            StringBuilder dbQueryBuilder = new StringBuilder(baseDbQuery);
            if (query.Filters?.Count > 0)
            {
                var filterQuery = _converter.Convert(query.Filters);
                dbQueryBuilder.Append($"\nwhere {filterQuery}");
            }
            if (query.ExcludedIds?.Count > 0)
            {
                if (query.Filters?.Count > 0)
                    dbQueryBuilder.Append(" and true ");
                else
                    dbQueryBuilder.Append("\nwhere true ");
                foreach (var excludeId in query.ExcludedIds)
                    dbQueryBuilder.Append($" and {_propertyMap.GetPropertyNameInDb(nameof(Element.Id)).ColumnName} <> '{excludeId}' ");
            }
            if (withPagination)
            {
                if (query.Count > 0)
                    dbQueryBuilder.Append($" limit {query.Count} ");
                if (query.Offset > 0)
                    dbQueryBuilder.Append($" offset {query.Offset}");
            }            
            return dbQueryBuilder.ToString();
        }        



    }
}
