using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ElementsEditor
{
    public interface IElementsEditorViewModel: INotifyPropertyChanged
    {
        int CurrentPage { get;  set; }
        int PageSize { get; set; }
        bool EnablePagination { get;  }
        bool EnableAsync { get; set; }
        bool EnableRemoving { get; }
        long ItemsCount { get; }
        int PagesCount { get; }
        bool IsBusy { get; }        

        #region elements
        ObservableCollection<Element> Elements { get; }
        IEnumerable<ElementBuilder>? ElementBuilders { get; }
        IList SelectedElements { get; set; }
        #endregion        

        #region filters
        ObservableCollection<IPropertyFilterModel> Filters { get; }
        IEnumerable<PropertyFilterDescriptor>? FilterDescriptors { get; }
        IPropertyFilterModel? SelectedFilter { get; set; }
        PropertyFilterDescriptor? SelectedFilterDescriptor { get; set; }
        string? AppliedFiltersInfo { get; }
        #endregion

        #region commands
        Command NextPageCommand { get; }
        Command PreviousPageCommand { get; }
        Command RefreshElementsCommand { get; }
        Command RemoveSelectedElementsCommand { get; }        
        Command SaveChangesCommand { get; }
        Command ApplyFiltersCommand { get; }
        Command AddNewElementCommand { get; }
        Command AddNewFilterCommand { get; }        
        Command ShowFiltersCommand { get; }
        Command DeleteAppliedFiltersCommand { get; }
        void UpdateCommandsCanExecute();
        #endregion
    }
}
