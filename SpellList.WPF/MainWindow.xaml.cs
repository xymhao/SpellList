using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
using SpellList.WPF.Annotations;

namespace SpellList.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<ProductDetail> DataList = new List<ProductDetail>();
        public MainWindow()
        {

            InitializeComponent();
            DataList = new List<ProductDetail>()
            {
                new ProductDetail("商品名称","商品价格"),
                new ProductDetail("大蒜","2"),
            };
            ProductList.ItemsSource = DataList;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(NameText.Text);
            DataList.Add(new ProductDetail(NameText, PriceText));
            ProductList.UpdateLayout();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class ProductDetail:INotifyPropertyChanged 
    {
        public ProductDetail(TextBox nameText, TextBox priceText)
        {
            Name = nameText.Text;
            Price = priceText.Text;
        }

        public ProductDetail(string name, string price)
        {
            Name = name;
            Price = price;
        }

        public string Price { get; set ; }

        public string Name { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
