using PointOfSale_Application.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PointOfSale_Application.Inventory
{
    /// <summary>
    /// Interaction logic for ReturnToSellerPage.xaml
    /// </summary>
    public partial class ReturnToSellerPage : Page
    {
        public ReturnToSellerPage()
        {
            InitializeComponent();
            Update_Grid();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        private void rtsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (product_return_grid.SelectedItem != null)
            {
                try
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Would you like to confirm and create a receipt for this?", "Transaction Confirmation", System.Windows.MessageBoxButton.YesNo);
                    
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        int report_id = ReceiptModel.GetMaxID();
                        ProductModel transact = (ProductModel)product_return_grid.SelectedItem;

                        //Deduct from status_tbl
                        DeductTable(transact.prod_id, transact.qty);

                        //Save to product_in_transaction table
                        Product_Transaction_Model save_param = new Product_Transaction_Model();

                        save_param.report_id = report_id;
                        save_param.prod_id = transact.prod_id;
                        save_param.name = transact.prod_name;
                        save_param.qty_transacted = transact.qty;
                        save_param.total_amount = transact.msrp * transact.qty;
                        Product_Transaction_Model.Save_Production_Transaction(report_id, save_param);

                        //Save to receipt
                        ReceiptModel receipt = new ReceiptModel();

                        receipt.report_id = report_id;
                        receipt.staff_id = Convert.ToInt32(staff_IdText.Text);
                        receipt.sales_type = "RTS";
                        receipt.address = "Seller Address";
                        receipt.paid_amount = save_param.total_amount;
                        receipt.date_issued = DateTime.Now;
                        ReceiptModel.SaveReceipt(receipt);
                    }

                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
                finally
                {
                    Update_Grid();
                }
            }
            else
            {
                MessageBox.Show("Choose a product first");
            }
        }

        public void DeductTable(int id, int qty)
        {
            SqlConnection con = DBConnection.GetConnection();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.CommandText = "UPDATE product_status_tbl SET qty = qty - @qty WHERE prod_id = @prod_id AND status = 'Defective'";
                cmd.Parameters.AddWithValue("@prod_id", id);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                throw new Exception("Error in deducting the product table : " + error.Message);
            }
            finally
            {
                cmd.Parameters.Clear();
                con.Close();
            }
        }

        public void Update_Grid()
        {
            product_return_grid.ItemsSource = ProductModel.GetDefectiveProducts();
        }
    }
}
