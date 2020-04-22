using MusicBoxWTS.Core.Models;
using MusicBoxWTS.Core.Services;
using MusicBoxWTS.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Transactions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MusicBoxWTS.Views
{
    public sealed partial class SpecialOrderPage : Page, INotifyPropertyChanged
    {
        List<States> statesList = new List<States>();
        List<ProdTypes> prodTypesList = new List<ProdTypes>();
        List<Suffix> suffixList = new List<Suffix>();
        List<SOStatus> soStatusList = new List<SOStatus>();
        private Boolean isLookupCustomer { get; set; } = false;
        private Boolean isLookupProduct { get; set; } = false;
        private int lookedupCustId { get; set; } = -1;
        private int lookedupProdId { get; set; } = -1;

        public ObservableCollection<MBCustomer> CustSource { get; } = new ObservableCollection<MBCustomer>();
        public ObservableCollection<MBProduct> ProdSource { get; } = new ObservableCollection<MBProduct>();
        public ObservableCollection<MBProduct> SOProdSource { get; } = new ObservableCollection<MBProduct>();
        public SpecialOrderPage()
        {
            InitializeComponent();

            #region States

            statesList.Add(new States("AL", "Alabama"));
            statesList.Add(new States("AK", "Alaska"));
            statesList.Add(new States("AZ", "Arizona"));
            statesList.Add(new States("AR", "Arkansas"));
            statesList.Add(new States("CA", "California"));
            statesList.Add(new States("CO", "Colorado"));
            statesList.Add(new States("CT", "Connecticut"));
            statesList.Add(new States("DE", "Delaware"));
            statesList.Add(new States("FL", "Florida"));
            statesList.Add(new States("GA", "Georgia"));
            statesList.Add(new States("HI", "Hawaii"));
            statesList.Add(new States("ID", "Idaho"));
            statesList.Add(new States("IL", "Illinois"));
            statesList.Add(new States("IN", "Indiana"));
            statesList.Add(new States("IA", "Iowa"));
            statesList.Add(new States("KS", "Kansas"));
            statesList.Add(new States("KY", "Kentucky"));
            statesList.Add(new States("LA", "Louisiana"));
            statesList.Add(new States("ME", "Maine"));
            statesList.Add(new States("MD", "Maryland"));
            statesList.Add(new States("MA", "Massachusetts"));
            statesList.Add(new States("MI", "Michigan"));
            statesList.Add(new States("MN", "Minnesota"));
            statesList.Add(new States("MS", "Mississippi"));
            statesList.Add(new States("MO", "Missouri"));
            statesList.Add(new States("MT", "Montana"));
            statesList.Add(new States("NE", "Nebraska"));
            statesList.Add(new States("NV", "Nevada"));
            statesList.Add(new States("NH", "New Hampshire"));
            statesList.Add(new States("NJ", "New Jersey"));
            statesList.Add(new States("NM", "New Mexico"));
            statesList.Add(new States("NY", "New York"));
            statesList.Add(new States("NC", "North Carolina"));
            statesList.Add(new States("ND", "North Dakota"));
            statesList.Add(new States("OH", "Ohio"));
            statesList.Add(new States("OK", "Oklahoma"));
            statesList.Add(new States("OR", "Oregon"));
            statesList.Add(new States("PA", "Pennsylvania"));
            statesList.Add(new States("RI", "Rhode Island"));
            statesList.Add(new States("SC", "South Carolina"));
            statesList.Add(new States("SD", "South Dakota"));
            statesList.Add(new States("TN", "Tennessee"));
            statesList.Add(new States("TX", "Texas"));
            statesList.Add(new States("UT", "UTAH"));
            statesList.Add(new States("VT", "Vermont"));
            statesList.Add(new States("VA", "Virginia"));
            statesList.Add(new States("WA", "Washington"));
            statesList.Add(new States("WV", "West Virginia"));
            statesList.Add(new States("WI", "Wisconsin"));
            statesList.Add(new States("WY", "Wyoming"));

            #endregion

            #region ProdTypes
            prodTypesList.Add(new ProdTypes("CD", "Compact Disc"));
            prodTypesList.Add(new ProdTypes("LP", "Long Playing Record"));
            prodTypesList.Add(new ProdTypes("45", "45 RPM Record"));
            prodTypesList.Add(new ProdTypes("DVD", "Digital Video Disc"));
            prodTypesList.Add(new ProdTypes("Blu-Ray", "Blu-Ray Multimedia Disc"));
            prodTypesList.Add(new ProdTypes("Poster", "Image for display on wall"));
            prodTypesList.Add(new ProdTypes("Incense", "Aromatic biotic material"));
            prodTypesList.Add(new ProdTypes("Stickers", "Stickers"));
            prodTypesList.Add(new ProdTypes("Clothing", "Wearable products"));
            prodTypesList.Add(new ProdTypes("Lighters", "Flame producing devices"));
            prodTypesList.Add(new ProdTypes("Cassettes", "Cassette Tapes"));
            prodTypesList.Add(new ProdTypes("Patches", "Clothing patches"));
            prodTypesList.Add(new ProdTypes("Accessories", "Miscellaneous"));
            prodTypesList.Add(new ProdTypes("CD+DVD", "Compact Disc with DVD"));
            prodTypesList.Add(new ProdTypes("Game", "Video or other games"));

            #endregion

            #region Suffix
            suffixList.Add(new Suffix("Default", ""));
            suffixList.Add(new Suffix("Junior", "Jr."));
            suffixList.Add(new Suffix("Senior", "Sr."));
            suffixList.Add(new Suffix("First", "I"));
            suffixList.Add(new Suffix("Second", "II"));
            suffixList.Add(new Suffix("Third", "III"));
            suffixList.Add(new Suffix("Fourth", "IV"));
            suffixList.Add(new Suffix("Fifth", "V"));
            suffixList.Add(new Suffix("Esquire", "Esq."));

            #endregion

            #region SOStatus
            soStatusList.Add(new SOStatus("Open", "Open"));
            soStatusList.Add(new SOStatus("Closed", "Closed"));
            soStatusList.Add(new SOStatus("Cancelled", "Cancelled"));

            #endregion
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

        private void bCustLkup_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Popup1.Height = Window.Current.Bounds.Height;
            Popup1.IsOpen = true;
        }

        private async void MenuFlyoutItem_Select(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext as MBCustomer;
            int indx = -1;

            isLookupCustomer = true;
            lookedupCustId = item.CustId;
            tbFName.Text = string.IsNullOrEmpty(item.CustFName) ? "" : item.CustFName;
            tbFName.IsEnabled = false;
            tbMI.Text = string.IsNullOrEmpty(item.CustMI) ? "" : item.CustMI;
            tbMI.IsEnabled = false;
            tbLName.Text = string.IsNullOrEmpty(item.CustLName) ? "" : item.CustLName;
            tbLName.IsEnabled = false;
            if (string.IsNullOrEmpty(item.CustSuffix)) { cbSuffix.SelectedIndex = -1; } else { cbSuffix.Text = item.CustSuffix; }
            cbSuffix.IsEnabled = false;
            tbAddr1.Text = string.IsNullOrEmpty(item.CustAddress1) ? "" : item.CustAddress1;
            tbAddr1.IsEnabled = false;
            tbAddr2.Text = string.IsNullOrEmpty(item.CustAddress2) ? "" : item.CustAddress2;
            tbAddr2.IsEnabled = false;
            tbCity.Text = string.IsNullOrEmpty(item.CustCity) ? "" : item.CustCity;
            tbCity.IsEnabled = false;
            //if (string.IsNullOrEmpty(item.CustState)) { cbStates.SelectedIndex = -1; } else { cbStates.Text = item.CustState; }

            foreach(var itm in cbStates.Items)
            {
                States st = (States)itm;
                if (st.ID.Equals(item.CustState.Trim()))
                {
                    cbStates.SelectedItem = itm;
                    break;
                }
            }

            cbStates.IsEnabled = false;
            tbZip.Text = string.IsNullOrEmpty(item.CustZip) ? "" : item.CustZip;
            tbZip.IsEnabled = false;
            tbEmail1.Text = string.IsNullOrEmpty(item.CustEmail1) ? "" : item.CustEmail1;
            tbEmail1.IsEnabled = false;
            tbEmail2.Text = string.IsNullOrEmpty(item.CustEmail2) ? "" : item.CustEmail2;
            tbEmail2.IsEnabled = false;
            tbPhone1.Text = string.IsNullOrEmpty(item.CustPhone1) ? "" : item.CustPhone1;
            tbPhone1.IsEnabled = false;
            tbPhone2.Text = string.IsNullOrEmpty(item.CustPhone2) ? "" : item.CustPhone2;
            tbPhone2.IsEnabled = false;

            Popup1.IsOpen = false;

            //ContentDialog mbCustDialog = new ContentDialog
            //{
            //    Title = "MBCustomer_Select",
            //    Content = item.ToString(),
            //    CloseButtonText = "Ok"
            //};

            //ContentDialogResult result = await mbCustDialog.ShowAsync();
        }


        private async void ProdMenuFlyoutItem_Select(object sender, RoutedEventArgs e)
        {

            var item = (sender as FrameworkElement).DataContext as MBProduct;

            isLookupProduct = true;
            lookedupProdId = item.ProdId;

            tbArtistName.Text = string.IsNullOrEmpty(item.ProdArtistName) ? "" : item.ProdArtistName;
            tbArtistName.IsEnabled = false;
            tbTitle.Text = string.IsNullOrEmpty(item.ProdTitle) ? "" : item.ProdTitle;
            tbTitle.IsEnabled = false;
            tbDesc.Text = string.IsNullOrEmpty(item.ProdDesc) ? "" : item.ProdDesc;
            tbDesc.IsEnabled = false;
            tbExtId.Text = string.IsNullOrEmpty(item.ProdExtId) ? "" : item.ProdExtId;
            tbExtId.IsEnabled = false;
            tbPrice.Text = string.IsNullOrEmpty(item.ProdPrice.ToString()) ? "" : item.ProdPrice.ToString();
            tbPrice.IsEnabled = false;
            tbVendor.Text = string.IsNullOrEmpty(item.ProdVendor) ? "" : item.ProdVendor;
            tbVendor.IsEnabled = false;

            foreach (var itm in cbProdType.Items)
            {
                ProdTypes pt = (ProdTypes)itm;
                if (pt.Name.Equals(item.ProdType.Trim()))
                {
                    cbProdType.SelectedItem = itm;
                    break;
                }
            }

            cbProdType.IsEnabled = false;

            Popup2.IsOpen = false;


            //ContentDialog mbProdDialog = new ContentDialog
            //{
            //    Title = "MBProduct Select",
            //    Content = item.ToString(),
            //    CloseButtonText = "Ok"
            //};

            //ContentDialogResult result = await mbProdDialog.ShowAsync();
        }

        private async void ProdMenuFlyoutItem_Remove(object sender, RoutedEventArgs e)
        {

            if (SOProdSource.Count > 0)
            {
                var item = (sender as FrameworkElement).DataContext as MBProduct;

                SOProdSource.Remove(item);

            }

            //ContentDialog mbProdDialog = new ContentDialog
            //{
            //    Title = "MBProduct Remove",
            //    Content = item.ToString(),
            //    CloseButtonText = "Ok"
            //};

            //ContentDialogResult result = await mbProdDialog.ShowAsync();
        }

        private async void Popup1_Opened(object sender, object e)
        {
            CustSource.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await MySQLDataService.GetCustGridDataAsync();

            foreach (var item in data)
            {
                CustSource.Add(item);
            }



            //ContentDialog mbPopupOpenDialog = new ContentDialog
            //{
            //    Title = "Popup Opened Event",
            //    Content = sender.ToString(),
            //    CloseButtonText = "Ok"
            //};

            //ContentDialogResult result = await mbPopupOpenDialog.ShowAsync();

        }

        private async void bSubmit_Click(object sender, RoutedEventArgs e)
        {
            MBCustomer newCust = new MBCustomer();
            int insertCustId = -1;
            List<int> insertProdIds = new List<int>();
            //List<MBProduct> prodInsertList = new List<MBProduct>();
            int insertSOId = -1;

            string fName = tbFName.Text;
            string lName = tbLName.Text;
            int itemCount = SOProdSource.Count;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    if((string.IsNullOrEmpty(tbFName.Text) && string.IsNullOrEmpty(tbLName.Text)))
                    {
                        throw new TransactionAbortedException("Customer must include a first or last name.");
                    }

                    if (SOProdSource.Count < 1)
                    {
                        throw new TransactionAbortedException("Special Order must have selected items.");
                    }
                


                    if (!isLookupCustomer)
                    {
                        newCust.CustFName = tbFName.Text;
                        newCust.CustMI = tbMI.Text;
                        newCust.CustLName = tbLName.Text;
                        newCust.CustSuffix = ((Suffix)cbSuffix.SelectedItem).Name;
                        newCust.CustAddress1 = tbAddr1.Text;
                        newCust.CustAddress2 = tbAddr2.Text;
                        newCust.CustCity = tbCity.Text;
                        newCust.CustState = ((States)cbStates.SelectedItem).ID;
                        newCust.CustZip = tbZip.Text;
                        newCust.CustEmail1 = tbEmail1.Text;
                        newCust.CustEmail2 = tbEmail2.Text;
                        newCust.CustPhone1 = tbPhone1.Text;
                        newCust.CustPhone2 = tbPhone2.Text;
                        newCust.CustTextNum = "";

                        insertCustId = MySQLDataService.insertCustomer(newCust);
                    }
                    else
                    {
                        insertCustId = lookedupCustId;
                    }

                    MBSpecialOrder mbso = new MBSpecialOrder();
                    mbso.SODate = DateTime.Now;
                    mbso.SOStatus = "Open";
                    mbso.SOCloseDate = new DateTime();
                    mbso.Customer_CustId = insertCustId;

                    insertSOId = MySQLDataService.insertSpecialOrder(mbso);

                    foreach (MBProduct prod in SOProdSource)
                    {
                        if (prod.ProdId == -1)
                        {
                            int pid = MySQLDataService.insertProduct(prod);
                            insertProdIds.Add(pid);
                        }
                        else
                        {
                            insertProdIds.Add(prod.ProdId);
                        }
                    }

                    foreach (int prodid in insertProdIds)
                    {
                        MBSpecialOrderLookup mbsol = new MBSpecialOrderLookup();
                        mbsol.Product_ProdId = prodid;
                        mbsol.SpecialOrder_SOId = insertSOId;
                        MySQLDataService.insertSOL(mbsol);
                    }

                    resetCustomer();
                    resetProduct();
                    SOProdSource.Clear();

                    scope.Complete();

                    ContentDialog mbTransSuccessDialog = new ContentDialog
                    {
                        Title = "Special Order Created!",
                        Content = "Special Order for customer " + fName + " " + lName + " of " + itemCount + " items created successfully!", 
                        CloseButtonText = "Ok"
                    };

                    ContentDialogResult result = await mbTransSuccessDialog.ShowAsync();

                }

            }
            catch (TransactionAbortedException tbe)
            {
                ContentDialog mbTransAbortedDialog = new ContentDialog
                {
                    Title = "Create Special Order Unsuccessful",
                    Content = "TransactionAbortedException thrown on insert of Special Order. " + Environment.NewLine + tbe.Message,
                    CloseButtonText = "Ok"
                };

                ContentDialogResult result = await mbTransAbortedDialog.ShowAsync();

            }
            catch (Exception exc)
            {
                ContentDialog mbExceptionDialog = new ContentDialog
                {
                    Title = "Create Special Order Failed",
                    Content = "Exception thrown on insert of Special Order. " + Environment.NewLine + exc.Message,
                    CloseButtonText = "Ok"
                };

                ContentDialogResult result = await mbExceptionDialog.ShowAsync();


            }

        }

        private void resetCustomer()
        {
            //Reset class properties
            lookedupCustId = -1;
            isLookupCustomer = false;

            //Clear form
            tbFName.Text = "";
            tbFName.IsEnabled = true;
            tbMI.Text = "";
            tbMI.IsEnabled = true;
            tbLName.Text = "";
            tbLName.IsEnabled = true;
            cbSuffix.SelectedIndex = -1;
            cbSuffix.IsEnabled = true;
            tbAddr1.Text = "";
            tbAddr1.IsEnabled = true;
            tbAddr2.Text = "";
            tbAddr2.IsEnabled = true;
            tbCity.Text = "";
            tbCity.IsEnabled = true;
            cbStates.SelectedIndex = -1;
            cbStates.IsEnabled = true;
            tbZip.Text = "";
            tbZip.IsEnabled = true;
            tbEmail1.Text = "";
            tbEmail1.IsEnabled = true;
            tbEmail2.Text = "";
            tbEmail2.IsEnabled = true;
            tbPhone1.Text = "";
            tbPhone1.IsEnabled = true;
            tbPhone2.Text = "";
            tbPhone2.IsEnabled = true;

        }

        private void resetProduct()
        {
            //Reset class properties
            lookedupProdId = -1;
            isLookupProduct = false;

            //Clear form
            tbArtistName.Text = "";
            tbArtistName.IsEnabled = true;
            tbTitle.Text = "";
            tbTitle.IsEnabled = true;
            tbDesc.Text = "";
            tbDesc.IsEnabled = true;
            tbExtId.Text = "";
            tbExtId.IsEnabled = true;
            tbPrice.Text = "";
            tbPrice.IsEnabled = true;
            tbVendor.Text = "";
            tbVendor.IsEnabled = true;
            cbProdType.SelectedIndex = -1;
            cbProdType.IsEnabled = true;
        }

        private void bCustReset_Click(object sender, RoutedEventArgs e)
        {
            resetCustomer();
        }

        private void bLookupItem_Click(object sender, RoutedEventArgs e)
        {
            Popup2.Height = Window.Current.Bounds.Height;
            Popup2.IsOpen = true;

        }

        private async void bAddItem_Click(object sender, RoutedEventArgs e)
        {
            MBProduct mbp = new MBProduct();
            double price = 0.0;

            if (string.IsNullOrEmpty(tbTitle.Text))
            {
                ContentDialog TitleEmptyDialog = new ContentDialog
                {
                    Title = "Item Not Added to List",
                    Content = "Item could not be added to list." + Environment.NewLine + "Title is empty",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await TitleEmptyDialog.ShowAsync();
            }
            else if(cbProdType.SelectedItem is null)
            {
                ContentDialog ProdTypeNullDialog = new ContentDialog
                {
                    Title = "Item Not Added to List",
                    Content = "Item could not be added to list." + Environment.NewLine + "User must select a Product Type before adding item to Special Order",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await ProdTypeNullDialog.ShowAsync();

            }
            else
            {
                if (isLookupProduct)
                    mbp.ProdId = lookedupProdId;
                else
                    mbp.ProdId = -1;

                mbp.ProdArtistName = tbArtistName.Text;
                mbp.ProdTitle = tbTitle.Text;
                mbp.ProdDesc = tbDesc.Text;
                Double.TryParse(tbPrice.Text, out price);
                mbp.ProdPrice = price;
                mbp.ProdType = ((ProdTypes)cbProdType.SelectedItem).Name;

                SOProdSource.Add(mbp);
            }
        }

        private void bResetItem_Click(object sender, RoutedEventArgs e)
        {
            isLookupProduct = false;
            resetProduct();
        }

        private async void Popup2_Opened(object sender, object e)
        {
            ProdSource.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await MySQLDataService.GetProdGridDataAsync();

            foreach (var item in data)
            {
                ProdSource.Add(item);
            }


        }
    }
}
