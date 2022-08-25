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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
namespace Invoice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Article
    {
        public int idArt { get; set; }
        public string nameArt { get; set; }

        public string measureUnit { get; set; }
        public double quatity { get; set; }

        public double price { get; set; }

        public double priceWithoutPDV { get; set; }
        public int pdvLevel { get; set; }

        public double pdv { get; set; }
       

        public double sumPrice { get; set; }

    }
    public class Customer
    {
        public string pibCustomer { get; set; }
        public string nameCustomer { get; set; }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        DataTable dt=new DataTable();
        public List<Article> articles=new List<Article>();
        public Customer customer = new Customer();
        
        public string dateCreate { get; set; }

        public string dateDelivery { get; set; }

        public string nameOfInvoice { get; set; }


        private void btnAddCell_Click(object sender, RoutedEventArgs e)
        {
            articles.Add(new Article()
            {
                idArt = 1,
                nameArt="naziv",
                quatity=5,
                price=100,
                pdv=20
               
            });
           


        }

        public void displayData()
        {

        }

        private void winmain_Initialized(object sender, EventArgs e)
        {

            dt.Columns.Add("Rbr");
            dt.Columns.Add("Šifra");
            dt.Columns.Add("Naziv");
            dt.Columns.Add("J.M");
            dt.Columns.Add("Količina");
            dt.Columns.Add("Cena po jedinici");
            dt.Columns.Add("Poreska osnovica");
            dt.Columns.Add("Stopa PDV");
            dt.Columns.Add("Iznos PDV");
            dt.Columns.Add("Ukupna naknada");







            dataGridInvoice.ItemsSource = articles;
        }
    }
}
