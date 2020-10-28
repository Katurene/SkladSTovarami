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
using SkladSTovarami.ViewModel;

namespace SkladSTovarami.View
{
    /// <summary>
    /// Логика взаимодействия для DeliveryEdit.xaml
    /// </summary>
    public partial class DeliveryEdit : Window
    {
        public int IdDelivery = -1;
        public int EmployeeId;
        List<DeliveryViewModel> lst = new List<DeliveryViewModel>();
        DeliveryViewModel delinfo = null;
        public int i = 1;

        public DeliveryEdit()
        {
            InitializeComponent();
            MyContext db = new MyContext();

            var q = db.Product.Include("MainProduct");
            dataGrid.ItemsSource = GoodsViewMode(q.Include("Secondary").ToList());
            GoodsGrid.Visibility = Visibility.Hidden;
            CountPriceGrid.Visibility = Visibility.Hidden;
            List<string> combobox = new List<string>() { "Наличная", "Безналичная", "Неоплачена" };
            comboBox_TypePayment.ItemsSource = combobox;
        }

        public List<ProductsViewModel> GoodsViewMode(List<Product> good)
        {
            List<ProductsViewModel> lst = new List<ProductsViewModel>();
            foreach (Product p in good)
            {
                ProductsViewModel s = new ProductsViewModel() { SellPrice = p.SellPrice, PricePurchase = p.PricePurchase, Balance = p.Balance, Id = p.Id };
                if (p.Secondary == null)
                {
                    s.Name = p.MainProduct.CodeName;
                    s.Type = p.MainProduct.Type;
                }
                else
                {
                    s.Name = p.Secondary.Name;
                    s.Type = p.Secondary.Type;
                }
                lst.Add(s);
            }
            return lst;
        }

