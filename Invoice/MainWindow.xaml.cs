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
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using GemBox.Spreadsheet;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using Color = System.Windows.Media.Color;

namespace Invoice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class ArticalOnStock
    {
        public string barkod { get; set; }
        public double porez { get; set; }

        public string jedinica_mere { get; set; }
        public double cena { get; set; }

        public string naziv { get; set; }

        public string sifra { get; set; }

        public int vrsta_artikla { get; set; }


    }

    public class Article
    {
        public string idArt { get; set; }
        public string nameArt { get; set; }

        public string measureUnit { get; set; }
        public double quatity { get; set; }

        public double price { get; set; }

        public double poreskaOsnovica { get; set; }
        public string pdvLevel { get; set; }

        public double pdvValue { get; set; }


        public double sumPrice { get; set; }

    }
    public class Customer
    {
        public string pibCustomer { get; set; }
        public string nameCustomer { get; set; }
    }

    public partial class MainWindow : Window
    {
        DataTable dt;
        public List<Article> articles;

        public Customer customer = new Customer();

        public string dateCreate { get; set; }

        public string dateDelivery { get; set; }

        public string nameOfInvoice { get; set; }

        double sumAllPDV = 0;
        double sumAllPoreskuOsnovicu = 0;
        double summAllAmountOfArticles = 0;

        private bool selectedCell = false;
        private int idSelectedRow;

        public List<ArticalOnStock> onStocks;

        public MainWindow()
        {
            InitializeComponent();
            onStocks = new List<ArticalOnStock>();
            articles = new List<Article>();
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

            txtQuantity.Text = "1";
            txtDiscount.Text = "0";

            txtUniqueNameOfInvoice.Text = DateTime.Now.ToString("ddMMyyyyHHmm");

            txtIDArticle.Focus();

        }
        //Fuction where we check are all nessesary fields filled if it is all correct display data in grid
        private void btnAddCell_Click(object sender, RoutedEventArgs e)
        {
            if (txtNameArt.Text != "" && txtQuantity.Text != ""
                && txtPriceArt.Text != "")
            {
                //moze samo tacka kao broj.neka decimala 

                if (double.Parse(txtQuantity.Text) > 0 && double.Parse(txtPriceArt.Text) > 0)
                {
                    displayData();
                }
                else
                {
                    MessageBox.Show("Proverite polja cene i kolicine mora biti zarez izmedju brojeva ne tačka!",
                        "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            //}else if(txtIDArticle.Text!=""&& txtNameArt.Text==""){
            //    displayDataFromStock();
            //}
            else
            {
                MessageBox.Show("Morate popuniti sva polja!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /*  public void displayDataFromStock()
          {
              foreach (ArticalOnStock artical in onStocks)
              {
                  if (artical.barkod == txtIDArticle.Text)
                  {
                      double price = artical.cena;

                      double poreskaOsnovica = Math.Round((price / ((100 + double.Parse(cmbPDV.SelectedValue.ToString())) / 100)) * double.Parse(txtQuantity.Text), 2);

                      double sumCell = Math.Round(price * double.Parse(txtQuantity.Text), 2);
                      double pdvValue = Math.Round(sumCell - poreskaOsnovica, 2);
                      //Math.round(value,2) to round valute to two decimal place

                      articles.Add(new Article()
                      {
                          idArt = artical.barkod,
                          nameArt = artical.naziv,
                          measureUnit = artical.jedinica_mere,
                          quatity = double.Parse(txtQuantity.Text),
                          price = price,
                          poreskaOsnovica = poreskaOsnovica,
                          pdvLevel = artical.porez.ToString(),
                          pdvValue = pdvValue,
                          sumPrice = sumCell,

                      });

                      sumAllPDV = Math.Round(sumAllPDV + pdvValue, 2);
                      txtPdvSum.Text = sumAllPDV.ToString();

                      summAllAmountOfArticles = Math.Round(summAllAmountOfArticles + sumCell, 2);
                      txtAmountOfAllAcrticles.Text = summAllAmountOfArticles.ToString();

                      sumAllPoreskuOsnovicu = Math.Round(sumAllPoreskuOsnovicu + poreskaOsnovica, 2);
                      txtNetoSum.Text = sumAllPoreskuOsnovicu.ToString();

                      DataRow dr = dt.NewRow();

                      dr[0] = artical.barkod;
                      dr[1] = artical.naziv;
                      dr[2] = artical.jedinica_mere;
                      dr[3] = txtQuantity.Text;
                      dr[4] = artical.naziv;
                      dr[5] = poreskaOsnovica.ToString();//poreska osnovica
                      dr[6] = artical.porez;
                      dr[7] = pdvValue.ToString();
                      dr[8] = sumCell.ToString();

                      dt.Rows.Add(dr);

                      emptyRow();
                  }

              }
          }*/
        //function where we need to add new article in list and also add new row in data grid
        public void displayData()
        {
            // double rabat = double.Parse(txtDiscount.Text);
            double price = double.Parse(txtPriceArt.Text);

            double poreskaOsnovica = Math.Round((price / ((100 + double.Parse(cmbPDV.SelectedValue.ToString())) / 100)) * double.Parse(txtQuantity.Text), 2);

            double sumCell = Math.Round(price * double.Parse(txtQuantity.Text), 2);
            double pdvValue = Math.Round(sumCell - poreskaOsnovica, 2);
            //Math.round(value,2) to round valute to two decimal place

            articles.Add(new Article()
            {
                idArt = txtIDArticle.Text,
                nameArt = txtNameArt.Text,
                measureUnit = cmbMeasureUnit.SelectedValue.ToString() != "" ? cmbMeasureUnit.SelectedValue.ToString() : "kom",
                quatity = double.Parse(txtQuantity.Text),
                price = price,
                poreskaOsnovica = poreskaOsnovica,
                pdvLevel = cmbPDV.SelectedValue.ToString() != "" ? cmbPDV.SelectedValue.ToString() : "20",
                pdvValue = pdvValue,
                sumPrice = sumCell,

            });

            sumAllPDV = Math.Round(sumAllPDV + pdvValue, 2);
            txtPdvSum.Text = sumAllPDV.ToString();

            summAllAmountOfArticles = Math.Round(summAllAmountOfArticles + sumCell, 2);
            txtAmountOfAllAcrticles.Text = summAllAmountOfArticles.ToString();

            sumAllPoreskuOsnovicu = Math.Round(sumAllPoreskuOsnovicu + poreskaOsnovica, 2);
            txtNetoSum.Text = sumAllPoreskuOsnovicu.ToString();

            DataRow dr = dt.NewRow();

            dr[0] = txtIDArticle.Text;
            dr[1] = txtNameArt.Text;
            dr[2] = cmbMeasureUnit.SelectedValue.ToString();
            dr[3] = txtQuantity.Text;
            dr[4] = txtPriceArt.Text;
            dr[5] = poreskaOsnovica.ToString();//poreska osnovica
            dr[6] = cmbPDV.SelectedValue.ToString();
            dr[7] = pdvValue.ToString();
            dr[8] = sumCell.ToString();

            dt.Rows.Add(dr);
            txtIDArticle.Focus();
            emptyRow();
        }

        //creating the name of file for invoice
        public string getPathNameForNewFile()
        {
            SaveFileDialog path = new SaveFileDialog();

            if (path.ShowDialog() == true)
            {
                return path.FileName;
            }
            return "";
        }

        //Funcition where we create Invoice and fill header cells in template.xlsx
        //and copy data from dataGrid to place which is planned to be fill with articles
        private void btnCreateOffer_Click(object sender, RoutedEventArgs e)
        {

            if (File.Exists("template.xlsx"))
            {
                if (txtDateCreate.SelectedDate != null && txtDateDelivery.SelectedDate != null
                    && txtUniqueNameOfInvoice.Text != "" && txtCustomerName.Text != "" && txtPIBCustomer.Text != "")
                {

                    try
                    {
                        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

                        var workbook = ExcelFile.Load("template.xlsx");

                        var worksheet = workbook.Worksheets[0];

                        //data of customer

                        worksheet.Cells["G11"].Value = txtCustomerName.Text;
                        worksheet.Cells["G11"].Style.WrapText = true;

                        worksheet.Cells["G12"].Value = txtAdressCustomer.Text;
                        worksheet.Cells["G13"].Value = txtCityPlaceCustomer.Text;
                        worksheet.Cells["G14"].Value = txtPIBCustomer.Text;

                        //data for header of invoice
                        worksheet.Cells["D8"].Value = txtDateCreate.SelectedDate.Value.Date.ToString("dd-MM-yyyy");
                        worksheet.Cells["D9"].Value = txtDateCreate.SelectedDate.Value.Date.ToString("dd-MM-yyyy");
                        worksheet.Cells["D10"].Value = txtDateDelivery.SelectedDate.Value.Date.ToString("dd-MM-yyyy");
                        worksheet.Cells["F17"].Value = txtUniqueNameOfInvoice.Text;

                        //Filling table parts which is predicted for articles in .xlsx
                        int count = articles.Count;
                        for (int i = 0; i < articles.Count; i++)
                        {
                            if (i == 0)
                            {
                                worksheet.Rows.InsertEmpty(21, count - 1);
                            }
                            //serial number
                            worksheet.Cells[$"A{21 + i}"].Value = i + 1;
                            //ID of article
                            worksheet.Cells[$"B{21 + i}"].Value = articles[i].idArt;
                            //name of artilce
                            worksheet.Cells[$"C{21 + i}"].Value = articles[i].nameArt;

                            worksheet.Cells[$"C{21 + i}"].Style.WrapText = true;

                            //Measure unit of articles
                            worksheet.Cells[$"D{21 + i}"].Value = articles[i].measureUnit;
                            //Quantitiy of articles
                            worksheet.Cells[$"E{21 + i}"].Value = articles[i].quatity;
                            worksheet.Cells[$"E{21 + i}"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                            //price of article per one
                            worksheet.Cells[$"F{21 + i}"].Value = articles[i].price;
                            worksheet.Cells[$"F{21 + i}"].Style.NumberFormat = "#,##0.00";

                            //poreska osnovica
                            worksheet.Cells[$"G{21 + i}"].Value = articles[i].poreskaOsnovica;
                            worksheet.Cells[$"G{21 + i}"].Style.NumberFormat = "#,##0.00";

                            //rate PDV 
                            worksheet.Cells[$"H{21 + i}"].Value = articles[i].pdvLevel + "%";
                            worksheet.Cells[$"H{21 + i}"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                            //sum of pdv
                            worksheet.Cells[$"I{21 + i}"].Value = articles[i].pdvValue;
                            worksheet.Cells[$"I{21 + i}"].Style.NumberFormat = "#,##0.00";

                            //Ammount of product 
                            worksheet.Cells[$"J{21 + i}"].Value = articles[i].sumPrice;
                            worksheet.Cells[$"J{21 + i}"].Style.NumberFormat = "#,##0.00";


                            if (i == count - 1)
                            {
                                worksheet.Cells[$"J{21 + count}"].Formula = $"=Sum({worksheet.Cells["G21"]}:{worksheet.Cells["G" + (21 + i)]})";
                                worksheet.Cells[$"J{21 + count + 1}"].Formula = $"=Sum({worksheet.Cells["I21"]}:{worksheet.Cells["I" + (21 + i)]})";
                                worksheet.Cells[$"J{21 + count + 2}"].Formula = $"=Sum({worksheet.Cells["J21"]}:{worksheet.Cells["J" + (21 + i)]})";
                                // worksheet.Cells[$"J{21 + count + 3}"].Value = double.Parse(txtAvans.Text);
                                worksheet.Cells[$"J{21 + count + 4}"].Formula = $"=Sum({worksheet.Cells["J" + (21 + count + 2)]}-{worksheet.Cells["J" + (21 + count + 3)]})";
                                worksheet.Cells[$"D{21 + count + 5}"].Formula = txtAmmountMoneySpell.Text;
                            }

                        }

                        string path = getPathNameForNewFile();

                        if (path != "")
                        {
                            string pathFile = $"{path}{DateTime.Now.ToString("HHmmssMMddyyyy")}.xlsx";
                            workbook.Save(pathFile);
                            MessageBox.Show("Uspesno kreiran Račun", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);

                            Process process = new Process();
                            process.StartInfo.FileName = pathFile;
                            process.StartInfo.Arguments = "ProcessStart.cs";
                            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                            process.StartInfo.UseShellExecute = true;
                            process.Start();

                        }
                        else
                        {
                            MessageBox.Show("Niste uneli naziv fajla", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Došlo je do greške", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

                        throw;
                    }
                }
                else
                {
                    MessageBox.Show("Niste uneli sva polja", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("template.xlsx file is unavailable!!!\n Please contact you administrator!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Empty all textboxes which are filled with data
        public void emptyRow()
        {
            txtIDArticle.Text = "";
            txtNameArt.Text = "";
            txtQuantity.Text = "1";
            txtPriceArt.Text = "";
        }

        // Secting the one cell for editing or deleting
        // If is table emtry selected index is -1
        // we disable button from add new row and enable others
        private void dataGridInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRow = dataGridInvoice.SelectedIndex;

            if (selectedRow >= 0)
            {
                this.selectedCell = true;
                this.idSelectedRow = selectedRow;

                Article selectedArticle = articles[selectedRow];

                txtIDArticle.Text = selectedArticle.idArt;
                txtNameArt.Text = selectedArticle.nameArt;
                txtQuantity.Text = selectedArticle.quatity.ToString();
                txtPriceArt.Text = selectedArticle.price.ToString();
                cmbMeasureUnit.Text = selectedArticle.measureUnit;
                cmbPDV.Text = selectedArticle.pdvLevel;

                btnUpdateCell.IsEnabled = true;
                btnDeleteCell.IsEnabled = true;
                btnCancel.IsEnabled = true;
                btnAddCell.IsEnabled = false;

            }
        }
        // Update selected row in data grid
        private void btnUpdateCell_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Brisanje", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {

                // Fist of all we need to set the values like befoure becouse in next steps we have to set updated values
                double price = articles[idSelectedRow].price;
                double poreskaOsnovica = Math.Round((price / ((100 + double.Parse(cmbPDV.SelectedValue.ToString())) / 100)) * articles[idSelectedRow].quatity, 2);
                double sumCell = Math.Round(price * articles[idSelectedRow].quatity, 2);
                double pdvValue = Math.Round(sumCell - poreskaOsnovica, 2);

                sumAllPDV = Math.Round(sumAllPDV - pdvValue, 2);
                summAllAmountOfArticles = Math.Round(summAllAmountOfArticles - sumCell, 2);
                sumAllPoreskuOsnovicu = Math.Round(sumAllPoreskuOsnovicu - poreskaOsnovica, 2);

                // Set the new values in variables
                price = double.Parse(txtPriceArt.Text);
                poreskaOsnovica = Math.Round((price / ((100 + double.Parse(cmbPDV.SelectedValue.ToString())) / 100)) * double.Parse(txtQuantity.Text), 2);
                sumCell = Math.Round(price * double.Parse(txtQuantity.Text), 2);
                pdvValue = Math.Round(sumCell - poreskaOsnovica, 2);

                articles[idSelectedRow].idArt = txtIDArticle.Text;
                articles[idSelectedRow].nameArt = txtNameArt.Text;
                articles[idSelectedRow].measureUnit = cmbMeasureUnit.SelectedValue.ToString() != "" ? cmbMeasureUnit.SelectedValue.ToString() : "kom";
                articles[idSelectedRow].quatity = double.Parse(txtQuantity.Text);
                articles[idSelectedRow].price = price;
                articles[idSelectedRow].poreskaOsnovica = poreskaOsnovica;
                articles[idSelectedRow].pdvLevel = cmbPDV.SelectedValue.ToString() != "" ? cmbPDV.SelectedValue.ToString() : "20";
                articles[idSelectedRow].pdvValue = pdvValue;
                articles[idSelectedRow].sumPrice = sumCell;

                sumAllPDV = Math.Round(sumAllPDV + pdvValue, 2);
                txtPdvSum.Text = sumAllPDV.ToString();

                summAllAmountOfArticles = Math.Round(summAllAmountOfArticles + sumCell, 2);
                txtAmountOfAllAcrticles.Text = summAllAmountOfArticles.ToString();

                sumAllPoreskuOsnovicu = Math.Round(sumAllPoreskuOsnovicu + poreskaOsnovica, 2);
                txtNetoSum.Text = sumAllPoreskuOsnovicu.ToString();

                dt.Rows[idSelectedRow][0] = txtIDArticle.Text;
                dt.Rows[idSelectedRow][1] = txtNameArt.Text;
                dt.Rows[idSelectedRow][2] = cmbMeasureUnit.SelectedValue.ToString();
                dt.Rows[idSelectedRow][3] = txtQuantity.Text;
                dt.Rows[idSelectedRow][4] = txtPriceArt.Text;
                dt.Rows[idSelectedRow][5] = poreskaOsnovica.ToString();//poreska osnovica
                dt.Rows[idSelectedRow][6] = cmbPDV.SelectedValue.ToString();
                dt.Rows[idSelectedRow][7] = pdvValue.ToString();
                dt.Rows[idSelectedRow][8] = sumCell.ToString();

                emptyRow();
                ResetBtn();
            }
        }
        // Delete selected row in data grid
        private void btnDeleteCell_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Brisanje", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                sumAllPDV = Math.Round(sumAllPDV - articles[idSelectedRow].pdvValue, 2);
                txtPdvSum.Text = sumAllPDV.ToString();

                summAllAmountOfArticles = Math.Round(summAllAmountOfArticles - articles[idSelectedRow].sumPrice, 2);
                txtAmountOfAllAcrticles.Text = summAllAmountOfArticles.ToString();

                sumAllPoreskuOsnovicu = Math.Round(sumAllPoreskuOsnovicu - articles[idSelectedRow].poreskaOsnovica, 2);
                txtNetoSum.Text = sumAllPoreskuOsnovicu.ToString();

                dt.Rows.RemoveAt(idSelectedRow);
                articles.RemoveAt(idSelectedRow);
                ResetBtn();
                emptyRow();
            }
        }
        public void ResetBtn()
        {
            btnUpdateCell.IsEnabled = false;
            btnDeleteCell.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnAddCell.IsEnabled = true;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetBtn();
            emptyRow();
        }

        // Control text box is it input only decimal numbers
        private void txtPriceArt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }
        // Control text box is it input only decimal numbers
        private void txtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }
        // Control text box is it input only decimal numbers
        private void txtAvans_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            if (file.FileName != "")
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = file.FileName;
                    process.StartInfo.Arguments = "ProcessStart.cs";
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                catch (Exception)
                {
                    MessageBox.Show("Došlo je do greške", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Greška", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnOpenInvoice_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            file.ShowDialog();

            if (file.FileName != "")
            {
                txtFile.Text = file.FileName;
                try
                {
                    dynamic fajl = JsonConvert.DeserializeObject(File.ReadAllText(file.FileName));

                    foreach (var art in fajl)
                    {
                        onStocks.Add(new ArticalOnStock()
                        {
                            barkod = art["barkod"],
                            porez = art["porez"],
                            jedinica_mere = art["jedinica_mere"],
                            cena = art["cena"],
                            naziv = art["naziv"],
                            sifra = art["sifra"],
                            vrsta_artikla = art["vrsta_artikla"]
                        });
                    }
                    if (onStocks.Count > 0)
                    {
                        Color color = (Color)ColorConverter.ConvertFromString("#33c500");
                        SolidColorBrush brush = new SolidColorBrush(color);
                        recImported.Fill = brush;

                        importIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
                    }
                    txtIDArticle.Focus();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Došlo je do greške", "Greška", MessageBoxButton.OK, MessageBoxImage.Information);
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati .json file", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtIDArticle_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                foreach (ArticalOnStock article in onStocks)
                {
                    if (article.barkod == txtIDArticle.Text)
                    {
                        txtNameArt.Text = article.naziv;
                        txtPriceArt.Text = article.cena.ToString();
                    }
                }
                txtNameArt.SelectAll();
                txtNameArt.Focus();
            }
        }

        private void txtQuantity_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btnAddCell_Click(sender, e);
            }
        }

        private void txtNameArt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtPriceArt.SelectAll();
                txtPriceArt.Focus();
            }
        }

        private void txtPriceArt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtQuantity.SelectAll();
                txtQuantity.Focus();
            }
        }
    }
}
