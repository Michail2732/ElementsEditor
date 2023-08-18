using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;

namespace ElementsEditor
{
	[TemplatePart("PART_ElementsHost", typeof(SelectingItemsControl))]
	public partial class ElementsEditorView : UserControl, IElementsStateWatcher
	{
		private IReadOnlyList<IPropertyFilter>? _applyiedFilters;
		private SelectingItemsControl? _elementsHostControl;
        private CancellationTokenSource? _extractCts;		
		private readonly ElementsCollectionGateway _changedItemsGateway;
		private const int _defaultCurrentPage = 1;

		public static readonly DirectProperty<ElementsEditorView, IElementsGateway> ItemsGatewayProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, IElementsGateway>(
				nameof(ItemsGateway),
				getter: o => o.ItemsGateway,
				setter: (o, v) => o.ItemsGateway = v);

		private IElementsGateway _itemsGateway;
		public IElementsGateway ItemsGateway
		{
			get { return _itemsGateway; }
			set { SetAndRaise(ItemsGatewayProperty, ref _itemsGateway, value); }
		}

		public static readonly DirectProperty<ElementsEditorView, int> CurrentPageProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, int>(
				nameof(CurrentPage), getter: o => o.CurrentPage, setter: (o, v) => o.CurrentPage = v);
		private int _currentPage = _defaultCurrentPage;
		public int CurrentPage
		{
			get { return _currentPage; }
			private set { SetAndRaise(CurrentPageProperty, ref _currentPage, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, int> PageSizeProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, int>(
				nameof(PageSize), getter: o => o.PageSize, setter: (o, v) => o.PageSize = v);
		private int _pageSize;
		public int PageSize
		{
			get { return _pageSize; }
			set { SetAndRaise(PageSizeProperty, ref _pageSize, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, bool> EnablePagginationProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, bool>(
				nameof(EnablePaggination), getter: o => o.EnablePaggination, setter: (o, v) => o.EnablePaggination = v);
		private bool _enablePaggination;
		public bool EnablePaggination
		{
			get { return _enablePaggination; }
			set { SetAndRaise(EnablePagginationProperty, ref _enablePaggination, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, bool> AllowPagginationSettingsProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, bool>(
				nameof(AllowPagginationSettings), getter: o => o.AllowPagginationSettings, setter: (o, v) => o.AllowPagginationSettings = v);
		private bool _allowPagginationSettings;
		public bool AllowPagginationSettings
		{
			get { return _allowPagginationSettings; }
			set { SetAndRaise(AllowPagginationSettingsProperty, ref _allowPagginationSettings, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, bool> EnableAsyncProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, bool>(
				nameof(EnableAsync), getter: o => o.EnableAsync, setter: (o, v) => o.EnableAsync = v);
		private bool _enableAsync;
		public bool EnableAsync
		{
			get { return _enableAsync; }
			set { SetAndRaise(EnableAsyncProperty, ref _enableAsync, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, bool> AllowAsyncSettingsProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, bool>(
				nameof(AllowAsyncSettings), getter: o => o.AllowAsyncSettings, setter: (o, v) => o.AllowAsyncSettings = v);
		private bool _allowAsyncSettings;
		public bool AllowAsyncSettings
		{
			get { return _allowAsyncSettings; }
			set { SetAndRaise(AllowAsyncSettingsProperty, ref _allowAsyncSettings, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, bool> EnableRemovingProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, bool>(
				nameof(EnableRemoving), getter: o => o.EnableRemoving, setter: (o, v) => o.EnableRemoving = v);
		private bool _enableRemoving;
		public bool EnableRemoving
		{
			get { return _enableRemoving; }
			set { SetAndRaise(EnableRemovingProperty, ref _enableRemoving, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, bool> AllowRemoveSettingsProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, bool>(
				nameof(AllowRemoveSettings), getter: o => o.AllowRemoveSettings, setter: (o, v) => o.AllowRemoveSettings = v);
		private bool _allowRemoveSettings;
		public bool AllowRemoveSettings
		{
			get { return _allowRemoveSettings; }
			set { SetAndRaise(AllowRemoveSettingsProperty, ref _allowRemoveSettings, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, bool> EnableAddingProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, bool>(
				nameof(EnableAdding), getter: o => o.EnableAdding, setter: (o, v) => o.EnableAdding = v);
		private bool _enableAdding;
		public bool EnableAdding
		{
			get { return _enableAdding; }
			set { SetAndRaise(EnableAddingProperty, ref _enableAdding, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, bool> AllowAddingSettingsProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, bool>(
				nameof(AllowAddingSettings), getter: o => o.AllowAddingSettings, setter: (o, v) => o.AllowAddingSettings = v);
		private bool _allowAddingSettings;
		public bool AllowAddingSettings
		{
			get { return _allowAddingSettings; }
			set { SetAndRaise(AllowAddingSettingsProperty, ref _allowAddingSettings, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, bool> EnableFiltersProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, bool>(
				nameof(EnableFilters),
				getter: o => o.EnableFilters,
				setter: (o, v) => o.EnableFilters = v);

		private bool _enableFilters;
		public bool EnableFilters
		{
			get { return _enableFilters; }
			set { SetAndRaise(EnableFiltersProperty, ref _enableFilters, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, long> ItemsCountProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, long>(
				nameof(ItemsCount), getter: o => o.ItemsCount);
		private long _itemsCount;
		public long ItemsCount
		{
			get { return _itemsCount; }
			private set { SetAndRaise(ItemsCountProperty, ref _itemsCount, value); }
		}



		public static readonly DirectProperty<ElementsEditorView, int> PagesCountProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, int>(
				nameof(PagesCount),
				getter: o => o.PagesCount);
		private int _pagesCount;
		public int PagesCount
		{
			get { return _pagesCount; }
			private set { SetAndRaise(PagesCountProperty, ref _pagesCount, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, bool> IsBusyProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, bool>(
				nameof(IsBusy), getter: o => o.IsBusy);
		private bool _isBusy;
		public bool IsBusy
		{
			get { return _isBusy; }
			private set { SetAndRaise(IsBusyProperty, ref _isBusy, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, IList> SelectedElementsProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, IList>(
				nameof(SelectedElements),
				getter: o => o.SelectedElements, 
				setter: (o, v) => o.SelectedElements = v);

		private IList _selectedElements;
		public IList SelectedElements
		{
			get { return _selectedElements; }
			private set { SetAndRaise(SelectedElementsProperty, ref _selectedElements, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, bool> ShowChangesProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, bool>(
				nameof(ShowChanges), getter: o => o.ShowChanges, setter: (o, v) => o.ShowChanges = v);
		private bool _showChanges;
		public bool ShowChanges
		{
			get { return _showChanges; }
			private set { SetAndRaise(ShowChangesProperty, ref _showChanges, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, ObservableCollection<Element>> ItemsProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, ObservableCollection<Element>>(
				nameof(Items), getter: o => o.Items);
		private ObservableCollection<Element> _items;
		public ObservableCollection<Element> Items
		{
			get { return _items; }
			private set { SetAndRaise(ItemsProperty, ref _items, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, ObservableCollection<IPropertyFilter>> FiltersProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, ObservableCollection<IPropertyFilter>>(
				nameof(Filters), getter: o => o.Filters);
		private ObservableCollection<IPropertyFilter> _filters;
		public ObservableCollection<IPropertyFilter> Filters
		{
			get { return _filters; }
			private set { SetAndRaise(FiltersProperty, ref _filters, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, object> SelectedFilterProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, object>(
				nameof(SelectedFilter), getter: o => o.SelectedFilter, setter: (o, v) => o.SelectedFilter = v);
		private object _selectedFilter;
		public object SelectedFilter
		{
			get { return _selectedFilter; }
			private set { SetAndRaise(SelectedFilterProperty, ref _selectedFilter, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, IEnumerable<PropertyFilterCreator>> FiltersSourceProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, IEnumerable<PropertyFilterCreator>>(
				nameof(FiltersSource), getter: o => o.FiltersSource, setter: (o, v) => o.FiltersSource = v);
		private IEnumerable<PropertyFilterCreator> _filtersSource;
		public IEnumerable<PropertyFilterCreator> FiltersSource
		{
			get { return _filtersSource; }
			set { SetAndRaise(FiltersSourceProperty, ref _filtersSource, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, Command> NextPageCommandProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, Command>(
				nameof(NextPageCommand), getter: o => o.NextPageCommand);
		private Command _nextPageCommand;
		public Command NextPageCommand
		{
			get { return _nextPageCommand; }
			private set { SetAndRaise(NextPageCommandProperty, ref _nextPageCommand, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, Command> PreviousPageCommandProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, Command>(
				nameof(PreviousPageCommand), getter: o => o.PreviousPageCommand);
		private Command _previousPageCommand;
		public Command PreviousPageCommand
		{
			get { return _previousPageCommand; }
			private set { SetAndRaise(PreviousPageCommandProperty, ref _previousPageCommand, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, Command> RefreshItemsCommandProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, Command>(
				nameof(RefreshItemsCommand), getter: o => o.RefreshItemsCommand);
		private Command _refreshItemsCommand;
		public Command RefreshItemsCommand
		{
			get { return _refreshItemsCommand; }
			private set { SetAndRaise(RefreshItemsCommandProperty, ref _refreshItemsCommand, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, Command> RemoveSelectedItemsCommandProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, Command>(
				nameof(RemoveSelectedItemsCommand), getter: o => o.RemoveSelectedItemsCommand);
		private Command _removeSelectedItemsCommand;
		public Command RemoveSelectedItemsCommand
		{
			get { return _removeSelectedItemsCommand; }
			private set { SetAndRaise(RemoveSelectedItemsCommandProperty, ref _removeSelectedItemsCommand, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, Command> RestoreSelectedItemsCommandProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, Command>(
				nameof(RestoreSelectedItemsCommand), getter: o => o.RestoreSelectedItemsCommand);
		private Command _restoreSelectedItemsCommand;
		public Command RestoreSelectedItemsCommand
		{
			get { return _restoreSelectedItemsCommand; }
			private set { SetAndRaise(RestoreSelectedItemsCommandProperty, ref _restoreSelectedItemsCommand, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, Command> SaveChangesCommandProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, Command>(
				nameof(SaveChangesCommand), getter: o => o.SaveChangesCommand);
		private Command _saveChangesCommand;
		public Command SaveChangesCommand
		{
			get { return _saveChangesCommand; }
			private set { SetAndRaise(SaveChangesCommandProperty, ref _saveChangesCommand, value); }
		}


		public static readonly DirectProperty<ElementsEditorView, Command> ApplyFiltersCommandProperty =
			AvaloniaProperty.RegisterDirect<ElementsEditorView, Command>(
				nameof(ApplyFiltersCommand), getter: o => o.ApplyFiltersCommand);
		private Command _applyFiltersCommand;
		public Command ApplyFiltersCommand
		{
			get { return _applyFiltersCommand; }
			private set { SetAndRaise(ApplyFiltersCommandProperty, ref _applyFiltersCommand, value); }
		}


		public ElementsEditorView()
		{
			_items = new ObservableCollection<Element>();			
            _filters = new ObservableCollection<IPropertyFilter>
            {
                //ermi
                new StringPropertyFilter(TryGetPropertyValue, "NameProp"),
                new IntPropertyFilter(TryGetPropertyValue, "SomeYo"),
                new DateTimePropertyFilter(TryGetPropertyValue, "HelloMan")
            };
			bool TryGetPropertyValue<TResult>(Element element, out TResult? result)
			{
				result = default!;
				return false;
			}
            _changedItemsGateway = new ElementsCollectionGateway(Array.Empty<Element>());
			NextPageCommand = new Command(
					param => CurrentPage++,
					param => !IsBusy && EnablePaggination && ItemsCount > (CurrentPage * PageSize));
			PreviousPageCommand = new Command(
				param => CurrentPage--,
				param => !IsBusy && EnablePaggination && CurrentPage > 1);
			RefreshItemsCommand = new Command(
				param => InvalidateItems(),
				param => !IsBusy);
			RemoveSelectedItemsCommand = new Command(
				param => RemoveSelectedItems(),
				param => !IsBusy && !ShowChanges && SelectedElements?.Count > 0);
			RestoreSelectedItemsCommand = new Command(
				param => RestoreSelectedItems(),
				param => !IsBusy && ShowChanges && SelectedElements?.Count > 0);
			SaveChangesCommand = new Command(
				param => SaveChanges(),
				param => !IsBusy && ShowChanges && _changedItemsGateway.Elements.Count > 0);
			ApplyFiltersCommand = new Command(
				param => ApplyFilters(),
				param => !IsBusy && Filters.Count > 0);
			InitializeComponent();
		}


        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
			if (_elementsHostControl != null)
				_elementsHostControl.SelectionChanged -= OnSelectedItemsChanged;

            _elementsHostControl = e.NameScope.Get<SelectingItemsControl>("PART_ElementsHost");
			if (_elementsHostControl != null)
                _elementsHostControl.SelectionChanged += OnSelectedItemsChanged;
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
		{
			base.OnPropertyChanged(change);
			switch (change.Property.Name)
			{
				case nameof(CurrentPage):
					InvalidateItems();
					break;
				case nameof(PageSize):
					_isBusy = true;
					CurrentPage = _defaultCurrentPage;
					_isBusy = false;
					InvalidateItems();
					break;
				case nameof(EnableFilters):
					if (!change.GetNewValue<bool>())
                        _applyiedFilters = null;
					InvalidateItems();
					break;
				case nameof(EnablePaggination):
					InvalidateItems();
					break;
				case nameof(ItemsGateway):
					InvalidateItems();
					break;
				case nameof(SelectedElements):
					UpdateCommandsCanExecute();					
                    break;
				case nameof(ShowChanges):
					InvalidateItems();
					break;
				case nameof(SelectedFilter):
                    UpdateCommandsCanExecute();
                    break;
				case nameof(FiltersSource):
					UpdateCommandsCanExecute();
                    break;
				default:
					break;
			}
		}

		private void UpdateCommandsCanExecute()
		{
			NextPageCommand.OnCanExecuteChanged();
			PreviousPageCommand.OnCanExecuteChanged();
			RefreshItemsCommand.OnCanExecuteChanged();
			RemoveSelectedItemsCommand.OnCanExecuteChanged();
			RestoreSelectedItemsCommand.OnCanExecuteChanged();
			SaveChangesCommand.OnCanExecuteChanged();
			ApplyFiltersCommand.OnCanExecuteChanged();
		}


		private void OnSelectedItemsChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateCommandsCanExecute();				
		}


		private void ApplyFilters()
		{
			_applyiedFilters = new List<IPropertyFilter>(_filters);
            _isBusy = true;
			CurrentPage = _defaultCurrentPage;
			_isBusy = false;
			InvalidateItems();
		}

		private void RestoreSelectedItems()
		{
			foreach (Element selectedItem in SelectedElements!)
			{
                selectedItem.ResetState();
                _changedItemsGateway.Elements.Remove(selectedItem);
            }
			InvalidateItems();
		}

		private void RemoveSelectedItems()
		{
			foreach (Element selectedItem in SelectedElements!)
			{
                selectedItem.State = ElementState.Removed;
				if (!_changedItemsGateway.Elements.Contains(selectedItem))
					_changedItemsGateway.Elements.Add(selectedItem);
            }				
			InvalidateItems();
		}

		private async void SaveChanges()
		{
			try
			{
				_extractCts = new CancellationTokenSource();
				IsBusy = true;				
				if (EnableAsync)
					await _itemsGateway.SaveChangesAsync(
                        _changedItemsGateway.Elements,
						_extractCts.Token);
				else
					_itemsGateway.SaveChanges(
                        _changedItemsGateway.Elements);
                _changedItemsGateway.Elements.Clear();            
            }
			catch (Exception e) 
			{ 
				DataValidationErrors.SetErrors(this, new[] { e.ToString() });
				return;
			}
			finally 
			{
				IsBusy = false;
				UpdateCommandsCanExecute();
			}
            InvalidateItems();
        }

		private async void InvalidateItems()
		{
            if (IsBusy || _itemsGateway is null)
                return;			
            try
			{                				
                if (DataValidationErrors.GetHasErrors(this))
					DataValidationErrors.ClearErrors(this);
                _extractCts = new CancellationTokenSource();
				IsBusy = true;
				UpdateCommandsCanExecute();
				
				var elementsQuery = new Query(
						_enablePaggination ? _pageSize * (_currentPage - _defaultCurrentPage) : null,
                        _enablePaggination ? _pageSize : null,
                        !_showChanges ? _changedItemsGateway.Elements.Select(a => a.Id).ToList().NullIfEmpty()
									  : null,
						_applyiedFilters 
					);

				var itemsGateway = _showChanges ? _changedItemsGateway : _itemsGateway;

				var newElements = _enableAsync
						? await itemsGateway.GetElementsAsync(elementsQuery, _extractCts.Token)
						: itemsGateway.GetElements(elementsQuery);

				ItemsCount = _enableAsync
						? await itemsGateway.GetCountAsync(elementsQuery, _extractCts.Token)
						: itemsGateway.GetCount(elementsQuery);

				if (_enablePaggination)
					PagesCount = (int)(_itemsCount / _pageSize) + (_itemsCount % _pageSize == 0 ? 0 : 1); 

				Items.Clear();
				for (int i = 0; i < newElements.Length; i++)
				{
					newElements[i].StateWatcher = this;
					Items.Add(newElements[i]);
				}
			}
			catch (Exception e) { DataValidationErrors.SetErrors(this, new[] { e.ToString() }); }
			finally
			{
				IsBusy = false;
				UpdateCommandsCanExecute();
			}
		}

		void IElementsStateWatcher.OnChangeState(Element element)
		{
			if (!_changedItemsGateway.Elements.Contains(element))
                _changedItemsGateway.Elements.Add(element);
		}
	}
}