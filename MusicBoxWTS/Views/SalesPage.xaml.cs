using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;

using MusicBoxWTS.Core.Models;
using MusicBoxWTS.Core.Services;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Data;
using System.Collections.Generic;

namespace MusicBoxWTS.Views
{
    public sealed partial class SalesPage : Page, INotifyPropertyChanged
    {
        public SalesPage()
        {
            InitializeComponent();
            dpSalesDate.SelectedDate = DateTime.Today;
        }

        public ObservableCollection<MBSales> SalesSource { get; } = new ObservableCollection<MBSales>();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            try
            {
                base.OnNavigatedTo(e);
                SalesSource.Clear();

                // TODO WTS: Replace this with your actual data
                var data = await MySQLDataService.GetGridDataAsync();

                foreach (var item in data)
                {
                    SalesSource.Add(item);
                }

                dg_SortSales(SalesGrid, new DataGridColumnEventArgs(SalesGrid.Columns[1]));
            }
            catch (Exception eSales)
            {
                ContentDialog SalesDBException = new ContentDialog
                {
                    Title = "Sales - Database Exception",
                    Content = "Exception Message: " + eSales.Message + Environment.NewLine + "InnerException Message: " + eSales.InnerException.Message,
                    CloseButtonText = "Ok"
                };

                ContentDialogResult result = await SalesDBException.ShowAsync();

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void btnSubmit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
                //ContentDialog noWifiDialog = new ContentDialog
                //{
                //    Title = "No wifi connection",
                //    Content = "Check your connection and try again.",
                //    CloseButtonText = "Ok"
                //};

                //ContentDialogResult result = await noWifiDialog.ShowAsync();

            try
            {
                MBSales mbs = new MBSales();

                if (dpSalesDate.SelectedDate.HasValue)
                {
                    DateTimeOffset dto = dpSalesDate.SelectedDate.Value;
                    mbs.SalesDate = dto.DateTime;
                }

                mbs.SalesAmt = Double.Parse(tbSalesAmount.Text);
                mbs.SalesNotes = tbNotes.Text;
                mbs.User = "Ray";

                //ContentDialog mbSalesDialog = new ContentDialog
                //{
                //    Title = "MBSales",
                //    Content = mbs.ToString(),
                //    CloseButtonText = "Ok"
                //};

                //ContentDialogResult result3 = await mbSalesDialog.ShowAsync();
                MySQLDataService.insertSales(mbs);

                SalesSource.Clear();

                tbSalesAmount.Text = "0.00";
                tbNotes.Text = "";
                btnSubmit.IsEnabled = false;

                var data = await MySQLDataService.GetGridDataAsync();

                foreach (var item in data)
                {
                    SalesSource.Add(item);
                }

                SalesGrid.Columns[1].SortDirection = DataGridSortDirection.Ascending;

                dg_SortSales(SalesGrid, new DataGridColumnEventArgs(SalesGrid.Columns[1]));

            }
            catch (Exception ex)
            { 
  
                ContentDialog insertExceptionDialog = new ContentDialog
                {
                    Title = "Error inserting row in Sales table",
                    Content = ex.Message,
                    CloseButtonText = "Ok"
                };

                ContentDialogResult result2 = await insertExceptionDialog.ShowAsync();
            }


        }

        private void tbSalesAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            Double tbValue = 0.0;
            if (Double.TryParse(tbSalesAmount.Text, out tbValue)){ 
                if (tbValue > 0.00)
                {
                    btnSubmit.IsEnabled = true;
                }
            }
            else
            {
                btnSubmit.IsEnabled = false;
            }
        }

        private void SalesGrid_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {

        }

        //private async void MyMenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    var item = (sender as FrameworkElement).DataContext as MBSales;
        //    ContentDialog mbSalesDialog = new ContentDialog
        //    {
        //        Title = "MBSales_MyMenuItem",
        //        Content = item.ToString(),
        //        CloseButtonText = "Ok"
        //    };

        //    ContentDialogResult result = await mbSalesDialog.ShowAsync();

        //}