        private void buttonRedact_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridGoods.SelectedItem != null)
            {
                MainGrid.IsEnabled = false;
                MyContext db = new MyContext();
                CountPriceGrid.Visibility = Visibility.Visible;
                DeliveryViewModel edit = dataGridGoods.SelectedItem as DeliveryViewModel;
                textBox_Count.Text = Convert.ToString(edit.Count);
                textBox_PurchasePrice.Text = Convert.ToString(edit.Price);
                textBox_SellPrice.Text = Convert.ToString(db.Product.Find(edit.ProductsId).SellPrice);
            }
            else
            {
                MessageBox.Show("Вы ничего не выбрали!", "Ошибка");
            }
            SetSumma();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textBox_Count.Text != "" && textBox_Count.Text != "0" && textBox_PurchasePrice.Text != "" && textBox_PurchasePrice.Text != "0" && textBox_SellPrice.Text != "" && textBox_SellPrice.Text != "0")
                {
                    MyContext db = new MyContext();
                    if (delinfo == null)
                    {
                        DeliveryViewModel edit = dataGridGoods.SelectedItem as DeliveryViewModel;
                        DeliveryViewModel f = lst.FirstOrDefault(x => x == edit);
                        db.Product.FirstOrDefault(x => x.Id == f.ProductsId).Balance -= f.Count;
                        f.Price = Convert.ToDouble(textBox_PurchasePrice.Text);
                        f.Count = Convert.ToInt32(textBox_Count.Text);
                        db.Product.Find(edit.ProductsId).SellPrice = Convert.ToDouble(textBox_SellPrice.Text);
                        db.Product.Find(f.ProductsId).PricePurchase = Convert.ToDouble(textBox_PurchasePrice.Text);
                        db.Product.FirstOrDefault(x => x.Id == f.ProductsId).Balance += f.Count;
                        dataGridGoods.ItemsSource = null;
                    }
                    else
                    {
                        delinfo.Price = Convert.ToDouble(textBox_PurchasePrice.Text);
                        delinfo.Count = Convert.ToInt32(textBox_Count.Text);
                        db.Product.Find(delinfo.ProductsId).SellPrice = Convert.ToDouble(textBox_SellPrice.Text);
                        db.Product.Find(delinfo.ProductsId).PricePurchase = Convert.ToDouble(textBox_PurchasePrice.Text);
                        lst.Add(delinfo);
                        db.Product.FirstOrDefault(x => x.Id == delinfo.ProductsId).Balance += delinfo.Count;
                        dataGridGoods.ItemsSource = null;
                        delinfo = null;
                    }
                    db.SaveChanges();

                    buttonCancel_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Неправильный ввод");
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            textBox_SellPrice.Text = null;
            textBox_PurchasePrice.Text = null;
            textBox_Count.Text = "1";
            MainGrid.IsEnabled = true;
            CountPriceGrid.Visibility = Visibility.Hidden;
            MainGrid.Visibility = Visibility.Visible;
            GoodsGrid.Visibility = Visibility.Hidden;
            dataGridGoods.ItemsSource = lst;
            SetSumma();
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                delinfo = new DeliveryViewModel();
                MyContext db = new MyContext();
                var q = db.Product.Include("MainProduct");
                ProductsViewModel per = dataGrid.SelectedItem as ProductsViewModel;
                Product f = q.Include("Secondary").FirstOrDefault(x => x.Id == per.Id);
                delinfo.ProductsId = f.Id;
                delinfo.Id = i;
                if (f.Secondary == null)
                {
                    delinfo.Name = f.MainProduct.CodeName;
                }
                else
                {
                    delinfo.Name = f.Secondary.Name;
                }
                CountPriceGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Вы ничего не выбрали!", "Ошибка");
            }
        }

        private void buttonCancelling_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Visible;
            textBox.Text = "Поиск";
            GoodsGrid.Visibility = Visibility.Hidden;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Hidden;
            GoodsGrid.Visibility = Visibility.Visible;
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridGoods.SelectedItem != null)
            {
                MyContext db = new MyContext();
                DeliveryViewModel del = dataGridGoods.SelectedItem as DeliveryViewModel;
                db.Product.FirstOrDefault(x => x.Id == del.ProductsId).Balance -= del.Count;
                lst.Remove(lst.FirstOrDefault(x => x == del));
                dataGridGoods.ItemsSource = null;
                dataGridGoods.ItemsSource = lst;
            }
            else
            {
                MessageBox.Show("Вы ничего не выбрали!", "Ошибка");
            }
            SetSumma();
        }

        private void buttonSaving_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MyContext db = new MyContext();
                if (IdDelivery == -1)
                {
                    DeliveryNote delnew = new DeliveryNote();
                    delnew.Employees = db.Emloyees.FirstOrDefault(x => x.Id == EmployeeId);
                    delnew.TypePayment = comboBox_TypePayment.SelectedItem as string;
                    delnew.Sum = 0;
                    delnew.Date = DateTime.Now;

                    foreach (DeliveryViewModel delvm in lst)
                    {
                        DeliveryInfo delinfo = new DeliveryInfo();
                        delinfo.Count = delvm.Count;
                        delinfo.Price = delvm.Price;
                        delinfo.Products = db.Product.Find(delvm.ProductsId);
                        delnew.Sum += delvm.Summa;
                        delnew.DeliveryInfos.Add(delinfo);
                    }                    
                    db.DeliveryNotes.Add(delnew);
                }
                else
                {
                    DeliveryNote delnew = db.DeliveryNotes.FirstOrDefault(x => x.Id == IdDelivery);
                    delnew.Employees = db.Emloyees.FirstOrDefault(x => x.Id == EmployeeId);
                    delnew.TypePayment = comboBox_TypePayment.SelectedItem as string;
                    delnew.Sum = 0;
                    delnew.Date = DateTime.Now;
                    delnew.DeliveryInfos.Clear();

                    foreach (DeliveryViewModel delvm in lst)
                    {
                        DeliveryInfo delinfo = new DeliveryInfo();
                        delinfo.Count = delvm.Count;
                        delinfo.Price = delvm.Price;
                        delinfo.Products = db.Product.Find(delvm.ProductsId);
                        delnew.Sum += delvm.Summa;
                        delnew.DeliveryInfos.Add(delinfo);
                    }                   
                }
                db.SaveChanges();
                Close();
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        public void SetSumma()
        {
            double sum = 0;
            foreach (DeliveryViewModel s in lst)
            {
                sum += s.Summa;
            }
            textBlock.Text = Convert.ToString(sum);
        }

        private void Lol_Loaded(object sender, RoutedEventArgs e)
        {
            MyContext db = new MyContext();
            if (IdDelivery != -1)
            {
                DeliveryNote deliver = db.DeliveryNotes.FirstOrDefault(x => x.Id == IdDelivery);
                label4.Content = deliver.Employees.Surname + "  " + deliver.Employees.Name;
                foreach (DeliveryInfo s in deliver.DeliveryInfos)
                {
                    lst.Add(new DeliveryViewModel(s, s.Id));
                }
            }
            else
            {
                Employee emp = db.Emloyees.FirstOrDefault(x => x.Id == EmployeeId);
                label4.Content = emp.Surname + "  " + emp.Name;
            }
            if (lst.Count != 0)
            {
                i = lst[lst.Count - 1].Id + 1;
            }
            dataGridGoods.ItemsSource = lst;
            SetSumma();
        }

        private void textBox_Count_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox Tb1 = sender as TextBox;
            if (Char.IsDigit(e.Text, 0))
            {
                e.Handled = false;
            }
            else e.Handled = true;
        }

        private void textBox_PurchasePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
        }

        private void textBox_SellPrice_LostFocus(object sender, RoutedEventArgs e)
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
            if (textBox.Text == "Поиск")
                textBox.Text = null;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MyContext db = new MyContext();
            var s = db.Product.Include("MainProduct");
            List<ProductsViewModel> list = GoodsViewMode(s.Include("Secondary").ToList());
            dataGrid.ItemsSource = list.Where(x => x.Name.ToUpper().Contains(textBox.Text.ToUpper()));
        }

        private void buttonFind_Click(object sender, RoutedEventArgs e)
        {
            //GlobalFind f = new GlobalFind();
            //f.ShowDialog();
            //List<GoodsViewModel> s = dataGridGoods.ItemsSource as List<GoodsViewModel>;
            //int i = s.IndexOf(s.FirstOrDefault(x => x.Id == f.Id));
            //dataGrid.SelectedIndex = i;
        }
    }
}
