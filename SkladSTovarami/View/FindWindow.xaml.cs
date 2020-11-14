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
    /// Логика взаимодействия для FindWindow.xaml
    /// </summary>
    public partial class FindWindow : Window
    {
        public int Id
        {
            get; set;
        }

        MyContext db = new MyContext();

        public FindWindow()
        {
            InitializeComponent();
            textBox_spNum_from.Text = Convert.ToString(db.MainProducts.Min(x => x.SpeedNum));
            textBox_spNum_to.Text = Convert.ToString(db.MainProducts.Max(x => x.SpeedNum));
            textBox_diam_from.Text = Convert.ToString(db.MainProducts.Min(x => x.Diameter));
            textBox_diam_to.Text = Convert.ToString(db.MainProducts.Max(x => x.Diameter));            
            textBox_TireWidth.Text = Convert.ToString(db.MainProducts.Min(x => x.TireWidth));
            dataGrid_MainProd.ItemsSource = db.MainProducts.ToList();
            dataGrid_Secondaries.ItemsSource = db.Secondaries.ToList();
            List<string> s = new List<string>() { "Дополнительный", "Основной" };
            comboBox.ItemsSource = s;

            MainProd.Visibility = Visibility.Visible;
            checkBox.Visibility = Visibility.Visible;
            dataGrid_MainProd.Visibility = Visibility.Visible;
            Secondar.Visibility = Visibility.Hidden;
            dataGrid_Secondaries.Visibility = Visibility.Hidden;
            comboBox.SelectedItem = "Основной";
        }

        private void textBox_Brand_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox_Brand.Text != "")
            {
                SortMainProduct();
            }
        }

        private void SortMainProduct()
        {
            try
            {
                List<MainProduct> lst = db.MainProducts.ToList();

                if (checkBox1_name.IsChecked == true)
                {
                    lst = lst = lst.Where(x => x.CodeName.ToUpper().Contains(textBox_CodeName.Text.ToUpper())).ToList();
                }
                if (checkBox2_brand.IsChecked == true)
                {
                    lst = lst.Where((x => x.Brand.ToUpper().Contains(textBox_Brand.Text.ToUpper()))).ToList();
                }
                if (checkBox3_tireWidth.IsChecked == true)
                {
                    double tireWidth;
                    if (textBox_TireWidth.Text == "")
                    { }
                    else
                    {
                        tireWidth = Convert.ToDouble(textBox_TireWidth.Text);
                        lst = lst.Where(x => x.TireWidth == tireWidth).ToList();
                    }
                }
                if (checkBox4_spNum.IsChecked == true)
                {
                    int SpeedNumfrom;
                    int SpeedNumto;
                    if (textBox_spNum_from.Text == "")
                        SpeedNumfrom = 0;
                    else
                        SpeedNumfrom = Convert.ToInt16(textBox_spNum_from.Text);


                    if (textBox_spNum_to.Text == "")
                        SpeedNumto = db.MainProducts.Max(x => x.SpeedNum);
                    else
                        SpeedNumto = Convert.ToInt32(textBox_spNum_to.Text);

                    lst = lst.Where(x => x.SpeedNum >= SpeedNumfrom).ToList();
                    lst = lst.Where(x => x.SpeedNum <= SpeedNumto).ToList();
                }
                if (checkBox5_diam.IsChecked == true)
                {
                    int Diameterfrom;
                    int Diameterto;
                    if (textBox_diam_from.Text == "")
                        Diameterfrom = 0;
                    else
                        Diameterfrom = Convert.ToInt32(textBox_diam_from.Text);


                    if (textBox_diam_from.Text == "")
                        Diameterto = db.MainProducts.Max(x => x.Diameter);
                    else
                        Diameterto = Convert.ToInt32(textBox_diam_to.Text);

                    lst = lst.Where(x => x.Diameter >= Diameterfrom).ToList();
                    lst = lst.Where(x => x.Diameter <= Diameterto).ToList();
                }               
                if (checkBox.IsChecked == true)
                {
                    lst = lst.Where(x => x.Used == checkBox.IsChecked).ToList();
                }
                lst = lst.Where(x => x.Type.ToUpper().Contains(textBox_type.Text.ToUpper())).ToList();
                dataGrid_MainProd.ItemsSource = lst;
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private void textBox_CodeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox_CodeName.Text != "")
            {
                SortMainProduct();
            }
        }

        private void textBox_tireWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox_TireWidth.Text != "")
            {
                SortMainProduct();
            }
        }

        private void checkBox1_name_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox1_name.IsChecked == true)
            {
                textBox_CodeName.IsEnabled = true;
            }
            else
                textBox_CodeName.IsEnabled = false;
            SortMainProduct();
        }

        private void checkBox2_brand_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox2_brand.IsChecked == true)
            {
                textBox_Brand.IsEnabled = true;
            }
            else
                textBox_Brand.IsEnabled = false;
            SortMainProduct();
        }

        private void checkBox3_tireWidth_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox3_tireWidth.IsChecked == true)
            {
                textBox_TireWidth.IsEnabled = true;
            }
            else
                textBox_TireWidth.IsEnabled = false;
            SortMainProduct();
        }

        private void checkBox4_speedNum_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox4_spNum.IsChecked == true)
            {
                textBox_spNum_from.IsEnabled = true;
                textBox_spNum_to.IsEnabled = true;
            }
            else
            {
                textBox_spNum_from.IsEnabled = false;
                textBox_spNum_to.IsEnabled = false;
            }
            SortMainProduct();
        }

        private void checkBox5_diameter_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox5_diam.IsChecked == true)
            {
                textBox_diam_from.IsEnabled = true;
                textBox_diam_to.IsEnabled = true;
            }
            else
            {
                textBox_diam_from.IsEnabled = false;
                textBox_diam_to.IsEnabled = false;
            }
            SortMainProduct();
        }      

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            SortMainProduct();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedItem.ToString() == "Основной")
            {
                MainProd.Visibility = Visibility.Visible;
                checkBox.Visibility = Visibility.Visible;
                dataGrid_MainProd.Visibility = Visibility.Visible;
                Secondar.Visibility = Visibility.Hidden;
                dataGrid_Secondaries.Visibility = Visibility.Hidden;
                dataGrid_Secondaries.SelectedItem = null;
                dataGrid_MainProd.SelectedItem = null;
            }
            else
            {
                dataGrid_Secondaries.Visibility = Visibility.Visible;
                Secondar.Visibility = Visibility.Visible;
                MainProd.Visibility = Visibility.Hidden;
                dataGrid_MainProd.Visibility = Visibility.Hidden;
                checkBox.Visibility = Visibility.Hidden;
                dataGrid_Secondaries.SelectedItem = null;
                dataGrid_MainProd.SelectedItem = null;
            }
        }

        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked == true)
                textBox.IsEnabled = true;
            else
                textBox.IsEnabled = false;
            sortAccess();
        }

        private void checkBox2_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox2.IsChecked == true)
                textBox2.IsEnabled = true;
            else
                textBox2.IsEnabled = false;
            sortAccess();
        }

        private void sortAccess()
        {
            try
            {
                List<Secondary> lst = db.Secondaries.ToList();

                if (checkBox1.IsChecked == true)
                {
                    lst = lst = lst.Where(x => x.Name.ToUpper().Contains(textBox.Text.ToUpper())).ToList();
                }
                if (checkBox2.IsChecked == true)
                {
                    lst = lst.Where((x => x.Characteristics.ToUpper().Contains(textBox2.Text.ToUpper()))).ToList();
                }
                lst = lst.Where(x => x.Type.ToUpper().Contains(textBox_type.Text.ToUpper())).ToList();
                dataGrid_Secondaries.ItemsSource = lst;
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            sortAccess();
        }

        private void dataGrid_Secondaries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Secondary s = dataGrid_Secondaries.SelectedItem as Secondary;
            Product q = db.Product.FirstOrDefault(x => x.Id == s.Id);
            label_balance.Content = "На складе : " + Convert.ToString(q.Balance);
            label_Purchase.Content = "Цена закупки : " + Convert.ToString(q.PricePurchase);
            label_Sell.Content = "Цена продажи : " + Convert.ToString(q.SellPrice);
        }

        private void dataGrid_MainProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainProduct s = dataGrid_MainProd.SelectedItem as MainProduct;
            Product q = db.Product.FirstOrDefault(x => x.Id == s.Id);
            label_balance.Content = "На складе : " + Convert.ToString(q.Balance);
            label_Purchase.Content = "Цена закупки : " + Convert.ToString(q.PricePurchase);
            label_Sell.Content = "Цена продажи : " + Convert.ToString(q.SellPrice);
        }

        private void textBox_amm_from_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox Tb1 = sender as TextBox;
            if (Char.IsDigit(e.Text, 0))
            {
                e.Handled = false;
            }
            else e.Handled = true;
        }

        private void textBox_speed_to_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb1 = sender as TextBox;
            if (tb1.Text.IndexOf(",") == tb1.Text.Length - 1)
            {
                tb1.Text += "0";
            }
            if (tb1.Text[0] == '0')
                tb1.Text = tb1.Text.TrimStart(new char[] { '0' });
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
                    if (Tb1.Text.Length - Tb1.Text.IndexOf(",") >= 4)
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

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_Secondaries.SelectedItem != null)
            {
                Secondary s = dataGrid_Secondaries.SelectedItem as Secondary;
                Id = s.Id;
                this.Close();
            }
            else if (dataGrid_MainProd.SelectedItem != null)
            {
                MainProduct s = dataGrid_MainProd.SelectedItem as MainProduct;
                Id = s.Id;
                this.Close();
            }
            else
            {
                MessageBox.Show("Вы ничего не выбрали");
            }
        }
    }
}

