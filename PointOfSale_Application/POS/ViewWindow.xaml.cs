using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PointOfSale_Application.Classes;

namespace PointOfSale_Application.POS
{
    /// <summary>
    /// Interaction logic for ViewWindow.xaml
    /// </summary>
    public partial class ViewWindow : Window
    {

        public ViewWindow()
        {
            InitializeComponent();
            
            srchText.TextChanged += srchText_TextChanged;
        }

        SqlConnection con = DBConnection.GetConnection();

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductModel product = (ProductModel)product_list.SelectedItem;
            Product_Transaction_Model temp_transaction = new Product_Transaction_Model();

            if (product_list.SelectedValue != null)
            {
                try
                {
                    if (qtyText.Text.Trim() == string.Empty)
                    {
                        throw new Exception("Please input the quantity of products to be transacted");
                    }

                    temp_transaction.prod_id = product.prod_id;
                    temp_transaction.qty_transacted = Convert.ToInt32(qtyText.Text);
                    temp_transaction.name = product.prod_name;
                    temp_transaction.total_amount = Convert.ToInt32(qtyText.Text) * product.msrp;

                    if (product.qty >= temp_transaction.qty_transacted && temp_transaction.qty_transacted > 0)
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(POS))
                            {
                                decimal amount = Convert.ToDecimal((window as POS).total_amount.Text);
                                amount += temp_transaction.total_amount;
                                (window as POS).total_amount.Text = Convert.ToString(amount);
                                (window as POS).products_table.Items.Add(temp_transaction);
                                product.qty -= Convert.ToInt32(qtyText.Text);
                                product_list.Items.Refresh();
                            }
                        }
                    }

                    else
                    {
                        MessageBox.Show("The quantity inputted is more than the stocks in the inventory or is an invalid number");
                    }
                }

                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }

                finally
                {
                    Clear();
                }
            }

        }

        public void Clear()
        {
            qtyText.Text = "";
            srchText.Text = "";
        }

        private void srchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var tbx = sender as TextBox;
                List<ProductModel> products = ProductModel.GetGoodProducts();

                if (tbx.Text != "")
                {
                    var filteredProduct = products.Where(x => x.prod_name.ToLower().Contains(tbx.Text.ToLower()));
                    product_list.ItemsSource = null;
                    product_list.ItemsSource = filteredProduct;
                }
                else
                {
                    product_list.ItemsSource = products;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
