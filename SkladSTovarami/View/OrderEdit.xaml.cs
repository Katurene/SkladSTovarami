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
using Microsoft.Office.Interop.Excel;
using SkladSTovarami.Entities;
using SkladSTovarami.ViewModel;

namespace SkladSTovarami.View
{
    /// <summary>
    /// Логика взаимодействия для OrderEdit.xaml
    /// </summary>
    public partial class OrderEdit : System.Windows.Window
    {

        public int IdDelivery = -1;
        public int EmployeeId;
        List<OrderViewModel> lst = new List<OrderViewModel>();
        OrderViewModel delinfo = null;
        public int i = 1;

        public OrderEdit()
        {
            InitializeComponent();
            MyContext db = new MyContext();
            var q = db.Product.Include("MainProduct");
            dataGrid.ItemsSource = GoodsViewMode(q.Include("Secondary").ToList());
            GoodsGrid.Visibility = Visibility.Hidden;
            CountPriceGrid.Visibility = Visibility.Hidden;
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

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridGoods.SelectedItem != null)
            {
                MainGrid.IsEnabled = false;
                MyContext db = new MyContext();
                CountPriceGrid.Visibility = Visibility.Visible;
                OrderViewModel edit = dataGridGoods.SelectedItem as OrderViewModel;
                textBox_Count.Text = Convert.ToString(edit.Count);
                textBLock_Price.Text = Convert.ToString(edit.Price);
            }
            else
            {
                MessageBox.Show("Вы ничего не выбрали!", "Ошибка");
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Hidden;
            GoodsGrid.Visibility = Visibility.Visible;
        }

        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridGoods.SelectedItem != null)
            {
                MyContext db = new MyContext();
                OrderViewModel del = dataGridGoods.SelectedItem as OrderViewModel;
                lst.Remove(lst.FirstOrDefault(x => x == del));
                dataGridGoods.ItemsSource = null;
                dataGridGoods.ItemsSource = lst;
            }
            else
            {
                MessageBox.Show("Вы ничего не выбрали!", "Ошибка");
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MyContext db = new MyContext();
                if (IdDelivery == -1)
                {
                    Order delnew = new Order();
                    delnew.Employees = db.Emloyees.FirstOrDefault(x => x.Id == EmployeeId);
                    delnew.Date = DateTime.Now;
                    if (checkBox.IsChecked == true)
                        delnew.Status = true;
                    else
                        delnew.Status = false;

                    foreach (OrderViewModel delvm in lst)
                    {
                        OrderInfo delinfo = new OrderInfo();
                        delinfo.Count = delvm.Count;
                        delinfo.Products = db.Product.Find(delvm.ProductsId);
                        delnew.OrderInfos.Add(delinfo);
                    }
                    db.Orders.Add(delnew);                   
                }
                else
                {
                    Order delnew = db.Orders.Include("OrderInfos").FirstOrDefault(x => x.Id == IdDelivery);
                    delnew.Employees = db.Emloyees.FirstOrDefault(x => x.Id == EmployeeId);
                    delnew.OrderInfos.Clear();
                    delnew.Date = DateTime.Now;
                    if (checkBox.IsChecked == true)
                        delnew.Status = true;                   
                    else
                        delnew.Status = false;
                    delnew.OrderInfos.Clear();
                    foreach (OrderViewModel delvm in lst)
                    {
                        OrderInfo delinfo = new OrderInfo();
                        delinfo.Count = delvm.Count;
                        delinfo.Products = db.Product.Find(delvm.ProductsId);
                        delnew.OrderInfos.Add(delinfo);
                    }                   
                }
                foreach (OrderInfo s in db.OrderInfos.ToList())
                {
                    if (s.ProductsId == null)
                        db.OrderInfos.Remove(s);
                }
                db.SaveChanges();
                Close();
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                delinfo = new OrderViewModel();
                MyContext db = new MyContext();
                var q = db.Product.Include("MainProduct");
                ProductsViewModel per = dataGrid.SelectedItem as ProductsViewModel;
                Product f = q.Include("Secondary").FirstOrDefault(x => x.Id == per.Id);
                delinfo.ProductsId = f.Id;
                delinfo.Id = i;
                i++;
                textBLock_Price.Text = Convert.ToString(f.PricePurchase);
                if (f.Secondary == null)
                {
                    delinfo.Name = f.MainProduct.CodeName;
                }
                else
                {
                    delinfo.Name = f.Secondary.Name;
                }
                CountPriceGrid.Visibility = Visibility.Visible;
                textBox_Count.Focus();
            }
            else
            {
                MessageBox.Show("Вы ничего не выбрали!", "Ошибка");
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Visible;
            textBox.Text = "Поиск";
            GoodsGrid.Visibility = Visibility.Hidden;
        }

