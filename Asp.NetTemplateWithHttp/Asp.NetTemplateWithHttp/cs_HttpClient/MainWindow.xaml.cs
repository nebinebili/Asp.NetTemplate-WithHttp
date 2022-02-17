using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
using UserDatabase.Models;

namespace cs_HttpClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = null;
        string json = null;
        StringContent content = null;
        string requestUri = "http://localhost:4545";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_GetUser_Click(object sender, RoutedEventArgs e)
        {
            listbx_users.SelectedIndex = -1;
            txb_password.Text = null;
            txb_name.Text = null;
            txb_email.Text = null;

            response = client.GetAsync(requestUri).Result;

            json = response.Content.ReadAsStringAsync().Result;

            var users = JsonSerializer.Deserialize<List<User>>(json);

            listbx_users.ItemsSource = users;
        }

        private void btn_AddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txb_email.Text)
                                && !string.IsNullOrEmpty(txb_name.Text)
                                && !string.IsNullOrEmpty(txb_password.Text)
                                && listbx_users.SelectedIndex == -1
                                )
                {
                    var newuser = new User { Email = txb_email.Text, Name = txb_name.Text, Password = txb_password.Text };
                    json = JsonSerializer.Serialize(newuser);
                    content = new StringContent(json, Encoding.UTF8, "application/json");
                    response = client.PostAsync(requestUri, content).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("This User is aveilable!");
                        txb_password.Text = null;
                        txb_name.Text = null;
                        txb_email.Text = null;
                    }
                    else
                    {
                        MessageBox.Show("User Add Succesfully!");
                        txb_password.Text = null;
                        txb_name.Text = null;
                        txb_email.Text = null;
                    }
                    
                }
                else
                {
                    MessageBox.Show("Error");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        

        private void btn_UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listbx_users.SelectedIndex != -1)
                {
                    var selecteduser = (listbx_users.SelectedItem as User);
                    int _id=selecteduser.Id;
                    var user = new User { Id =_id, Name = txb_name.Text, Email = txb_email.Text, Password = txb_password.Text };

                    json = JsonSerializer.Serialize(user);
                    content = new StringContent(json, Encoding.UTF8, "application/json");
                    client.PutAsync(requestUri, content);

                    MessageBox.Show("User Update Succesfully!");
                    txb_password.Text = null;
                    txb_name.Text = null;
                    txb_email.Text = null;
                    listbx_users.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Select User!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void listbx_users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var user = (listbx_users.SelectedItem as User);
            if (listbx_users.SelectedIndex != -1)
            {
                txb_name.Text = user.Name;
                txb_email.Text = user.Email;
                txb_password.Text = user.Password;
            }
            
        }

        private void btn_DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if(listbx_users.SelectedIndex==-1 
                && string.IsNullOrEmpty(txb_name.Text) 
                && !string.IsNullOrEmpty(txb_email.Text)
                && !string.IsNullOrEmpty(txb_password.Text))
            {
                requestUri += $"?Email={txb_email.Text}&Password={txb_password.Text}";
                response=client.DeleteAsync(requestUri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("User is not aveilable!");
                    txb_password.Text = null;
                    txb_name.Text = null;
                    txb_email.Text = null;
                }
                else
                {
                    MessageBox.Show("User Delete Succesfully!");
                    txb_password.Text = null;
                    txb_name.Text = null;
                    txb_email.Text = null;
                }
                requestUri = "http://localhost:4545";
            }
            else
            {
                MessageBox.Show("Fill Email and Password");
            }          
        }
    }
}
