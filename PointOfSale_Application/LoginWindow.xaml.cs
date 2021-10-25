using System;
using System.Windows;
using PointOfSale_Application.Classes;

namespace PointOfSale_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                bool flag = StaffModel.VerifyLogin(usernameText.Text.ToString(), passwordText.Password.ToString());
                StaffModel staffObj = new StaffModel();
                staffObj = StaffModel.GetStaffRecordByUserPass(usernameText.Text.ToString(), passwordText.Password.ToString());

                if (flag)
                {
                    MainWindow main = new MainWindow();
                    this.Close();
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(MainWindow))
                        {
                            (window as MainWindow).staffid_Text.Text = staffObj.staff_id.ToString();
                            (window as MainWindow).staffname_Text.Text = staffObj.staff_name.ToString();
                        }
                    }
                    main.Show();
                    
                }
                else
                {
                    MessageBox.Show("wrong username or password");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
