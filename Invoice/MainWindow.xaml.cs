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

using System.IO;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using GemBox.Spreadsheet;


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

        public string netoPrice { get; set; }

        public string poreskaOsnovica { get; set; }
        public string pdvLevel { get; set; }

        public string pdv { get; set; }
       

        public string brtoPrice { get; set; }

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

        //initialiye fields in the Gui part(grid view, combo boxs...)
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
        //Fuction where we check are all nessesary fields filled if it is all correct display data in grid
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
        //function where we need to add new article in list and also add new row in data grid
        public void displayData()
        {
            articles.Add(new Article() {
                idArt=txtIDArticle.Text,
                nameArt=txtNameArt.Text,
                measureUnit=cmbMeasureUnit.SelectedValue.ToString(),
                quatity=txtQuantity.Text,
                netoPrice=txtPriceArt.Text,
                poreskaOsnovica=txtPoreskaOsnovica.Text,
                pdvLevel=cmbPDV.SelectedValue.ToString(),
                pdv=txtValueOfPDV.Text,
                brtoPrice=txtSumOfCell.Text,

            });


            DataRow dr = dt.NewRow();

            dr[0] = txtIDArticle.Text;
            dr[1] = txtNameArt.Text;
            dr[2] = cmbMeasureUnit.SelectedValue.ToString();
            dr[3] = txtQuantity.Text;
            dr[4] = txtPriceArt.Text;
            dr[5] = txtPoreskaOsnovica.Text;//poreska osnovica
            dr[6] = cmbPDV.SelectedValue.ToString();
            dr[7] = txtValueOfPDV.Text;
            dr[8] = txtSumOfCell.Text;

            dt.Rows.Add(dr);

        }

        //Funcition where we create Invoice and fill header cells in template.xlsx
        //and copy data from dataGrid to place which is planned to be fill with articles
        private void btnCreateOffer_Click(object sender, RoutedEventArgs e)
        {


            if (txtDateCreate.SelectedDate != null && txtDateDelivery.SelectedDate != null
                && txtUniqueNameOfInvoice.Text != "" && txtCustomerName.Text != "" && txtPIBCustomer.Text != "")
            {


                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                var workbook = ExcelFile.Load("template.xlsx");

                var worksheet = workbook.Worksheets[0];


                //data of customer

                worksheet.Cells["G11"].Value = txtCustomerName.Text;
                worksheet.Cells["G12"].Value = txtAdressCustomer.Text;
                worksheet.Cells["G13"].Value = txtCityPlaceCustomer.Text;
                worksheet.Cells["G14"].Value = txtPIBCustomer.Text;

                //data for header of invoice
                worksheet.Cells["D8"].Value = txtDateCreate.SelectedDate.Value.Date.ToString("dd-MM-yyyy");
                worksheet.Cells["D9"].Value = txtDateCreate.SelectedDate.Value.Date.ToString("dd-MM-yyyy");
                worksheet.Cells["D10"].Value = txtDateDelivery.SelectedDate.Value.Date.ToString("dd-MM-yyyy");
                worksheet.Cells["F17"].Value = txtUniqueNameOfInvoice.Text;





                workbook.Save($"../../../{txtCustomerName.Text}{DateTime.Now.ToString("HHmmssMMddyyyy")}.xlsx");

                MessageBox.Show("Uspesno kreiran Račun","Obaveštenje", MessageBoxButton.OK,MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("Niste uneli sva polja", "Greška",MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }
    }
}