        private async void MenuFlyoutItem_Copy(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext as MBSales;
            ContentDialog mbSalesDialog = new ContentDialog
            {
                Title = "MBSales_Copy",
                Content = item.ToString(),
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await mbSalesDialog.ShowAsync();
        }

        private async void MenuFlyoutItem_Delete(object sender, RoutedEventArgs e)
        {
            var row = (sender as FrameworkElement).DataContext as MBSales;
            //ContentDialog mbSalesDialog = new ContentDialog
            //{
            //    Title = "MBSales_Delete",
            //    Content = row.ToString(),
            //    CloseButtonText = "Ok"
            //};

            //ContentDialogResult result = await mbSalesDialog.ShowAsync();

            ContentDialog deleteSalesRowDialog = new ContentDialog
            {
                Title = "Delete row permanently?",
                Content = "Are you sure you want to delete this row from the database?" + Environment.NewLine + row.ToString(),
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteSalesRowDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                MySQLDataService.deleteSales(row.SalesId);

                SalesSource.Clear();

                tbSalesAmount.Text = "0.00";
                tbNotes.Text = "";

                var data = await MySQLDataService.GetGridDataAsync();

                foreach (var item in data)
                {
                    SalesSource.Add(item);
                }
            }
        }

        private void dg_SortSales(object sender, DataGridColumnEventArgs e)
        {
            if (e.Column.Tag.ToString() == "Sales Date")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    List<MBSales> myList = SalesSource.ToList<MBSales>();
                    myList.Sort(new MBSales.SortBySalesDateAsc());

                    SalesSource.Clear();

                    foreach (MBSales item in myList)
                    {
                        SalesSource.Add(item);
                    }

                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                    
                }
                else //sort descending
                {
                    List<MBSales> myList = SalesSource.ToList<MBSales>();
                    myList.Sort(new MBSales.SortBySalesDateDesc());

                    SalesSource.Clear();

                    foreach (MBSales item in myList)
                    {
                        SalesSource.Add(item);
                    }

                    e.Column.SortDirection = DataGridSortDirection.Descending;

                }
            }


            if (e.Column.Tag.ToString() == "Sales Amount")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    List<MBSales> myList = SalesSource.ToList<MBSales>();
                    myList.Sort(new MBSales.SortBySalesAmtAsc());

                    SalesSource.Clear();

                    foreach (MBSales item in myList)
                    {
                        SalesSource.Add(item);
                    }

                    e.Column.SortDirection = DataGridSortDirection.Ascending;

                }
                else //sort descending
                {
                    List<MBSales> myList = SalesSource.ToList<MBSales>();
                    myList.Sort(new MBSales.SortBySalesAmtDesc());

                    SalesSource.Clear();

                    foreach (MBSales item in myList)
                    {
                        SalesSource.Add(item);
                    }

                    e.Column.SortDirection = DataGridSortDirection.Descending;

                }
            }


            if (e.Column.Tag.ToString() == "Notes")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    List<MBSales> myList = SalesSource.ToList<MBSales>();
                    myList.Sort(new MBSales.SortBySalesNotesAsc());

                    SalesSource.Clear();

                    foreach (MBSales item in myList)
                    {
                        SalesSource.Add(item);
                    }

                    e.Column.SortDirection = DataGridSortDirection.Ascending;

                }
                else //sort descending
                {
                    List<MBSales> myList = SalesSource.ToList<MBSales>();
                    myList.Sort(new MBSales.SortBySalesNotesDesc());

                    SalesSource.Clear();

                    foreach (MBSales item in myList)
                    {
                        SalesSource.Add(item);
                    }

                    e.Column.SortDirection = DataGridSortDirection.Descending;

                }
            }
            if (e.Column.Tag.ToString() == "User")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    List<MBSales> myList = SalesSource.ToList<MBSales>();
                    myList.Sort(new MBSales.SortByUserAsc());

                    SalesSource.Clear();

                    foreach (MBSales item in myList)
                    {
                        SalesSource.Add(item);
                    }

                    e.Column.SortDirection = DataGridSortDirection.Ascending;

                }
                else //sort descending
                {
                    List<MBSales> myList = SalesSource.ToList<MBSales>();
                    myList.Sort(new MBSales.SortByUserDesc());

                    SalesSource.Clear();

                    foreach (MBSales item in myList)
                    {
                        SalesSource.Add(item);
                    }

                    e.Column.SortDirection = DataGridSortDirection.Descending;

                }
            }

        }
    }
}
