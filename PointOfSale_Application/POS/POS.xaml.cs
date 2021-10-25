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
    /// Interaction logic for POS.xaml
    /// </summary>
    public partial class POS : Window
    {
        private List<ProductModel> productModels = new List<ProductModel>();

        public POS()
        {
            InitializeComponent();
            date_now.Text = DateTime.Today.ToString("MM/dd/yyyy");
            productModels = ProductModel.GetGoodProducts();
        }

        #region Private buttons
        private void discountBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (products_table.SelectedItem != null)
                {
                    decimal discountInput = Convert.ToDecimal(discountText.Text);
                    if (discountInput > 0 && discountInput <= 100)
                    {
                        Product_Transaction_Model item = (Product_Transaction_Model)products_table.SelectedItem;
                        decimal orig_price = item.total_amount;
                        decimal less_price = orig_price * (decimal.Round(discountInput, 2) / 100);
                        item.total_amount = decimal.Round(orig_price - less_price, 2);
                        decimal temp_amount = decimal.Round(Convert.ToDecimal(total_amount.Text) - less_price, 2);

                        total_amount.Text = (temp_amount).ToString();
                        products_table.Items.Refresh();
                    }
                }

            }
            catch (FormatException)
            {
                MessageBox.Show("Please input only numbers on the discount input");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }

        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            Product_Transaction_Model temp = (Product_Transaction_Model)products_table.SelectedItem;
            var selectedItem = products_table.SelectedItem as Product_Transaction_Model;
            bool res = true;

            if (selectedItem != null)
            {
                /*
                foreach (ProductModel row in productModels)
                {
                    if (temp.prod_id == row.prod_id)
                    {
                        products_table.Items.Remove(selectedItem);
                        Decimal amount = Convert.ToDecimal(total_amount.Text);
                        amount -= temp.total_amount;
                        total_amount.Text = Convert.ToString(amount);
                        row.qty += temp.qty_transacted;

                    }
                }
                */

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(ViewWindow))
                    {
                        foreach (ProductModel row in (window as ViewWindow).product_list.Items)
                        {
                            if (temp.prod_id == row.prod_id)
                            {
                                products_table.Items.Remove(selectedItem);
                                Decimal amount = Convert.ToDecimal(total_amount.Text);
                                amount -= temp.total_amount;
                                total_amount.Text = Convert.ToString(amount);
                                row.qty += temp.qty_transacted;
                                (window as ViewWindow).product_list.Items.Refresh();
                                res = false;
                            }
                        }
                    }
                }
            }

            if (res)
            {
                MessageBox.Show("No item is selected or view window is not open");
            }

            
        }

        private void viewBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (!Application.Current.Windows.OfType<ViewWindow>().Any())
                {
                    ViewWindow view = new ViewWindow();
                    view.product_list.ItemsSource = productModels;
                    view.Show();
                }
            }
        }

        private void payBtn_Click(object sender, RoutedEventArgs e)
        {
            Transact_Cart("Payment");
        }

        private void refundBtn_Click(object sender, RoutedEventArgs e)
        {
            Transact_Cart("Refund");
        }

        private void clrBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Would you like to remove all records in the application?", "Remove Sales", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                clear();
            }
        }

        private void back_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        public void Transact_Cart(string typeOfButton)
        {
            List<Product_Transaction_Model> receiptList = new List<Product_Transaction_Model>();
            ReceiptModel receiptmodel = new ReceiptModel();
            decimal pay;
            decimal total;
            decimal amount_temp = 0;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Would you like to confirm and print digital receipt?", "Transaction Confirmation", System.Windows.MessageBoxButton.YesNo);
            try
            {
                pay = Convert.ToDecimal(paid_amount.Text);
                total = Convert.ToDecimal(total_amount.Text);
                if (messageBoxResult == MessageBoxResult.Yes && products_table.Items.Count != 0 && CheckMoney(pay, total))
                {
                    receiptmodel.report_id = ReceiptModel.GetMaxID();
                    receiptmodel.staff_id = Convert.ToInt32(staffId_Text.Text);
                    receiptmodel.sales_type = typeOfButton;
                    receiptmodel.address = address_block.Text;

                    if (!Application.Current.Windows.OfType<ReceiptWindow>().Any())
                    {
                        ReceiptWindow viewObj = new ReceiptWindow();
                        viewObj.Show();
                        foreach (Product_Transaction_Model row in products_table.Items) //This will loop through the contents of the datagrid
                        {
                            //Save all records to a list
                            amount_temp += row.total_amount;
                            receiptmodel.paid_amount = amount_temp;
                            receiptList.Add(GetTemp(receiptmodel.report_id, row));


                            //Save To Database
                            Product_Transaction_Model.Save_Production_Transaction(receiptmodel.report_id, row);

                            //Deduct from the database
                            if (receiptmodel.sales_type == "Payment")
                            {
                                TransactTable(row.prod_id, row.qty_transacted);
                            }
                            else if (receiptmodel.sales_type == "Refund")
                            {
                                RefundDefective(row.prod_id, row.qty_transacted);
                            }
                        }

                        //Pass all the data to the receipt window
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(ReceiptWindow))
                            {
                                (window as ReceiptWindow).receiptlist.ItemsSource = receiptList;
                                (window as ReceiptWindow).r_address.Text = address_block.Text;
                                (window as ReceiptWindow).total_amount.Text = amount_temp.ToString();
                                (window as ReceiptWindow).sales_id.Text = receiptmodel.report_id.ToString();
                                (window as ReceiptWindow).sales_type.Text = receiptmodel.sales_type.ToString();
                                (window as ReceiptWindow).r_pay.Text = paid_amount.Text;
                                (window as ReceiptWindow).r_change.Text = Pay(pay, total).ToString();
                            }
                        }
                        //Finally save the sales report to sales_report table
                        ReceiptModel.SaveReceipt(receiptmodel);
                        clear();
                    }
                }
                else if (messageBoxResult == MessageBoxResult.Yes && products_table.Items.Count == 0)
                {
                    MessageBox.Show("Cart is empty");
                }
                else if (messageBoxResult == MessageBoxResult.Yes && !CheckMoney(pay, total))
                {
                    MessageBox.Show("Insufficient pay amount");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Input only numbers on the paid amount");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        public void clear()
        {
            paid_amount.Text = "0";
            total_amount.Text = "0.00";
            discountText.Text = "";
            products_table.Items.Clear();
            productModels = ProductModel.GetGoodProducts();
        }

        public decimal Pay(decimal pay, decimal amount)
        {
            decimal result = 0;

            try
            {
                result = pay - amount;
                return result;
            }
            catch (FormatException)
            {
                throw new FormatException("Error input only numbers on the pay input");
            }
            catch
            {
                throw new Exception("Error in payment procedure");
            }
        }

        public bool CheckMoney(decimal pay, decimal amount)
        {
            try
            {
                decimal p1 = pay;
                decimal p2 = amount;

                if (p1 >= p2)
                    return true;

                return false;
            }
            catch (FormatException)
            {
                throw new Exception("Input only numbers on the input string");
            }
            catch
            {
                throw new Exception("Error in checkmoney function");
            }
        }

        public void TransactTable(int id, int qty)
        {
            SqlConnection con = DBConnection.GetConnection();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.CommandText = "UPDATE product_status_tbl SET qty = qty - @qty WHERE prod_id = @id AND status='Good'";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                throw new Exception("Error in transacting the product table : " + error.Message);
            }
            finally
            {
                cmd.Parameters.Clear();
                con.Close();
            }
        }

        public void RefundDefective(int id, int qty)
        {
            SqlConnection con = DBConnection.GetConnection();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            try
            {
                if (CheckProductDefectiveStatus(id))
                {
                    cmd.CommandText = "UPDATE product_status_tbl SET qty = qty + @qty WHERE prod_id = @id AND status='Defective'";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO product_status_tbl (status_id, prod_id, qty, status) VALUES (@status_id, @id, @qty, 'Defective')";
                    cmd.Parameters.AddWithValue("@status_id", ProductModel.GetMaxID("status"));
                }
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                throw new Exception("Error in refunding the products : " + error.Message);
            }
            finally
            {
                cmd.Parameters.Clear();
                con.Close();
            }
        }

        public bool CheckProductDefectiveStatus(int prod_id)
        {
            SqlConnection con = DBConnection.GetConnection();
            string query = "SELECT COUNT(1) FROM [product_status_tbl] WHERE prod_id = @prod_id AND status='Defective'";
            try
            {
                con.Open();
                SqlCommand sql = new SqlCommand(query, con);
                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@prod_id", prod_id);
                int count = Convert.ToInt32(sql.ExecuteScalar());

                if (count == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception error)
            {
                throw new Exception("Error in checking defective instance : " + error.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public Product_Transaction_Model GetTemp(int report_id, Product_Transaction_Model row)
        {
            Product_Transaction_Model temp = new Product_Transaction_Model();
            temp.report_id = report_id;
            temp.prod_id = row.prod_id;
            temp.name = row.name;
            temp.qty_transacted = row.qty_transacted;
            temp.total_amount = row.total_amount;

            return temp;
        }
    }
}
