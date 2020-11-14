using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using SkladSTovarami.Entities;
using SkladSTovarami.View;

namespace SkladSTovarami
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyContext db = new MyContext();

        public MainWindow()
        {
            InitializeComponent();
           
            dataGridWorker.ItemsSource = db.Emloyees.ToList().OrderBy(x => x.Login);
            if (db.Emloyees.FirstOrDefault(p => p.Role == "Администратор") != null)
            {
                button_Copy.Visibility = Visibility.Hidden;
            }
        }

        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridWorker.SelectedItem != null)
            {
                Autorization.Visibility = Visibility.Visible;
                Employee forlog = dataGridWorker.SelectedItem as Employee; //вытаскиваем логин
                textBox_log.Text = forlog.Login;
                passwordBox.Focus();
            }
            else
            {
                MessageBox.Show("Вы не выбрали никого", "Ошибка", MessageBoxButton.OK);
            }
        }

        private void Autorization_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Autorization.IsVisible == true)
            {
                button_Copy.IsEnabled = false;
                button.IsEnabled = false;
                dataGridWorker.IsEnabled = false;
            }
            else
            {
                button_Copy.IsEnabled = true;
                button.IsEnabled = true;
                dataGridWorker.IsEnabled = true;
                dataGridWorker.SelectedItem = null;
            }
        }

        private void button1_back_Click(object sender, RoutedEventArgs e)
        {
            Autorization.Visibility = Visibility.Hidden;
        }

        private void button1_enter_Click(object sender, RoutedEventArgs e)
        {            
            Employee forpass = dataGridWorker.SelectedItem as Employee;
            if (passwordBox.Password == forpass.Password)
            {
                if (forpass.Role == "Администратор")
                {
                    AdminWindow f1 = new AdminWindow();
                    f1.EmployeeId = forpass.Id;
                    f1.Show();
                    this.Hide();
                }
                else
                {
                    WorkerMain f1 = new WorkerMain();
                    f1.EmployeeId = forpass.Id;
                    f1.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Неправильный пароль, попробуйте ещё раз", "Ошибка", MessageBoxButton.OK);
                passwordBox.Password = null;
            }
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Create.Visibility = Visibility.Hidden;
            textBox.Text = null;
            textBox1.Text = null;
            textBox2.Text = null;
            passwordBox1.Password = null;
            passwordBox1_Copy.Password = null;
        }

        private void button_Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passwordBox1.Password == passwordBox1_Copy.Password)
                {
                    Employee emp = new Employee() { Name = textBox1.Text, Surname = textBox2.Text, Password = passwordBox1_Copy.Password, Login = textBox.Text, Role = "Администратор" };
                    db.Emloyees.Add(emp);
                    db.SaveChanges();
                    textBox.Text = null;
                    textBox1.Text = null;
                    textBox2.Text = null;
                    passwordBox1.Password = null;
                    passwordBox1_Copy.Password = null;
                    Create.Visibility = Visibility.Hidden;
                    dataGridWorker.ItemsSource = db.Emloyees.ToList().OrderBy(x => x.Login);
                    button_Create.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают", "Ошибка");
                    passwordBox1.Password = null;
                    passwordBox1_Copy.Password = null;
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Create.Visibility = Visibility.Visible;
        }

        private void Create_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Create.IsVisible == true)
            {
                button_Copy.IsEnabled = false;
                button.IsEnabled = false;
                dataGridWorker.IsEnabled = false;
            }
            else
            {
                button_Copy.IsEnabled = true;
                button.IsEnabled = true;
                dataGridWorker.IsEnabled = true;
                dataGridWorker.SelectedItem = null;
            }
        }

        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb1 = sender as TextBox;
            string pattern = @"^[a-zA-Z а-яА-Я ]+$";  //для имени
            Regex rgx = new Regex(pattern);
            string test = tb1.Text;
            if (!rgx.IsMatch(e.Text))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb1 = sender as TextBox;
            string pattern = @"^[a-zA-Z ]+$";   //логин только англ буквы
            Regex rgx = new Regex(pattern);
            string test = tb1.Text;
            if (!rgx.IsMatch(e.Text))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
    }
}
