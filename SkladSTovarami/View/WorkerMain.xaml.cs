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
    /// Логика взаимодействия для WorkerMain.xaml
    /// </summary>
    public partial class WorkerMain : Window
    {
        WorkingTime WorkTime = new WorkingTime();
        public int EmployeeId { get; set; }
        int i = 1;
        double Sum = 0;
        ProductsViewModel view;
        MyContext db1 = new MyContext();
        Brush LabelCancelAdd;
        Brush LabelSibmitAdd;
        List<CheckViewModel> checks = new List<CheckViewModel>();

        public WorkerMain()
        {
            MyContext db = new MyContext();
            InitializeComponent();
            //Запоняем таблицу для нового чека
            dataGridCheck.ItemsSource = checks;
            //Запоняем таблицу для покупателей
            dataGridCustomer.ItemsSource = db.Customers.ToList();
            //Запоняем таблицу для Товаров
            var s = db.Product.Include("MainProduct");
            dataGridGoods.ItemsSource = GoodsViewMode(s.Include("Secondary").ToList());
            AddGoodsGrid.Visibility = Visibility.Hidden;
        }

        //Конвертирование Списка товаров для отображение в таблице доступных к продаже товаров 
        //аналогия главному окну для админа
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult res;
            res = MessageBox.Show("Завершить работу?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                WorkingTime wt = new WorkingTime()
                {
                    TimeStart = WorkTime.TimeStart,
                    TimeEnd = DateTime.Now,
                    Employees = db1.Emloyees.FirstOrDefault(x => x.Id == EmployeeId),
                    EmployeesId = EmployeeId
                };
                db1.WorkingTimes.Add(wt);
                db1.SaveChanges();
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox.Text = "";
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var s = db1.Product.Include("MainProduct");
            List<ProductsViewModel> list = GoodsViewMode(s.Include("Secondary").ToList());
            dataGridGoods.ItemsSource = list.Where(x => x.Name.ToUpper().Contains(textBox.Text.ToUpper()));
        }

        private void dataGridGoods_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (dataGridGoods.SelectedItem != null)
                {
                    AddGoodsGrid.Visibility = Visibility.Visible;
                    view = dataGridGoods.SelectedItem as ProductsViewModel;
                    label_Price_set.Content = view.SellPrice;
                    Main.IsEnabled = false;
                    textBox_Count.Focus();
                }
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Border b = sender as Border;
            LabelSibmitAdd = b.Background;
            b.Background = Brushes.Gray;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            Border b = sender as Border;
            b.Background = LabelSibmitAdd;
        }

        private void CancelAdd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddGoodsGrid.Visibility = Visibility.Hidden;
            textBox_Count.Text = null;
            dataGridGoods.SelectedItem = null;
            Main.IsEnabled = true;
        }

        private void CancelAdd_MouseEnter(object sender, MouseEventArgs e)
        {
            Border b = sender as Border;
            LabelCancelAdd = b.Background;
            b.Background = Brushes.Gray;
        }

        private void CancelAdd_MouseLeave(object sender, MouseEventArgs e)
        {
            Border b = sender as Border;
            b.Background = LabelCancelAdd;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textBox_Count.Text) <= view.Balance && textBox_Count.Text != "0")
                {
                    CheckViewModel ch = new CheckViewModel();
                    ch.Count = Convert.ToInt32(textBox_Count.Text);
                    ch.Price = view.SellPrice;
                    ch.Name = view.Name;
                    ch.Id = i;
                    ch.GoodId = view.Id;//все верно там также
                    i++;
                    Sum += ch.Sum;
                    checks.Add(ch);
                    AddGoodsGrid.Visibility = Visibility.Hidden;
                    dataGridCheck.ItemsSource = checks.ToList();
                    label4.Content = Convert.ToString(Sum);
                    textBox_Count.Text = null;
                    Main.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Не хватает на складе", "Ошибка");
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dataGridCustomer.SelectedItem == null)
                    MessageBox.Show("Выберите покупателя", "Ошибка");
                else if (checks.Count == 0)
                    MessageBox.Show("Пустой чек", "Ошибка");
                else
                {
                    Check ch = new Check();
                    ch.Date = DateTime.Now;
                    ch.Sum = Sum;
                    ch.Employees = db1.Emloyees.Find(EmployeeId);
                    Customer v = dataGridCustomer.SelectedItem as Customer;
                    ch.Customer = db1.Customers.FirstOrDefault(x => x.Id == v.Id);
                    CheckInfo NewChIn = new CheckInfo();

                    foreach (CheckViewModel nwch in checks)
                    {
                        db1.Product.FirstOrDefault(x => x.Id == nwch.GoodId).Balance -= nwch.Count;
                        CheckInfo f = new CheckInfo();
                        f.Count = nwch.Count;
                        f.Products = db1.Product.Find(nwch.GoodId);//f.Goods = db1.Goodss.Find(nwch.GoodId);
                        ch.CheckInfos.Add(f);
                    }
                    db1.Checks.Add(ch);                    
                    db1.SaveChanges();
                    i = 0;
                    Sum = 0;
                    checks.Clear();
                    dataGridCheck.ItemsSource = checks;
                    dataGridGoods.ItemsSource = null;
                    var s = db1.Product.Include("MainProduct");
                    dataGridGoods.ItemsSource = GoodsViewMode(s.Include("Secondary").ToList());
                    label4.Content = "";
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void dataGridCheck_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridCheck.SelectedItem != null)
            {
                CheckViewModel dataforlabel = dataGridCheck.SelectedItem as CheckViewModel;
                label_Selected.Content = dataforlabel.Name;
                label_SummaSelected.Content = Convert.ToString(dataforlabel.Price) + " X " + Convert.ToString(dataforlabel.Count) + " = " + Convert.ToString(dataforlabel.Sum);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            EditCustomer f = new EditCustomer();
            f.Closing += AddCustomer_Closing;
            f.ShowDialog();
        }

        void AddCustomer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MyContext db = new MyContext();
            dataGridCustomer.ItemsSource = db.Customers.ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WorkTime.TimeStart = DateTime.Now;
            WorkTime.Employees = db1.Emloyees.FirstOrDefault(x => x.Id == EmployeeId);
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

        private void Border_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            //delete select
            if (dataGridCheck.SelectedItem != null)
            {
                CheckViewModel s = dataGridCheck.SelectedItem as CheckViewModel;
                checks.Remove(s);
                dataGridCheck.ItemsSource = null;
                dataGridCheck.ItemsSource = checks;
            }
            else
                MessageBox.Show("Вы не выбрали ничего", "Ошибка");
        }

        private void Border_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            //Delete all
            checks.Clear();
            dataGridCheck.ItemsSource = null;
            dataGridCheck.ItemsSource = checks;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //GlobalFind f = new GlobalFind();
            //f.ShowDialog();
            //List<GoodsViewModel> s = dataGridGoods.ItemsSource as List<GoodsViewModel>;
            //int i = s.IndexOf(s.FirstOrDefault(x => x.Id == f.Id));
            //dataGridGoods.SelectedIndex = i;
        }
    }
}
