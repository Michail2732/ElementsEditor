using Avalonia.Controls;
using Avalonia.Media;
using ElementsEditor.Sample.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ElementsEditor.Sample
{
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {            
            InitializeComponent();
            DataContext = new MainWindowViewModel();            
            //btn_change_itemsSource.Click += (s, a) =>
            //{
            //    int count = 10000;
            //    var array = new string[count];
            //    for (int i = 0; i < count; i++)
            //    {
            //        array[i] = $"[{DateTime.Now}] item {i}";                    
            //    }
            //    var sw = new Stopwatch();
            //    sw.Start();
            //    lbx_items.ItemsSource = array;
            //    sw.Stop();
            //    tbl_renderTime.Text = sw.ElapsedMilliseconds.ToString() + "ms";                
            //    sw.Reset();
            //};                        
        }
    }
}