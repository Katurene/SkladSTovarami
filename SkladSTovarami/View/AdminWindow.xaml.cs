﻿using System;
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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {  
        public int EmployeeId;

            public AdminWindow()
            {
                MyContext db = new MyContext();
                InitializeComponent();

                OrderGrid.Visibility = Visibility.Hidden;
                GoodsGrid.Visibility = Visibility.Visible;
                DeliveryGrid.Visibility = Visibility.Hidden;
                WorkerGrid.Visibility = Visibility.Hidden;              
                Useful.Visibility = Visibility.Hidden;
                dataGrid_goods.Visibility = Visibility.Hidden;
                dataGrid_useful.Visibility = Visibility.Hidden;
                var s = db.Product.Include("MainProduct");
                dataGridGoods.ItemsSource = GoodsViewMode(s.Include("Secondary").ToList());
                dataGridWorker.ItemsSource = db.Emloyees.ToList();
                dataGrid_Delivery.ItemsSource = db.DeliveryNotes.ToList();
                dataGridOrder.ItemsSource = db.Orders.ToList();
            }

            private void label_Worker_MouseDown(object sender, MouseButtonEventArgs e)
            {
                OrderGrid.Visibility = Visibility.Hidden;
                GoodsGrid.Visibility = Visibility.Hidden;
                DeliveryGrid.Visibility = Visibility.Hidden;
                WorkerGrid.Visibility = Visibility.Visible;                
                Useful.Visibility = Visibility.Hidden;
            }            

            private void label_Goods_MouseDown(object sender, MouseButtonEventArgs e)
            {
                OrderGrid.Visibility = Visibility.Hidden;
                GoodsGrid.Visibility = Visibility.Visible;
                DeliveryGrid.Visibility = Visibility.Hidden;
                WorkerGrid.Visibility = Visibility.Hidden;              
                Useful.Visibility = Visibility.Hidden;
            }

        private void label_Order_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OrderGrid.Visibility = Visibility.Visible;
            GoodsGrid.Visibility = Visibility.Hidden;
            DeliveryGrid.Visibility = Visibility.Hidden;
            WorkerGrid.Visibility = Visibility.Hidden;           
            Useful.Visibility = Visibility.Hidden;
        }

        private void label_Delivery_MouseDown(object sender, MouseButtonEventArgs e)
            {
                OrderGrid.Visibility = Visibility.Hidden;
                GoodsGrid.Visibility = Visibility.Hidden;
                DeliveryGrid.Visibility = Visibility.Visible;
                WorkerGrid.Visibility = Visibility.Hidden;              
                Useful.Visibility = Visibility.Hidden;
            }

            private void label_useful_MouseDown(object sender, MouseButtonEventArgs e)
            {
                OrderGrid.Visibility = Visibility.Hidden;
                GoodsGrid.Visibility = Visibility.Hidden;
                DeliveryGrid.Visibility = Visibility.Hidden;
                WorkerGrid.Visibility = Visibility.Hidden;             
                Useful.Visibility = Visibility.Visible;
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

            private void buttonAddNewProduct_Click(object sender, RoutedEventArgs e)
            {
            Products f = new Products();
            f.Closing += addNoteWindow_Closing;
            f.ShowDialog();
        }

            private void buttonChangeProduct_Click(object sender, RoutedEventArgs e)
            {
                MyContext db = new MyContext();
                if (dataGridGoods.SelectedItem != null)
                {
                    ProductsViewModel s = dataGridGoods.SelectedItem as ProductsViewModel;
                    var bd = db.Product.Include("MainProduct");

                    Product fnd = bd.Include("Secondary").FirstOrDefault(x => x.Id == s.Id);
                    Products f = new Products();
                    f.textBox.Text = Convert.ToString(fnd.Balance);
                    f.textBox1.Text = Convert.ToString(fnd.PricePurchase);
                    f.textBox2.Text = Convert.ToString(fnd.SellPrice);
                    f.textBox_Id.Text = Convert.ToString(fnd.Id);
                    if (fnd.Secondary != null)
                    {
                        Secondary Secondarys = db.Secondaries.Find(s.Id);
                        f.textBox_Type.Text = Secondarys.Type;
                        f.textBox_Name.Text = Secondarys.Name;
                        f.textBox_Charact.Text = Secondarys.Characteristics;
                        f.TypeGoods.SelectedItem = "Дополнительный";
                    }
                    else
                    {
                        MainProduct mainProduct = db.MainProducts.Find(s.Id);
                        f.textBox_TypeWeap.Text = mainProduct.Type;
                        f.textBox_NameWeap.Text = mainProduct.CodeName;
                        f.textBox_Brand.Text = mainProduct.Brand;
                        f.textBox_TireWidth.Text = Convert.ToString(mainProduct.TireWidth);
                        f.textBox_SpeedNum.Text = Convert.ToString(mainProduct.SpeedNum);
                        f.textBox_Diameter.Text = Convert.ToString(mainProduct.Diameter);                        
                        f.textBox_Info.Text = mainProduct.Info;
                        f.checkBox.IsChecked = mainProduct.Used;
                        f.TypeGoods.SelectedItem = "Основной";  
                    }
                    f.Closing += addNoteWindow_Closing;
                    f.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Вы не выбрали ничего", "Ошибка");
                }
            }

            private void button_DeleteGoods_Click(object sender, RoutedEventArgs e)
            {
            if (dataGridGoods.SelectedItem != null)
            {
                ProductsViewModel viewmodel = dataGridGoods.SelectedItem as ProductsViewModel;
                MyContext db = new MyContext();
                var f = db.Product.Include("MainProduct");
                var g = f.Include("Secondary");
                Product q = g.FirstOrDefault(x => x.Id == viewmodel.Id);
                db.Product.Remove(q);
                db.SaveChanges();
                var s = db.Product.Include("MainProduct");
                dataGridGoods.ItemsSource = GoodsViewMode(s.Include("Secondary").ToList());
            }
            else
            {
                MessageBox.Show("Надо что-нибудь выбрать!", "Ошибка");
            }
        }

            private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                MessageBoxResult res;
                res = MessageBox.Show("Закрыть программу?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }

            private void buttonAddWorker_Click(object sender, RoutedEventArgs e)
            {
            AddWorker wind = new AddWorker();
            wind.Closing += AddWorker_Closing;
            wind.ShowDialog();
        }

            private void button_DeleteWorker_Click(object sender, RoutedEventArgs e)
            {
            if (dataGridWorker.SelectedItem != null)
            {
                MyContext db = new MyContext();
                Employee viewmodel = dataGridWorker.SelectedItem as Employee;
                Employee q = db.Emloyees.Find(viewmodel.Id);

                db.Emloyees.Remove(q);
                db.SaveChanges();
                MyContext db1 = new MyContext();
                dataGridWorker.ItemsSource = db1.Emloyees.ToList();
            }
            else
            {
                MessageBox.Show("Выберите что-нибудь?", "Ошибка");
            }
        }

        private void button_EditWorker_Click(object sender, RoutedEventArgs e)
            {
            if (dataGridWorker.SelectedItem != null)
            {
                MyContext db = new MyContext();
                Employee s = dataGridWorker.SelectedItem as Employee;
                AddWorker wind = new AddWorker();
                wind.textBox_login.Text = s.Login;
                wind.textBox_name.Text = s.Name;
                wind.textBox_surname.Text = s.Surname;
                wind.comboBox_role.SelectedItem = s.Role;
                wind.textBlock_Id.Text = Convert.ToString(s.Id);
                wind.Closing += AddWorker_Closing;
                wind.ShowDialog();
            }
        }

            void addNoteWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
            MyContext db = new MyContext();
            var s = db.Product.Include("MainProduct");
            dataGridGoods.ItemsSource = GoodsViewMode(s.Include("Secondary").ToList());
        }

        void AddWorker_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MyContext db = new MyContext();
            dataGridWorker.ItemsSource = db.Emloyees.ToList();
        }

            private void buttonAddDelivery_Click(object sender, RoutedEventArgs e)
        {
            DeliveryEdit f = new DeliveryEdit() { IdDelivery = -1 };
            f.EmployeeId = EmployeeId;
            f.Closing += DeliveryEdit_closing;
            f.ShowDialog();
        }

        void DeliveryEdit_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MyContext db = new MyContext();
            dataGrid_Delivery.ItemsSource = db.DeliveryNotes.ToList();
            dataGridGoods.ItemsSource = GoodsViewMode(db.Product.ToList());
        }

        private void button_InfoDelivery_Click(object sender, RoutedEventArgs e)
            {
            if (dataGrid_Delivery.SelectedItem != null)
            {
                DeliveryNote delnote = dataGrid_Delivery.SelectedItem as DeliveryNote;
                DeliveryEdit wind = new DeliveryEdit() { IdDelivery = delnote.Id };
                wind.EmployeeId = EmployeeId;
                wind.Closing += DeliveryEdit_closing;
                wind.ShowDialog();
            }
            else
            {
                MessageBox.Show("Необходимо выбрать!", "Ошибка");
            }
        }

            private void button_DeleteDelivery_Click(object sender, RoutedEventArgs e)
            {
            if (dataGrid_Delivery.SelectedItem != null)
            {
                MyContext db = new MyContext();
                DeliveryNote delnote = dataGrid_Delivery.SelectedItem as DeliveryNote;
                DeliveryNote del = db.DeliveryNotes.FirstOrDefault(x => x.Id == delnote.Id);

                foreach (DeliveryInfo s in del.DeliveryInfos)
                {
                    db.Product.FirstOrDefault(x => x.Id == s.ProductsId).Balance -= s.Count;
                }
                db.DeliveryNotes.Remove(del);
                db.SaveChanges();
                MyContext db1 = new MyContext();
                dataGrid_Delivery.ItemsSource = db1.DeliveryNotes.ToList();
                dataGridGoods.ItemsSource = GoodsViewMode(db1.Product.ToList());
            }
            else
            {
                MessageBox.Show("Необходимо сделать выбор!", "Ошибка");
            }
        }

        private void button_OrderNew_Click(object sender, RoutedEventArgs e)
        {
            OrderEdit f = new OrderEdit() { IdDelivery = -1 };
            f.EmployeeId = EmployeeId;
            f.Closing += OrderEdit_closing;
            f.ShowDialog();
        }

            void OrderEdit_closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                MyContext db = new MyContext();
                dataGridOrder.ItemsSource = null;
                dataGridOrder.ItemsSource = db.Orders.ToList();
            }

            private void button_OrderDelete_Click(object sender, RoutedEventArgs e)
            {
            if (dataGridOrder.SelectedItem != null)
            {
                MyContext db = new MyContext();
                Order delnote = dataGridOrder.SelectedItem as Order;
                Order del = db.Orders.FirstOrDefault(x => x.Id == delnote.Id);
                db.Orders.Remove(del);
                db.SaveChanges();
                MyContext db1 = new MyContext();
                dataGridOrder.ItemsSource = db1.Orders.ToList();
            }
            else
            {
                MessageBox.Show("Выберите что-нибудь?", "Ошибка");
            }
        }

            private void button_OrderEdit_Click(object sender, RoutedEventArgs e)
            {
            if (dataGridOrder.SelectedItem != null)
            {
                Order delnote = dataGridOrder.SelectedItem as Order;
                OrderEdit wind = new OrderEdit() { IdDelivery = delnote.Id };
                wind.EmployeeId = EmployeeId;
                wind.Closing += OrderEdit_closing;
                wind.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите что-нибудь?", "Ошибка");
            }
        }    

        private void button1_Click_1(object sender, RoutedEventArgs e)
            {
                //GlobalFind f = new GlobalFind();
                //f.ShowDialog();
                //List<GoodsViewModel> s = dataGridGoods.ItemsSource as List<GoodsViewModel>;
                //int i = s.IndexOf(s.FirstOrDefault(x => x.Id == f.Id));
                //dataGridGoods.SelectedIndex = i;
            }
        
        private void buttonOtgrZaMesac_Click(object sender, RoutedEventArgs e)
        {
            MyContext db = new MyContext();
            List<CheckViewModel> lst = new List<CheckViewModel>();
            List<CheckInfo> WorkTime = db.CheckInfos.ToList().Where(x => x.Check.Date > DateTime.Now.AddDays(-30)).Where(x => x.Check.Date < DateTime.Now).ToList();
            foreach (CheckInfo s in WorkTime)
            {
                CheckViewModel t = new CheckViewModel();
                    t.Id = s.Id;
                    string name;
                    if (s.Products.Secondary == null)
                        name = s.Products.MainProduct.CodeName;
                    else
                        name = s.Products.Secondary.Name;
                    t.Name = name;
                    t.Price = s.Products.SellPrice;
                    t.Count = s.Count;
                    t.GoodId = s.Products.Id;
                    lst.Add(t);
                }
                dataGrid_useful.ItemsSource = lst;
                dataGrid_goods.Visibility = Visibility.Hidden;
                dataGrid_useful.Visibility = Visibility.Visible;
            }
        
        private void buttonOstatki_Click(object sender, RoutedEventArgs e)
        {
            MyContext db = new MyContext();
            var s = db.Product.Include("MainProduct");//если меньше 3 то мало
            dataGrid_goods.ItemsSource = GoodsViewMode(s.Include("Secondary").Where(x => x.Balance <= 3).ToList());
            dataGrid_goods.Visibility = Visibility.Visible;
            dataGrid_useful.Visibility = Visibility.Hidden;
        }           
    }    
}