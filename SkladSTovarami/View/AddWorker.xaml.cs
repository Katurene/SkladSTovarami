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
using System.Windows.Shapes;
using SkladSTovarami.Entities;

namespace SkladSTovarami.View
{
    /// <summary>
    /// Логика взаимодействия для AddWorker.xaml
    /// </summary>
    public partial class AddWorker : Window
    {
        MyContext db = new MyContext();

        public AddWorker()
        {
            InitializeComponent();
            List<string> s = new List<string>() { "Администратор", "Пользователь" };
            comboBox_role.ItemsSource = s;
        }

        bool allow = false;

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res;
                res = MessageBox.Show("Сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    bool allow = true;
                    Employee nw = new Employee();
                    if (textBlock_Id.Text != "-1")
                    {
                        nw = db.Emloyees.Find(Convert.ToInt16(textBlock_Id.Text));
                    }
                    nw.Name = textBox_name.Text;
                    nw.Surname = textBox_surname.Text;
                    nw.Role = comboBox_role.SelectedItem as string;
                    if (passwordBox.Password == passwordBox_approve.Password)
                    {
                        nw.Password = passwordBox_approve.Password;
                    }
                    else
                    {
                        allow = false;
                        MessageBox.Show("Пароли не совпадают", "Ошибка");
                    }
                    Employee main = db.Emloyees.FirstOrDefault(x => x.Login == textBox_login.Text) as Employee;
                    if (main != null && main.Id != nw.Id)
                    {
                        allow = false;
                        MessageBox.Show("Логин занят", "Ошибка");
                    }
                    else
                    {
                        nw.Login = textBox_login.Text;
                    }
                    if (allow && textBlock_Id.Text == "-1" && allow)
                    {
                        db.Emloyees.Add(nw);
                        db.SaveChanges();
                        this.Close();
                    }
                    else if (allow && textBlock_Id.Text != "-1" && allow)
                    {
                        db.SaveChanges();
                        this.Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res;
            res = MessageBox.Show("Уверены что не хотите сохранять?", "Ошибка", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb1 = sender as TextBox;
            string pattern = @"^[a-zA-Z а-яА-Я ]+$";
            Regex rgx = new Regex(pattern);
            if (!rgx.IsMatch(e.Text))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void textBox_login_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb1 = sender as TextBox;
            string pattern = @"^[a-zA-Z ]+$";
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
