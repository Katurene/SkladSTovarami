using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using SkladSTovarami.Entities;

namespace SkladSTovarami.View
{
    /// <summary>
    /// Логика взаимодействия для EditCustomer.xaml
    /// </summary>
    public partial class EditCustomer : Window
    {
        public EditCustomer()
        {
            InitializeComponent();
            MyContext db = new MyContext();
            dataGrid.ItemsSource = db.Customers.ToList();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer f = new AddCustomer();
            f.Closing += AddCustomer_Closing;
            f.ShowDialog();
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Customer cust = dataGrid.SelectedItem as Customer;
                AddCustomer f = new AddCustomer();
                f.textBox_surname.Text = cust.FullName.Split(' ')[0];
                f.textBox_name.Text = cust.FullName.Split(' ')[1];
                f.textBox_fathername.Text = cust.FullName.Split(' ')[2];
                f.textBox_Passport.Text = cust.PassportNumber;
                f.textBox_Contr.Text = cust.Contract;
                f.textBox_validfrom.Text = cust.ValidFrom.ToString("dd:MM:yyyy").Replace(":", ".");
                f.textBox_validto.Text = cust.ValidTo.ToString("dd:MM:yyyy").Replace(":", ".");
                f.Closing += AddCustomer_Closing;
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("Вы не выбрали никого", "Ошибка", MessageBoxButton.OK);
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGrid.SelectedItem != null)
                {
                    MyContext db = new MyContext();
                    Customer cust = dataGrid.SelectedItem as Customer;
                    Customer custdel = db.Customers.Find(cust.CustomerId);
                    db.Customers.Remove(custdel);
                    db.SaveChanges();

                    //новое - почти работает
                    MyContext db1 = new MyContext();
                    dataGrid.ItemsSource = db1.Customers.ToList();
                }
                else
                {
                    MessageBox.Show("Вы не выбрали никого", "Ошибка", MessageBoxButton.OK);
                }
            }
            catch
            {
                MessageBox.Show("Удалить чеки из продаж!", "Ошибка");
            }
        }

        void AddCustomer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MyContext db = new MyContext();
            dataGrid.ItemsSource = db.Customers.ToList();
        }
    }
}
