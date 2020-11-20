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
    /// Логика взаимодействия для Products.xaml
    /// </summary>
    public partial class Products : Window
    {
        MyContext db = new MyContext();

        public Products()
        {
            InitializeComponent();
            List<string> s = new List<string>() { "Основной", "Дополнительный" };
            TypeGoods.ItemsSource = s;
            MainProdGrid.Visibility = Visibility.Hidden;
            List<string> v = new List<string>() { "Пользовательский", "%" };
            comboBox.ItemsSource = v;
            comboBox.SelectedItem = "Пользовательский";
            comboBox.SelectedValue = "Пользовательский";
        }

        private void TypeGoods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button_Save.IsEnabled = true;
            var s = TypeGoods.SelectedItem as string;

            if (s == "Основной")
            {
                MainProdGrid.Visibility = Visibility.Visible;
                SecondaryGrid.Visibility = Visibility.Hidden;
            }
            if (s == "Дополнительный")
            {
                MainProdGrid.Visibility = Visibility.Hidden;
                SecondaryGrid.Visibility = Visibility.Visible;
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (comboBox != null)
            {
                string s = comboBox.SelectedItem as string;
                if (s == "Пользовательский")
                    textBox2.Text = textBox1.Text;
                else
                    Set();
            }
        }

        private void Set()
        {
            if (textBox1.Text != "" && Percent.Text != "")
                textBox2.Text = Convert.ToString(Convert.ToDouble(textBox1.Text) + Convert.ToDouble(textBox1.Text) * Convert.ToDouble(Percent.Text) / 100.0);
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedItem.ToString() == "Пользовательский")
            {
                label_percent.Visibility = Visibility.Hidden;
                Percent.Visibility = Visibility.Hidden;
                textBox2.IsEnabled = true;
            }
            if (comboBox.SelectedItem.ToString() == "%")
            {
                label_percent.Visibility = Visibility.Visible;
                Percent.Visibility = Visibility.Visible;
                textBox2.IsEnabled = false;
            }
        }

        private void button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult res;
                res = MessageBox.Show("Вы уверены, что хотите сохранить товар?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (textBox_Id.Text == "-1")
                    {
                        if (TypeGoods.SelectedItem == null)
                        {
                            MessageBox.Show("Вы ничего не выбрали!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Question);
                        }
                        else if (TypeGoods.SelectedItem.ToString() == "Основной")
                        {
                            List<TextBox> s = MainProdGrid.Children.OfType<TextBox>().ToList();
                            foreach (TextBox q in s)
                            {
                                if (q.Text == "" || q.Text == "0")
                                {
                                    MessageBox.Show("Не все поля заполнены");
                                    return;
                                }
                            }
                            MainProduct f = new MainProduct();
                            f.Type = textBox_TypeProd.Text;
                            f.CodeName = textBox_NameProd.Text;
                            f.Brand = textBox_Brand.Text;
                            f.TireWidth = Convert.ToDouble(textBox_TireWidth.Text);
                            f.Diameter = Convert.ToInt32(textBox_Diameter.Text);
                            f.SpeedNum = Convert.ToInt32(textBox_SpeedNum.Text);
                            f.Used = checkBox.IsChecked.Value;
                            f.Info = textBox_Info.Text;

                            Product god = new Product()
                            {
                                Balance = Convert.ToInt32(textBox.Text),
                                PricePurchase = Convert.ToDouble(textBox1.Text),
                                SellPrice = Convert.ToDouble(textBox2.Text),
                                MainProduct = f
                            };
                            db.Product.Add(god);
                            db.SaveChanges();
                            this.Close();
                        }
                        else if (TypeGoods.SelectedItem.ToString() == "Дополнительный")
                        {
                            List<TextBox> s = SecondaryGrid.Children.OfType<TextBox>().ToList();
                            foreach (TextBox q in s)
                            {
                                if (q.Text == "" || q.Text == "0")
                                {
                                    MessageBox.Show("Не все поля заполнены");
                                    return;
                                }
                            }
                            Secondary access = new Secondary();
                            access.Type = textBox_Type.Text;
                            access.Name = textBox_Name.Text;
                            access.Characteristics = textBox_Charact.Text;

                            Product god = new Product()
                            {
                                Secondary = access
                            };

                            god.Balance = Convert.ToInt32(textBox.Text);
                            god.PricePurchase = Convert.ToInt32(textBox1.Text);
                            god.SellPrice = Convert.ToInt32(textBox2.Text);
                            db.Product.Add(god);
                            db.SaveChanges();
                            this.Close();
                        }
                    }
                    else
                    {
                        Product f = db.Product.Find(Convert.ToInt32(textBox_Id.Text));
                        if (TypeGoods.SelectedItem.ToString() == "Основной")
                        {
                            MainProduct mainProduct = db.MainProducts.Find(Convert.ToInt32(textBox_Id.Text));
                            mainProduct.Type = textBox_TypeProd.Text;
                            mainProduct.CodeName = textBox_NameProd.Text;
                            mainProduct.Brand = textBox_Brand.Text;
                            mainProduct.TireWidth = Convert.ToDouble(textBox_TireWidth.Text);
                            mainProduct.Diameter = Convert.ToInt32(textBox_Diameter.Text);
                            mainProduct.SpeedNum = Convert.ToInt32(textBox_SpeedNum.Text);
                            mainProduct.Used = checkBox.IsChecked.Value;
                            mainProduct.Info = textBox_Info.Text;
                            f.Balance = Convert.ToInt32(textBox.Text);
                            f.PricePurchase = Convert.ToInt32(textBox1.Text);
                            f.SellPrice = Convert.ToInt32(textBox2.Text);
                            db.SaveChanges();
                        }
                        else if (TypeGoods.SelectedItem.ToString() == "Дополнительный")
                        {
                            Secondary secondary = db.Secondaries.Find(Convert.ToInt32(textBox_Id.Text));
                            secondary.Type = textBox_Type.Text;
                            secondary.Name = textBox_Name.Text;
                            secondary.Characteristics = textBox_Charact.Text;
                            f.Balance = Convert.ToInt32(textBox.Text);
                            f.PricePurchase = Convert.ToInt32(textBox1.Text);
                            f.SellPrice = Convert.ToInt32(textBox2.Text);
                            db.SaveChanges();
                        }
                        this.Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void textBox_tireWidth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox Tb1 = sender as TextBox;
            if (Char.IsDigit(e.Text, 0))
            {
                if (!Tb1.Text.Contains(","))
                {
                    e.Handled = false;
                }
                else
                {
                    if (Tb1.Text.Length - Tb1.Text.IndexOf(",") >= 2) //колич знаков после запятой не больше 1
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
            }
            else if ((e.Text == "," || e.Text == ".") && Tb1.Text != string.Empty)
            {
                if (Tb1.Text.Contains(","))
                {
                    e.Handled = true;
                }
                else
                {
                    Tb1.Text += ",";
                    e.Handled = true;
                    Tb1.Select(Tb1.Text.Length, Tb1.Text.Length);
                }
            }
            else e.Handled = true;
        }

        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox Tb1 = sender as TextBox;
            if (Char.IsDigit(e.Text, 0))
            {
                if (!Tb1.Text.Contains(","))
                {
                    e.Handled = false;
                }
                else
                {
                    if (Tb1.Text.Length - Tb1.Text.IndexOf(",") >= 3) //ТОЖЕ ИСПРАВИТЬ
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
            }
            else if ((e.Text == "," || e.Text == ".") && Tb1.Text != string.Empty)
            {
                if (Tb1.Text.Contains(","))
                {
                    e.Handled = true;
                }
                else
                {
                    Tb1.Text += ",";
                    e.Handled = true;
                    Tb1.Select(Tb1.Text.Length, Tb1.Text.Length);
                }
            }
            else e.Handled = true;
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox Tb1 = sender as TextBox;
            if (Char.IsDigit(e.Text, 0))
            {
                e.Handled = false;
            }
            else e.Handled = true;
        }

        private void textBox_tireWidth_LostFocus(object sender, RoutedEventArgs e)//???????????????
        {
            TextBox tb1 = sender as TextBox;
            if (tb1.Text.IndexOf(",") == tb1.Text.Length - 1)
            {
                tb1.Text += "0";
            }
            if (tb1.Text[0] == '0')
                tb1.Text = tb1.Text.TrimStart(new char[] { '0' });
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb1 = sender as TextBox;
            if (tb1.Text == "0")
                tb1.Text = "";
        }

        private void Percent_TextChanged(object sender, TextChangedEventArgs e)
        {
            Set();
        }

        private void Percent_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox Tb1 = sender as TextBox;
            if (Char.IsDigit(e.Text, 0))
            {
                if (!Tb1.Text.Contains(","))
                {
                    e.Handled = false;
                }
                else
                {
                    if (Tb1.Text.Length - Tb1.Text.IndexOf(",") >= 3)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
            }
            else if ((e.Text == "," || e.Text == ".") && Tb1.Text != string.Empty)
            {
                if (Tb1.Text.Contains(","))
                {
                    e.Handled = true;
                }
                else
                {
                    Tb1.Text += ",";
                    e.Handled = true;
                    Tb1.Select(Tb1.Text.Length, Tb1.Text.Length);
                }
            }
            else e.Handled = true;

            if (Tb1.Text.Length < 5)
                e.Handled = false;
            else e.Handled = true;
        }
    }
}
