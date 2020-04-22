using MusicBoxWTS.Core.Models;
using MusicBoxWTS.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Configuration;
using MusicBoxWTS.Helpers;
using System.Collections.Generic;

namespace MusicBoxWTS.Views
{
    public sealed partial class SpecialOrderMaintPage : Page, INotifyPropertyChanged
    {
        List<SOStatus> soStatusList = new List<SOStatus>();

        public ObservableCollection<MBSOMaint> SOMaintSource { get; } = new ObservableCollection<MBSOMaint>();
        public ObservableCollection<MBProduct> SOMaintProdSource { get; } = new ObservableCollection<MBProduct>();

        private int selectedSOId = -1;

        public SpecialOrderMaintPage()
        {
            InitializeComponent();

            #region SOStatus
            soStatusList.Add(new SOStatus("Open", "Open"));
            soStatusList.Add(new SOStatus("Closed", "Closed"));
            soStatusList.Add(new SOStatus("Cancelled", "Cancelled"));

            #endregion

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            LoadSOMaintSource("Open");
        }

        protected async void LoadSOMaintSource(string status)
        {
            SOMaintSource.Clear();

            var data = await MySQLDataService.GetSOMaintGridDataAsync(status);

            foreach (var item in data)
            {
                SOMaintSource.Add(item);
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

        private async void SOMaintMenuFlyoutItem_UpdateStatus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext as MBSOMaint;

            puUpdateStatus.IsOpen = true;

            string status = item.SOStatus;

            if (status.Equals("Open"))
                rbSOpen.IsChecked = true;
            else if (status.Equals("Closed"))
                rbSClosed.IsChecked = true;
            else
                rbSCancelled.IsChecked = true;

            selectedSOId = item.SOId;

            //ContentDialog mbSOUpdate = new ContentDialog
            //{
            //    Title = "SO Update Status",
            //    Content = item.ToString(),
            //    CloseButtonText = "Ok"
            //};

            //ContentDialogResult result = await mbSOUpdate.ShowAsync();
        }

        private void puUpdateStatus_Opened(object sender, object e)
        {

        }

        private async void radioButton_Clicked(object sender, object e)
        {
            var clicked = (RadioButton)sender;
            string selected = "";
            string status = "";

            if (clicked.Name.Contains("Open"))
                selected = "Open";
            else if (clicked.Name.Contains("Closed"))
                selected = "Closed";
            else
                selected = "Cancelled";

            ContentDialog confirmStatusChange = new ContentDialog
            {
                Title = "Change Special Order Status",
                Content = "Are you sure you want to change the status to " + selected + "?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };

            ContentDialogResult result = await confirmStatusChange.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                MySQLDataService.updateSOStatus(selected, selectedSOId);

            }

            puUpdateStatus.IsOpen = false;
            selectedSOId = -1;

            SOMaintProdSource.Clear();

            status = ((SOStatus)cbStatus.SelectedItem).Name;

            LoadSOMaintSource(status);
        }

        private async void SOProdGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Microsoft.Toolkit.Uwp.UI.Controls.DataGrid pGrid = (Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)sender;

            MBSOMaint mbsom = (MBSOMaint)pGrid.SelectedItem;

            if (!(mbsom is null))
            {

                SOMaintProdSource.Clear();

                var data = await MySQLDataService.GetSOMaintProdGridDataAsync(mbsom.SOId);

                foreach (var item in data)
                {
                    SOMaintProdSource.Add(item);
                }

                //ContentDialog confirmStatusChange = new ContentDialog
                //{
                //    Title = "Selection Changed",
                //    Content = mbsom.ToString(),
                //    CloseButtonText = "Ok"
                //};

                //ContentDialogResult result = await confirmStatusChange.ShowAsync();
            }
        }

        private async void cbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var item = (Windows.UI.Xaml.Controls.ComboBox)sender;

            string soStatus =  ((SOStatus)(((ComboBox)sender).SelectedItem)).Name;

            LoadSOMaintSource(soStatus);

            //string mystring = "";


            //switch (soStatus)
            //{
            //    case "Open":
            //        LoadSOMaintSource()
            //        mystring = "Open";
            //        break;

            //    case "Complete":
            //        mystring = "Complete";
            //        break;

            //    case "Cancelled":
            //        mystring = "Cancelled";
            //        break;

            //    default:
            //        mystring = "Default";
            //        break;
            //}



            //ContentDialog confirmStatusChange = new ContentDialog
            //{
            //    Title = "Selection Changed",
            //    Content = mystring,
            //    CloseButtonText = "Ok"
            //};

            //ContentDialogResult result = await confirmStatusChange.ShowAsync();

        }
    }
}