        private void buttonCancelling_Click(object sender, RoutedEventArgs e)
        {
            textBLock_Price.Text = null;
            textBox_Count.Text = "1";
            MainGrid.IsEnabled = true;
            CountPriceGrid.Visibility = Visibility.Hidden;
            MainGrid.Visibility = Visibility.Visible;
            GoodsGrid.Visibility = Visibility.Hidden;
            dataGridGoods.ItemsSource = lst;
        }

        private void buttonSaving_Click(object sender, RoutedEventArgs e)
        {
            MyContext db = new MyContext();
            if (delinfo == null)
            {
                OrderViewModel edit = dataGridGoods.SelectedItem as OrderViewModel;
                OrderViewModel f = lst.FirstOrDefault(x => x == edit);
                f.Price = Convert.ToDouble(textBLock_Price.Text);
                f.Count = Convert.ToInt32(textBox_Count.Text);
                dataGridGoods.ItemsSource = null;
            }
            else
            {
                delinfo.Price = Convert.ToDouble(textBLock_Price.Text);
                delinfo.Count = Convert.ToInt32(textBox_Count.Text);
                lst.Add(delinfo);
                dataGridGoods.ItemsSource = null;
                delinfo = null;
            }
            db.SaveChanges();

            buttonCancelling_Click(null, null);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MyContext db = new MyContext();
            if (IdDelivery != -1)
            {
                Order deliver = db.Orders.Include("OrderInfos").FirstOrDefault(x => x.Id == IdDelivery);
                label4.Content = deliver.Employees.Surname + "  " + deliver.Employees.Name;
                foreach (OrderInfo s in deliver.OrderInfos)
                {
                    lst.Add(new OrderViewModel(s, s.Id));
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
        }

        private void textBox_Count_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Windows.Controls.TextBox Tb1 = sender as System.Windows.Controls.TextBox;
            if (Char.IsDigit(e.Text, 0))
            {
                e.Handled = false;
            }
            else e.Handled = true;
        }

        private void textBox_SellPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb1 = sender as System.Windows.Controls.TextBox;
            if (tb1.Text.IndexOf(",") == tb1.Text.Length - 1)
            {
                tb1.Text += "0";
            }
            if (tb1.Text[0] == '0')
                tb1.Text = tb1.Text.TrimStart(new char[] { '0' });
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e) //фильтр поиска
        {
            MyContext db = new MyContext();
            var s = db.Product.Include("MainProduct");
            List<ProductsViewModel> list = GoodsViewMode(s.Include("Secondary").ToList());
            dataGrid.ItemsSource = list.Where(x => x.Name.ToUpper().Contains(textBox.Text.ToUpper()));
        }

        private void ButtonExl_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
            sheet1.Columns.AutoFit();

            for (int j = 0; j < dataGridGoods.Columns.Count; j++)
            {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = dataGridGoods.Columns[j].Header;
            }
            for (int i = 0; i < dataGridGoods.Columns.Count; i++)
            {
                for (int j = 0; j < dataGridGoods.Items.Count; j++)
                {
                    TextBlock b = dataGridGoods.Columns[i].GetCellContent(dataGridGoods.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }

        //private void buttonFind_Click(object sender, RoutedEventArgs e)
        //{
        //    FindWindow f = new FindWindow();
        //    f.ShowDialog();
        //    List<ProductsViewModel> s = dataGridGoods.ItemsSource as List<ProductsViewModel>;
        //    int i = s.IndexOf(s.FirstOrDefault(x => x.Id == f.Id));
        //    dataGrid.SelectedIndex = i;
        //}
    }
}

