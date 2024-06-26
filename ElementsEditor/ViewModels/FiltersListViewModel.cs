﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElementsEditor
{   
	public class FiltersListViewModel : DialogViewModel
	{

		public Command AddNewFilterCommand { get; }
		public Command DeleteSelectdFilters { get; }

        public IEnumerable<PropertyFilterDescriptor> FilterDescriptors { get; }


		private PropertyFilterDescriptor? _selectedFilterDescriptor;
		public PropertyFilterDescriptor? SelectedFilterDescriptor
        {
			get => _selectedFilterDescriptor;
			set
			{
                SetAndRaisePropertyChanged(ref _selectedFilterDescriptor, value);
				UpdateCanExecuteChanged();
            } 
		}

		public ObservableCollection<IPropertyFilterModel> Filters { get; }
		public List<IPropertyFilterModel> SelectedFilters { get; }

        public FiltersListViewModel(
			IEnumerable<PropertyFilterDescriptor> filterDescriptors,
            ObservableCollection<IPropertyFilterModel> filters)
			: base(false)
        {
			SelectedFilters = new List<IPropertyFilterModel>();
            FilterDescriptors = filterDescriptors ?? throw new ArgumentNullException(nameof(filterDescriptors));
            Filters = filters ?? throw new ArgumentNullException(nameof(filterDescriptors));
			AddNewFilterCommand = new Command(
				param => Filters.Add(_selectedFilterDescriptor!.CreateFilterModel(Filters.Count == 0)),
				param => _selectedFilterDescriptor != null
			);
			DeleteSelectdFilters = new Command(
				param =>
				{
					foreach (var selectedFilter in SelectedFilters.ToArray())
						Filters.Remove(selectedFilter);
					if (Filters.Count > 0)
                        Filters[0].IsFirst = true;

                },
				param => SelectedFilters.Count > 0
			);
        }

		internal void UpdateCanExecuteChanged()
		{
			AddNewFilterCommand.OnCanExecuteChanged();
			DeleteSelectdFilters.OnCanExecuteChanged();
		}
     
	}

}
