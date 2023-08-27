using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ElementsEditor
{
    /// <summary>
    /// There are defined some classes, which can adding to the parent control
    /// for change visual state of elements 
    /// editor_toolbar_button					       -- all button, that are located in the upper tool panel
    /// editor_toolbar_items-count_textblock	       -- textBlock 'Count - n'
    /// editor_pagination_page-size_textblock          -- textBlock 'PageSize -'
	/// editor_pagination_textbox				       -- all textBoxes in the pagination panel
	/// editor_pagination_page_button			       -- buttons to switch pages
	/// editor_pagination_pages-count_textblock        -- textblock '/ nn'
	/// editor_filterring_apply-filter_button          -- button for apply filters	
	/// editor_filterring_delete-applied-filter_button -- button for delete apllied filters
	/// editor_filterring_add-filter_button			   -- button for add new filter
	/// editor_filterring_filter-descriptors_combobox  -- combobox for select descriptor of filter
	/// editor_filterring_listbox					   -- container of filter items
	/// editor_elements_listbox						   -- container of elements
	/// editor_filterring_listmode_button			   -- button witch switch filters view
	/// editor_toolbar_panel						   -- panel which wrapping toolbar
	/// editor_pagination_panel						   -- panel which wrapping pagination
	/// editor_filterring_panel						   -- panel which wrapping filters
	/// editor_filterring_filter_logic_combobox			
	/// editor_filterring_filter_propname_textblock
	/// editor_filterring_filter_operations_combobox
    /// </summary>
    public partial class ElementsEditorView : UserControl
	{		


		public ElementsEditorView()
		{												
			InitializeComponent();
			lbx_elements.SelectionChanged += (s, e) =>
			{
				var viewModel = DataContext as IElementsEditorViewModel;
				if (viewModel != null)
					viewModel.UpdateCommandsCanExecute();
			};
			lbx_filters.ItemsView.CollectionChanged += (s, e) =>
			{				
				btn_show_filters_list.IsVisible = lbx_filters.ItemsView.Count > 0;
            };
            btn_addNewItem.Click += AddNewElementButtonClick_Handler;
			btn_show_filters_list.Click += ShowFiltersListButtonCLick_Handler;
        }

		private async void ShowFiltersListButtonCLick_Handler(object sender, RoutedEventArgs e)
		{
            var viewModel = DataContext as IElementsEditorViewModel;
            if (viewModel != null)
            {
                var modealWindow = new ModalWIndow(DataTemplates,
					includeCancelButton: false);
                modealWindow.View = new FiltersListView(new FiltersListViewModel(
					viewModel.FilterDescriptors!,
					viewModel.Filters));
                var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
                await modealWindow.ShowDialog<Element>(parentWindow);                
            }			
        }


		private async void AddNewElementButtonClick_Handler(object sender, RoutedEventArgs e)
		{
            var viewModel = DataContext as IElementsEditorViewModel;
            if (viewModel != null)
            {
                var modealWindow = new ModalWIndow(DataTemplates);
				modealWindow.View = new AddNewElementView(new AddElementViewModel(viewModel.ElementBuilders!));
				var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
                var result = await modealWindow.ShowDialog<Element>(parentWindow);
				if (result != null)
					viewModel.AddNewElement(result);
            }
        }
        
	}
}