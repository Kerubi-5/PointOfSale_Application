using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PointOfSale_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void transactionBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {

            foreach (Window window in Application.Current.Windows)
            {
                if (!Application.Current.Windows.OfType<POS.POS>().Any())
                {
                    POS.POS pOSWindow = new POS.POS();
                    pOSWindow.staffId_Text.Text = staffid_Text.Text;
                    pOSWindow.address_block.Text = staffname_Text.Text;
                    pOSWindow.Show();
                }
            }

        }

        private void inventoryBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {

            foreach (Window window in Application.Current.Windows)
            {
                if (!Application.Current.Windows.OfType<Inventory.InventoryWindow>().Any())
                {
                    Inventory.InventoryWindow inventoryWindow = new Inventory.InventoryWindow();
                    inventoryWindow.staff_IdText.Text = staffid_Text.Text;
                    inventoryWindow.Show();
                }

            }
        }

        private void logoutBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
