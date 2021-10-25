using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PointOfSale_Application.Classes;

namespace PointOfSale_Application.Inventory
{
    /// <summary>
    /// Interaction logic for InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow : Window
    {
        private List<ProductModel> goodproductlist = new List<ProductModel>();
        private List<ProductModel> badproductlist = new List<ProductModel>();
        public InventoryWindow()
        {
            InitializeComponent();
            try
            {
                goodproductlist = ProductModel.GetGoodProducts();
                badproductlist = ProductModel.GetDefectiveProducts();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }

        private void back_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void receivingBtn_Click(object sender, RoutedEventArgs e)
        {
            ReceivingPage receivingPage = new ReceivingPage();
            Inventory_Frame.Content = receivingPage;
        }

        private void rtsBtn_Click(object sender, RoutedEventArgs e)
        {
            ReturnToSellerPage returnToSellerPage = new ReturnToSellerPage();
            returnToSellerPage.staff_IdText.Text = staff_IdText.Text;
            Inventory_Frame.Content = returnToSellerPage;
        }

        private void displayAllBtn_Click(object sender, RoutedEventArgs e)
        {
            Inventory_Frame.Content = new ItemMovementPage();
        }

        private void receiptsBtn_Click(object sender, RoutedEventArgs e)
        {
            Inventory_Frame.Content = new ReceiptPage();
        }

        private void empBtn_Click(object sender, RoutedEventArgs e)
        {
            Inventory_Frame.Content = new EmployeePage();
        }
    }
}
