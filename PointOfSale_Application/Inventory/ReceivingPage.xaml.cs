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
    /// Interaction logic for ReceivingWindow.xaml
    /// </summary>
    public partial class ReceivingPage : Page
    {
        private List<ProductModel> products = ProductModel.GetGoodProducts();

        public ReceivingPage()
        {
            InitializeComponent();
            Update_Grid_Display();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void srch_id_combo_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tbx = sender as TextBox;

            try
            {
                if (tbx.Text != "")
                {
                    var filteredProduct = products.Where(x => x.prod_id.Equals(Convert.ToInt32(tbx.Text)));
                    product_receiving_grid.ItemsSource = null;
                    product_receiving_grid.ItemsSource = filteredProduct;
                }
                else
                {
                    product_receiving_grid.ItemsSource = ProductModel.GetGoodProducts();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Input only numbers in the id");
            }
        }

        private void srch_prodname_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var tbx = sender as TextBox;


                if (tbx.Text != "")
                {
                    var filteredProduct = products.Where(x => x.prod_name.ToLower().Contains(tbx.Text.ToLower()));
                    product_receiving_grid.ItemsSource = null;
                    product_receiving_grid.ItemsSource = filteredProduct;
                }
                else
                {
                    product_receiving_grid.ItemsSource = ProductModel.GetGoodProducts();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void filter_clrBtn_Click(object sender, RoutedEventArgs e)
        {
            srch_id.Text = null;
            srch_prodname.Text = null;
        }

        public void Update_Grid_Display()
        {
            product_receiving_grid.ItemsSource = ProductModel.GetGoodProducts();
        }

        public void Update_Stock(int prod_id, int qty)
        {
            SqlConnection con = DBConnection.GetConnection();
            string query = "UPDATE product_status_tbl SET qty = @qty WHERE prod_id = @prod_id AND status = 'Good'";

            SqlCommand sql = new SqlCommand(query, con);
            sql.CommandType = CommandType.Text;

            try
            {
                con.Open();
                sql.Parameters.AddWithValue("@qty", qty);
                sql.Parameters.AddWithValue("@prod_id", prod_id);
                sql.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                throw new Exception("Error in updating stock: " + error.Message);
            }
            finally
            {
                sql.Parameters.Clear();
                con.Close();
            }

        }

        public void Update_Price(int prod_id, decimal price)
        {
            SqlConnection con = DBConnection.GetConnection();
            string query = "UPDATE product_tbl SET msrp = @msrp WHERE prod_id = @prod_id";

            SqlCommand sql = new SqlCommand(query, con);
            sql.CommandType = CommandType.Text;

            try
            {
                con.Open();
                sql.Parameters.AddWithValue("@msrp", price);
                sql.Parameters.AddWithValue("@prod_id", prod_id);
                sql.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                throw new Exception("Error in updating price: " + error.Message);
            }
            finally
            {
                sql.Parameters.Clear();
                con.Close();
            }
        }

        public void Update_Name(int prod_id, string name)
        {
            SqlConnection con = DBConnection.GetConnection();
            string query = "UPDATE product_tbl SET name = @name WHERE prod_id = @prod_id";

            SqlCommand sql = new SqlCommand(query, con);
            sql.CommandType = CommandType.Text;

            try
            {
                con.Open();
                sql.Parameters.AddWithValue("@name", name);
                sql.Parameters.AddWithValue("@prod_id", prod_id);
                sql.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                throw new Exception("Error in updating name: " + error.Message);
            }
            finally
            {
                sql.Parameters.Clear();
                con.Close();
            }
        }

        private void stock_updBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (product_receiving_grid.SelectedItem != null)
                {
                    ProductModel productModel = (ProductModel)product_receiving_grid.SelectedItem;

                    if (stock_price.Text != "" && stock_qtyText.Text != "" && stock_name.Text != "")
                    {
                        Update_Stock(productModel.prod_id, Convert.ToInt32(stock_qtyText.Text));
                        Update_Price(productModel.prod_id, Convert.ToDecimal(stock_price.Text));
                        Update_Name(productModel.prod_id, stock_name.Text);
                    }
                    else
                    {
                        MessageBox.Show("Incomplete input detected in the update text boxes");
                    }
                }
                else
                {
                    MessageBox.Show("Select a product on the row to be updated");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            finally
            {
                Update_Grid_Display();
            }
        }

        private void stock_clrBtn_Click(object sender, RoutedEventArgs e)
        {
            stock_price.Text = "";
            stock_qtyText.Text = "";
        }

        private void add_addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int max_prodid = ProductModel.GetMaxID("product");
                int max_statusid = ProductModel.GetMaxID("status");

                if (add_prodname.Text != "" && add_brand.Text != "" && add_price.Text != "" && add_qty.Text != "")
                {
                    ProductModel.Add_ProductModel(max_prodid, add_prodname.Text, add_brand.Text, Convert.ToDecimal(add_price.Text));
                    ProductModel.Add_ProductQuantity(max_statusid, max_prodid, Convert.ToInt32(add_qty.Text));
                }
                else
                {
                    MessageBox.Show("Incomplete input in the process of adding product");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            finally
            {
                Update_Grid_Display();
            }
        }

        private void add_clrBtn_Click(object sender, RoutedEventArgs e)
        {
            add_prodname.Text = "";
            add_brand.Text = "";
            add_price.Text = "";
            add_qty.Text = "";
        }
    }
}
