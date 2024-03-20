using Avalonia.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace ElementsEditor
{   
	public class ElementsEditorViewModel<TElement> : IElementsEditorViewModel
        where TElement : Element
	{
        private const int _defaultCurrentPage = 1;
        
        private readonly IElementsGateway<TElement> _itemsGateway;

        private readonly List<TElement> _changedItems;
        private CancellationTokenSource? _extractCts;        


        private int _currentPage = _defaultCurrentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set 
            {             
                SetAndRaisePropertyChanged(ref _currentPage, value, out bool isChanged);
                if (isChanged)
                    InvalidateElements();                    
            }
        }

        private int _pageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set 
            {
                SetAndRaisePropertyChanged(ref _pageSize, value);
                if (CurrentPage == _defaultCurrentPage)
                    InvalidateElements();
                else 
                    CurrentPage = _defaultCurrentPage;                
            }
        }

        private bool _enablePaggination;
        public bool EnablePagination
        {
            get { return _enablePaggination; }
            private set { SetAndRaisePropertyChanged(ref _enablePaggination, value); }
        }

        private bool _enableAsync;
        public bool EnableAsync
        {
            get { return _enableAsync; }
            set { SetAndRaisePropertyChanged(ref _enableAsync, value); }
        }

        private bool _enableRemoving;
        public bool EnableRemoving
        {
            get { return _enableRemoving; }
            private set { SetAndRaisePropertyChanged(ref _enableRemoving, value); }
        }        

        private long _itemsCount;
        public long ItemsCount
        {
            get { return _itemsCount; }
            private set { SetAndRaisePropertyChanged(ref _itemsCount, value); }
        }

        private int _pagesCount;
        public int PagesCount
        {
            get { return _pagesCount; }
            private set { SetAndRaisePropertyChanged(ref _pagesCount, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            private set { SetAndRaisePropertyChanged(ref _isBusy, value); }
        }                

        public ObservableCollection<IPropertyFilterModel> Filters { get; }
        public IEnumerable<PropertyFilterDescriptor>? FilterDescriptors { get; }
        private IReadOnlyList<IPropertyFilter>? _appliedFilters;        

        private string? _appliedFiltersInfo;
        public string? AppliedFiltersInfo
        {
            get => _appliedFiltersInfo;
            set => SetAndRaisePropertyChanged(ref _appliedFiltersInfo, value);
        }

        private PropertyFilterDescriptor? _selectedFilterDescriptor;
        public PropertyFilterDescriptor? SelectedFilterDescriptor
        {
            get => _selectedFilterDescriptor;
            set
            {
                SetAndRaisePropertyChanged(ref _selectedFilterDescriptor, value);
                UpdateCommandsCanExecute();
            } 
        }

        private IPropertyFilterModel? _selectedFilter;
        public IPropertyFilterModel? SelectedFilter
        {
            get { return _selectedFilter; }
            set 
            { 
                SetAndRaisePropertyChanged(ref _selectedFilter, value);
                UpdateCommandsCanExecute();
            }
        }



        public ObservableCollection<Element> Elements { get; }        
        public IEnumerable<ElementBuilder>? ElementBuilders { get; }        

        private IList _selectedElements = new List<Element>();
        public IList SelectedElements
        {
            get => _selectedElements;
            set
            {
                SetAndRaisePropertyChanged(ref _selectedElements, value);
                UpdateCommandsCanExecute();
            } 
        }



        public Command NextPageCommand { get; }
        public Command PreviousPageCommand { get; }
        public Command RefreshElementsCommand { get; }                
        public Command RemoveSelectedElementsCommand { get; }
        public Command SaveChangesCommand { get; }                
        public Command ApplyFiltersCommand { get; }
        public Command AddNewElementCommand { get; }
        public Command AddNewFilterCommand { get; }
        public Command ShowFiltersCommand { get; }
        public Command DeleteAppliedFiltersCommand { get; }

        public ElementsEditorViewModel(
			IElementsGateway<TElement> itemsGateway,
            IEnumerable<PropertyFilterDescriptor>? filterFactories = null,
            IEnumerable<ElementBuilder>? elementBuilders = null,
            int? pageSize = null,
            bool enableAsync = false,
            bool enableRemoving = false)
        {
            _itemsGateway = itemsGateway;
            Elements = new ObservableCollection<Element>();
            Filters = new ObservableCollection<IPropertyFilterModel>();
            FilterDescriptors = filterFactories?.ToList();
            ElementBuilders = elementBuilders?.ToList();
            if (pageSize.HasValue)
			{
				_pageSize = pageSize.Value;
				_enablePaggination = true;
            }
            _enableAsync = enableAsync;
            _enableRemoving = enableRemoving;
            
            _changedItems = new List<TElement>();
            NextPageCommand = new Command(
                    param => CurrentPage++,
                    param => !IsBusy && EnablePagination && ItemsCount > (CurrentPage * PageSize));
            PreviousPageCommand = new Command(
                param => CurrentPage--,
                param => !IsBusy && EnablePagination && CurrentPage > 1);
            RefreshElementsCommand = new Command(
                param => InvalidateElements(),
                param => !IsBusy);
            ShowFiltersCommand = new Command(ShowFiltersCommandHandler,
                param => !IsBusy);
            RemoveSelectedElementsCommand = new Command(RemoveCommandHandler,
                param => !IsBusy && SelectedElements?.Count > 0);            
            SaveChangesCommand = new Command(SaveCommandHandler,
                param => !IsBusy && _changedItems.Count > 0);
            ApplyFiltersCommand = new Command(ApplyFiltersCommandHandler,
                param => !IsBusy && Filters.Count > 0);
            AddNewFilterCommand = new Command(AddNewFilterCommandHandler,
                param => !IsBusy && SelectedFilterDescriptor != null);
            AddNewElementCommand = new Command(AddNewElementCommandHandler,
                param => !IsBusy && ElementBuilders != null);
            DeleteAppliedFiltersCommand = new Command(DeleteAppliedFiltersCommandHandler,
                param => !IsBusy);
            InvalidateElements();
        }

        public void UpdateCommandsCanExecute()
        {
            NextPageCommand.OnCanExecuteChanged();
            PreviousPageCommand.OnCanExecuteChanged();
            RefreshElementsCommand.OnCanExecuteChanged();
            RemoveSelectedElementsCommand.OnCanExecuteChanged();            
            SaveChangesCommand.OnCanExecuteChanged();
            ApplyFiltersCommand.OnCanExecuteChanged();
            AddNewElementCommand.OnCanExecuteChanged();
            AddNewFilterCommand.OnCanExecuteChanged();
            DeleteAppliedFiltersCommand.OnCanExecuteChanged();
        }

        #region Command Handlers
        private void DeleteAppliedFiltersCommandHandler(object? param)
        {
            if (_appliedFilters != null)
            {
                _appliedFilters = null;
                AppliedFiltersInfo = null;
                InvalidateElements();
            }            
        }

        private void AddNewFilterCommandHandler(object? param)
        {            
            Filters.Add(SelectedFilterDescriptor!.CreateFilterModel(Filters.Count == 0));
            UpdateCommandsCanExecute();
        }

        private void ApplyFiltersCommandHandler(object? param)
        {
            AppliedFiltersInfo = string.Join("\n", Filters.Select(a => $"{a.PropertyName} {a.SelectedOperation} {a.GetValue()}"));
            _appliedFilters = new List<IPropertyFilter>(Filters.Select(a => a.ToPropertyFilter()));
            _isBusy = true;
            CurrentPage = _defaultCurrentPage;
            _isBusy = false;
            InvalidateElements();
        }      
        
        private async void ShowFiltersCommandHandler(object? param)
        {
            await this.ShowFiltersListDialog();
        }

        private async void RemoveCommandHandler(object? param)
        {            
            try
            {
                IsBusy = true;
                _extractCts = new CancellationTokenSource();
                if (!_enableAsync)
                    _itemsGateway.Remove((IEnumerable<TElement>)SelectedElements);
                else
                    await _itemsGateway.RemoveAsync((IEnumerable<TElement>)SelectedElements, _extractCts.Token);
                InvalidateElements();
            }
            catch (OperationCanceledException) { }
            catch (Exception e) { throw; }
            finally
            {
                IsBusy = false;
                UpdateCommandsCanExecute();
            }                                            
        }        

        private async void SaveCommandHandler(object? param)
        {
            try
            {
                _extractCts = new CancellationTokenSource();
                IsBusy = true;
                if (EnableAsync)
                    await _itemsGateway.SaveChangesAsync(_changedItems, _extractCts.Token);
                else
                    _itemsGateway.SaveChanges(_changedItems);                
                _changedItems.Clear();
            }
            catch (OperationCanceledException) { }
            catch (Exception e) { throw; }
            finally
            {
                IsBusy = false;
                UpdateCommandsCanExecute();
            }
            InvalidateElements();
        }
        #endregion

        public async void AddNewElementCommandHandler(object? param)
        {
            var newElement = await this.ShowAddNewElementDialog(); 
            if (newElement == null)
                return;
            try
            {                
                _extractCts = new CancellationTokenSource();
                IsBusy = true;
                if (_enableAsync)
                    await _itemsGateway.AddAsync((TElement)newElement!, _extractCts.Token);
                else
                    _itemsGateway.Add((TElement)newElement!);
            }
            catch (OperationCanceledException) { }
            catch (Exception e) { throw; }
            finally
            {
                IsBusy = false;
                UpdateCommandsCanExecute();
            }            
        }

        private async void InvalidateElements()
        {
            if (IsBusy || _itemsGateway is null)
                return;
            if (_changedItems.Count > 0)
            {
                var isContinue = await this.ShowHaveEditElementsDialog();
                if (!isContinue)
                    return;
            }
            try
            {                
                _extractCts = new CancellationTokenSource();
                IsBusy = true;
                UpdateCommandsCanExecute();

                var elementsQuery = new Query(
                        _enablePaggination ? _pageSize * (_currentPage - _defaultCurrentPage) : null,
                        _enablePaggination ? _pageSize : null,
                        null,
                        _appliedFilters
                    );                

                var newElements = _enableAsync
                        ? await _itemsGateway.GetElementsAsync(elementsQuery, _extractCts.Token)
                        : _itemsGateway.GetElements(elementsQuery);

                ItemsCount = _enableAsync
                        ? await _itemsGateway.GetCountAsync(elementsQuery, _extractCts.Token)
                        : _itemsGateway.GetCount(elementsQuery);

                if (_enablePaggination)
                    PagesCount = _pageSize != 0
                        ? (int)(_itemsCount / _pageSize) + (_itemsCount % _pageSize == 0 ? 0 : 1)
                        : 0;
                
                foreach (var element in Elements)
                    element.PropertyChanged -= OnElementPropertyChanged;
                
                Elements.Clear();                
                for (int i = 0; i < newElements.Length; i++)
                {                    
                    Elements.Add(newElements[i]);
                    newElements[i].PropertyChanged += OnElementPropertyChanged;
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception e) { throw; }
            finally
            {
                IsBusy = false;
                UpdateCommandsCanExecute();
            }
        }


        public void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            TElement element = (TElement)sender;
            if (!_changedItems.Contains(element))
            {
                _changedItems.Add(element);
                element.IsModified = true;
            }                
        }


        #region INotifyPropertyChanged impl
        public event PropertyChangedEventHandler? PropertyChanged;
		protected void SetAndRaisePropertyChanged<T>(ref T oldValue, T newValue, out bool isChanged,
            [CallerMemberName] string property = "")
		{
			isChanged = oldValue?.Equals(newValue) != true;
            if (!isChanged)
				return;
			oldValue = newValue;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}
		protected void SetAndRaisePropertyChanged<T>(ref T oldValue, T newValue, [CallerMemberName] string property = "")
			=> SetAndRaisePropertyChanged(ref oldValue, newValue, out var a, property);

        #endregion
    }

}
