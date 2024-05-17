using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor
{
    internal static class ShowDialogHelper
    {
        private static Dictionary<IElementsEditorViewModel, UserControl> _viewViewModels = new Dictionary<IElementsEditorViewModel, UserControl>();

        internal static void RegisterViewViewModel(UserControl view, IElementsEditorViewModel viewModel)
        {
            _viewViewModels[viewModel] = view;
        }

        internal static async Task<Element?> ShowAddNewElementDialog(this IElementsEditorViewModel viewModel)
        {
            var editorsView = _viewViewModels[viewModel];
            var modalWindow = new ModalWIndow();
            modalWindow.DataTemplates.AddRange(editorsView.DataTemplates);

            var addElementsVM = new AddElementViewModel(viewModel.ElementBuilders!);
            modalWindow.View = new AddNewElementView(addElementsVM);
            modalWindow.DataContext = addElementsVM;
            var parentWindow = (Window)TopLevel.GetTopLevel(editorsView)!;
            await modalWindow.ShowDialog(parentWindow);                        
            if (addElementsVM.Result == Result.Ok)
                return addElementsVM.ElementBuilder!.Build();
            return null;
        }

        internal static async Task ShowFiltersListDialog(this IElementsEditorViewModel viewModel)
        {
            var editorsView = _viewViewModels[viewModel];
            var modalWindow = new ModalWIndow();
            modalWindow.DataTemplates.AddRange(editorsView.DataTemplates);

            var filtersVm = new FiltersListViewModel(
                viewModel.FilterDescriptors!,
                viewModel.Filters);
            modalWindow.View = new FiltersListView(filtersVm);
            modalWindow.DataContext = filtersVm;
            var parentWindow = (Window)TopLevel.GetTopLevel(editorsView)!;
            await modalWindow.ShowDialog(parentWindow);
        }

        internal static async Task<bool> ShowHaveEditElementsDialog(this IElementsEditorViewModel viewModel)
        {
            var editorsView = _viewViewModels[viewModel];
            var modalWindow = new ModalWIndow();
            modalWindow.DataTemplates.AddRange(editorsView.DataTemplates);

            var warningVm = new WarningViewModel($"Имеются несохранённые изменения, продолжить ?");
            modalWindow.View = new WarningView(warningVm);
            modalWindow.DataContext = warningVm;
            var parentWindow = (Window)TopLevel.GetTopLevel(editorsView)!;
            await modalWindow.ShowDialog(parentWindow);
            return warningVm.Result == Result.Ok;

        }
    }
}
