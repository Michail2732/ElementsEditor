using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElementsEditor
{

    [TemplatePart("PART_ItemsPresenter", typeof(ListBox))]
    public class ElementsEditorControl : TemplatedControl, IElementsStateWatcher
    {
        private ListBox? _elementsPresenter;
        private List<Element> _elementWithChangeStates;
        private CancellationTokenSource? _extractCts;
        private IElementsGateway _localElementsGateway;        

        public static readonly DirectProperty<ElementsEditorControl, ElementsEditorConfiguration> ConfigurationProperty;
        public static readonly DirectProperty<ElementsEditorControl, ElementsFilter?> SelectedFilterProperty;
        public static readonly DirectProperty<ElementsEditorControl, int> CurrentPageProperty;        
        public static readonly DirectProperty<ElementsEditorControl, ElementsEditorCommand> NextPageCommandProperty;
        public static readonly DirectProperty<ElementsEditorControl, ElementsEditorCommand> PreviousPageCommandProperty;
        public static readonly DirectProperty<ElementsEditorControl, ElementsEditorCommand> RefreshCommandProperty;
        public static readonly DirectProperty<ElementsEditorControl, ElementsEditorCommand> RemoveSelectedElementsCommandProperty;
        public static readonly DirectProperty<ElementsEditorControl, ElementsEditorCommand> SaveChangesCommandProperty;        
        public static readonly DirectProperty<ElementsEditorControl, long> ElementsCountProperty;        
        public static readonly DirectProperty<ElementsEditorControl, bool> IsBusyProperty;
        public static readonly DirectProperty<ElementsEditorControl, bool> IsShowChangesProperty;


        private ElementsEditorConfiguration _configuration;
        public ElementsEditorConfiguration Configuration
        {
            get { return _configuration; }
            set { SetAndRaise(ConfigurationProperty, ref _configuration, value); }
        }


        private int _currentPage = 1;
        public int CurrentPage
        {
            get { return _currentPage; }
            set { SetAndRaise(CurrentPageProperty, ref _currentPage, value); }
        }        

        private ElementsEditorCommand _nextPageCommand;
        public ElementsEditorCommand NextPageCommand
        {
            get { return _nextPageCommand; }
            private set { SetAndRaise(NextPageCommandProperty, ref _nextPageCommand, value); }
        }

        private ElementsEditorCommand _previousPageCommand;
        public ElementsEditorCommand PreviousPageCommand
        {
            get { return _previousPageCommand; }
            private set { SetAndRaise(PreviousPageCommandProperty, ref _previousPageCommand, value); }
        }

        private ElementsEditorCommand _refreshCommand;
        public ElementsEditorCommand RefreshCommand
        {
            get { return _refreshCommand; }
            private set { SetAndRaise(RefreshCommandProperty, ref _refreshCommand, value); }
        }

        private ElementsEditorCommand _removeSelectedElementsCommand;
        public ElementsEditorCommand RemoveSelectedElementsCommand
        {
            get { return _removeSelectedElementsCommand; }
            private set { SetAndRaise(RemoveSelectedElementsCommandProperty, ref _removeSelectedElementsCommand, value); }
        }

        private ElementsEditorCommand _saveChangesCommand;
        public ElementsEditorCommand SaveChangesCommand
        {
            get { return _saveChangesCommand; }
            private set { SetAndRaise(SaveChangesCommandProperty, ref _saveChangesCommand, value); }
        }        

        private long _elementsCount;
        public long ElementsCount
        {
            get { return _elementsCount; }
            private set { SetAndRaise(ElementsCountProperty, ref _elementsCount, value); }
        }        

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            private set { SetAndRaise(IsBusyProperty, ref _isBusy, value); }
        }        

        private ElementsFilter? _selectedFilter;
        public ElementsFilter? SelectedFilter
        {
            get { return _selectedFilter; }
            set { SetAndRaise(SelectedFilterProperty, ref _selectedFilter, value); }
        }

        private bool _isShowChanges;
        public bool IsShowChanges
        {
            get { return _isShowChanges; }
            set { SetAndRaise(IsShowChangesProperty, ref _isShowChanges, value); }
        }

        static ElementsEditorControl()
        {
            ConfigurationProperty = AvaloniaProperty.RegisterDirect<ElementsEditorControl, ElementsEditorConfiguration>(
                nameof(Configuration),
                getter: o => o.Configuration,
                setter: (o, v) => o.Configuration = v);
            CurrentPageProperty = AvaloniaProperty.RegisterDirect<ElementsEditorControl, int>(
                nameof(CurrentPage),
                getter: a => a.CurrentPage,
                setter: (a, v) =>
                {
                    if (!a.IsBusy && v >= 0)
                        a.CurrentPage = v;
                },
                enableDataValidation: true);                        
            NextPageCommandProperty = AvaloniaProperty.RegisterDirect<ElementsEditorControl, ElementsEditorCommand>(
                nameof(NextPageCommand),
                getter: o => o.NextPageCommand);
            PreviousPageCommandProperty = AvaloniaProperty.RegisterDirect<ElementsEditorControl, ElementsEditorCommand>(
                nameof(PreviousPageCommand),
                getter: o => o.PreviousPageCommand);
            ElementsCountProperty = AvaloniaProperty.RegisterDirect<ElementsEditorControl, long>(
                nameof(ElementsCount),
                getter: o => o.ElementsCount);
            RefreshCommandProperty = AvaloniaProperty.RegisterDirect<ElementsEditorControl, ElementsEditorCommand>(
                nameof(RefreshCommand),
                getter: o => o.RefreshCommand);
            RemoveSelectedElementsCommandProperty = AvaloniaProperty.RegisterDirect<ElementsEditorControl, ElementsEditorCommand>(
                nameof(RefreshCommand),
                getter: o => o.RemoveSelectedElementsCommand);
            SaveChangesCommandProperty = AvaloniaProperty.RegisterDirect<ElementsEditorControl, ElementsEditorCommand>(
                nameof(SaveChangesCommand),
                getter: o => o.SaveChangesCommand);            
            IsBusyProperty = AvaloniaProperty.RegisterDirect<ElementsEditorControl, bool>(
                nameof(IsBusy),
                getter: o => o.IsBusy);
            IsShowChangesProperty = AvaloniaProperty.RegisterDirect<ElementsEditorControl, bool>(
                nameof(IsShowChanges),
                getter: o => o.IsShowChanges,
                setter: (o, v) => o.IsShowChanges = v);
            SelectedFilterProperty = AvaloniaProperty.RegisterDirect<ElementsEditorControl, ElementsFilter?>(
                nameof(SelectedFilter),
                getter: o => o.SelectedFilter,
                setter: (o, v) => o.SelectedFilter = v);            
        }


        public ElementsEditorControl()
        {
            PreviousPageCommand = new ElementsEditorCommand(this,
                editor => editor.CurrentPage -= 1,
                editor => editor._configuration.UsePaggination && editor._currentPage > 1 && !editor.IsBusy);
            NextPageCommand = new ElementsEditorCommand(this,
                editor => editor.CurrentPage += 1,
                editor =>
                {
                    return editor._configuration.UsePaggination &&
                          !editor.IsBusy &&
                           editor._elementsCount > (editor._currentPage * editor._configuration.PageSize);
                });
            RefreshCommand = new ElementsEditorCommand(this,
                editor => editor.InvalidateElements(),
                editor => !editor.IsBusy);
            RemoveSelectedElementsCommand = new ElementsEditorCommand(this,
                editor =>
                {
                    foreach (Element selectedElement in editor._elementsPresenter!.SelectedItems!)
                        selectedElement.IsRemoved = true;
                    editor.InvalidateElements();
                },
                editor => editor._elementsPresenter?.SelectedItems?.Count > 0);
            SaveChangesCommand = new ElementsEditorCommand(this,
                SaveChanges,
                editor => editor._elementWithChangeStates.Count > 0 && 
                          !editor._isBusy);
            _elementWithChangeStates = new List<Element>();
            _localElementsGateway = new ElementsCollectionGateway<Element>(_elementWithChangeStates);
        }


        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            if (_elementsPresenter != null)
            {
                _elementsPresenter.SelectionChanged -= OnSelectionElementsChanged;
            }
            _elementsPresenter = e.NameScope.Find("PART_ItemsPresenter") as ListBox;
            if (_elementsPresenter != null)
            {
                _elementsPresenter.SelectionChanged += OnSelectionElementsChanged;
            }
            InvalidateElements();
        }

        private void OnSelectionElementsChanged(object? sender, SelectionChangedEventArgs e)
        {
            UpdateCanExecuteCommands();
        }


        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            switch (change.Property.Name)
            {
                case nameof(Configuration):
                    OnConfigurationChange(change);
                    break;
                case nameof(CurrentPage):                
                    OnPageChange(change);
                    break;
                case nameof(SelectedFilter):
                    OnSelectedFilterChange(change);
                    break;
                case nameof(IsShowChanges):
                    OnShowChangesChange(change);
                    break;
                default:
                    break;
            }
        }

        private void OnConfigurationChange(AvaloniaPropertyChangedEventArgs change)
        {
            ResetThisState();
            if (change.OldValue != null)
                ((ElementsEditorConfiguration)change.OldValue).PropertyChanged -= OnConfigurationSubPropertyChange;
            if (change.NewValue != null)
                ((ElementsEditorConfiguration)change.NewValue).PropertyChanged += OnConfigurationSubPropertyChange;
            InvalidateElements();
        }

        private void OnConfigurationSubPropertyChange(object? sender, PropertyChangedEventArgs change)
        {
            switch (change.PropertyName)
            {
                case nameof(ElementsEditorConfiguration.ElementsGateway):
                    ResetThisState();
                    InvalidateElements();
                    break;
                case nameof(ElementsEditorConfiguration.PageSize):
                case nameof(ElementsEditorConfiguration.UsePaggination):
                    InvalidateElements();
                    break;
                case nameof(ElementsEditorConfiguration.AwailableRemove):
                    UpdateCanExecuteCommands();
                    break;
                case nameof(ElementsEditorConfiguration.Filters):                                                        
                case nameof(ElementsEditorConfiguration.UseAsync):                                                        
                default:
                    break;
            }
            InvalidateElements();
        }

        private void OnShowChangesChange(AvaloniaPropertyChangedEventArgs change)
        {            
            InvalidateElements();
        }        

        private void OnPageChange(AvaloniaPropertyChangedEventArgs change)
        {         
            InvalidateElements();
        }

        private void OnSelectedFilterChange(AvaloniaPropertyChangedEventArgs change)
        {
            var oldFilter = change.OldValue as ElementsFilter;
            var newFilter = change.NewValue as ElementsFilter;
            if (oldFilter != null)            
                oldFilter.ApplyFilter -= OnApplyFilter;
            if (newFilter != null)            
                newFilter.ApplyFilter += OnApplyFilter;            
        }


        private void OnApplyFilter(object? sender, EventArgs e)
        {
            _isBusy = true;
            CurrentPage = 0;
            _isBusy = false;             
            InvalidateElements();
        }

        private static async void SaveChanges(ElementsEditorControl editor)
        {
            try
            {
                editor._extractCts = new CancellationTokenSource();
                editor.IsBusy = true;
                editor.UpdateCanExecuteCommands();
                if (editor.Configuration.UseAsync)
                    await editor.Configuration.ElementsGateway.SaveChangesAsync(
                        editor._elementWithChangeStates,
                        editor._extractCts.Token);
                else
                    editor.Configuration.ElementsGateway.SaveChanges(
                        editor._elementWithChangeStates);
                editor._elementWithChangeStates.Clear();
            }
            catch (Exception e)
            {
                DataValidationErrors.SetError(editor, e);                
            }
            finally
            {
                editor.IsBusy = false;
                editor.UpdateCanExecuteCommands();
            }
        }

        private async void InvalidateElements()
        {            
            if (IsBusy)
                return;            
            if (_elementsPresenter != null && _configuration != null)
            {
                var gateway = _isShowChanges 
                    ? _localElementsGateway 
                    : _configuration.ElementsGateway;
                try
                {
                    _extractCts = new CancellationTokenSource();
                    IsBusy = true;
                    UpdateCanExecuteCommands();

                    var elementsQuery = new ElementsQuery(
                            _configuration.UsePaggination ? (_currentPage-1) * _configuration.PageSize : (int?)null,
                            _configuration.UsePaggination ? _configuration.PageSize : (int?)null,
                            !_isShowChanges
                                    ? _elementWithChangeStates.Where(a => a.IsRemoved)
                                          .Select(a => a.Id)
                                          .ToList()
                                          .NullIfEmpty()
                                    : null,
                            _selectedFilter
                        );
                    
                    var newElements = _configuration.UseAsync
                            ? await gateway.GetElementsAsync(elementsQuery, _extractCts.Token)
                            : gateway.GetElements(elementsQuery);                    

                    for (int i = 0; i < newElements.Length; i++)
                    {                                             
                        var changedElement = _elementWithChangeStates.FirstOrDefault(a => a == newElements[i]);
                        if (changedElement != null && changedElement.GetSingleState() == ElementState.Modified)
                            newElements[i] = changedElement;
                        newElements[i].StateWatcher = this;
                    }                        

                    _elementsPresenter!.ItemsSource = newElements;
                    ElementsCount = _configuration.UseAsync
                            ? await gateway.GetCountAsync(elementsQuery, _extractCts.Token)
                            : gateway.GetCount(elementsQuery);                    
                }
                catch (NotFoundQueryMapException) { throw; }
                catch (Exception e)
                {
                    DataValidationErrors.SetError(this, e);
                }
                finally
                {
                    IsBusy = false;
                    UpdateCanExecuteCommands();
                }
            }            
        }

        private void ResetThisState()
        {
            try
            {
                IsBusy = true;
                _elementWithChangeStates = new List<Element>();
                _localElementsGateway = new ElementsCollectionGateway<Element>(_elementWithChangeStates);
                SelectedFilter = null;
                IsShowChanges = false;
                CurrentPage = 0;
            }
            finally
            {
                IsBusy = false;
            }                                    
        }

        private void UpdateCanExecuteCommands()
        {
            _previousPageCommand.OnCanExecuteChanged();
            _nextPageCommand.OnCanExecuteChanged();
            _refreshCommand.OnCanExecuteChanged();
            _removeSelectedElementsCommand.OnCanExecuteChanged();
        }        

        void IElementsStateWatcher.OnChangeState(Element element)
        {
            if (!_elementWithChangeStates.Contains(element))
                _elementWithChangeStates.Add(element);
        }        
    }
}
