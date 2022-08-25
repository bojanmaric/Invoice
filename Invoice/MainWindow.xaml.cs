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
        public string idArt { get; set; }
        public string nameArt { get; set; }

        public string measureUnit { get; set; }
        public string quatity { get; set; }

        public string price { get; set; }

        public string priceWithoutPDV { get; set; }
        public string pdvLevel { get; set; }

        public string pdv { get; set; }
       

        public string sumPrice { get; set; }

    }
    public class Customer
    {
        public string pibCustomer { get; set; }
        public string nameCustomer { get; set; }
    }

    public partial class MainWindow : Window
    {
        DataTable dt;
        public List<Article> articles= new List<Article>();
          
        public Customer customer = new Customer();

        public string dateCreate { get; set; }

        public string dateDelivery { get; set; }

        public string nameOfInvoice { get; set; }
        

        public MainWindow()
        {
            InitializeComponent();

        }

        private void winmain_Initialized(object sender, EventArgs e)
        {

            dt = new DataTable();

            dt.Columns.Add("Šifra");
            dt.Columns.Add("Naziv");
            dt.Columns.Add("J-M");
            dt.Columns.Add("Količina");
            dt.Columns.Add("Cena po jedinici");
            dt.Columns.Add("Poreska osnovica");
            dt.Columns.Add("Stopa PDV");
            dt.Columns.Add("Iznos PDV");
            dt.Columns.Add("Ukupna naknada");

            dataGridInvoice.ItemsSource = dt.DefaultView;

            cmbMeasureUnit.Items.Add("m");
            cmbMeasureUnit.Items.Add("kom");
            cmbMeasureUnit.Text = "kom";

            cmbPDV.Items.Add("20");
            cmbPDV.Items.Add("10");
            cmbPDV.Text = "20";

        }

        private void btnAddCell_Click(object sender, RoutedEventArgs e)
        {
            if (txtNameArt.Text != "" && txtQuantity.Text != ""
                && txtPriceArt.Text != "" && cmbPDV.SelectedValue.ToString() != "")
            {
                displayData();
            }
            else
            {
                MessageBox.Show("Morate popuniti sva polja!","Greška", MessageBoxButton.OK,MessageBoxImage.Error);
            }


        }

        public void displayData()
        {
            DataRow dr = dt.NewRow();

            dr[0] = txtIDArticle.Text;
            dr[1] = txtNameArt.Text;
            dr[2] = cmbMeasureUnit.SelectedValue.ToString();
            dr[3] = txtQuantity.Text;
            dr[4] = txtPriceArt.Text;
            dr[5] = txtValueOfPDV.Text;//poreska osnovica
            dr[6] = cmbPDV.SelectedValue.ToString();
            dr[7] = txtValueOfPDV.Text;
            dr[8] = txtSumOfCell.Text;

            dt.Rows.Add(dr);

        }

       
    }
}
