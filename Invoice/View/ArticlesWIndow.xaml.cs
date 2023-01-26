using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Invoice.View
{
    /// <summary>
    /// Interaction logic for ArticlesWIndow.xaml
    /// </summary>
    public partial class ArticlesWIndow : Window
    {
        List<ArticalOnStock> articles;
        public ArticalOnStock selectedArticle;
        public ArticlesWIndow(List<ArticalOnStock> articles)
        {
            InitializeComponent();
            this.articles = articles;
            dataGridInvoice.ItemsSource = articles;
        }
        public bool IsClosed { get; private set; }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.IsClosed = true;
        }

        private void btnAddCell_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridInvoice.SelectedIndex > -1)
            {

                selectedArticle = (ArticalOnStock)dataGridInvoice.SelectedItem;

            }
            btnClose_Click(sender, e);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtKolicina_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));

        }

        private void txtCena_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtSifra_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }


        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            var filtered = articles.Where<ArticalOnStock>(artikal => artikal.naziv.ToUpper().Contains(txtSearch.Text.ToUpper()) || artikal.sifra.ToUpper().Contains(txtSearch.Text.ToUpper()));
            dataGridInvoice.ItemsSource = filtered;
        }

        private void dataGridInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedArticle = (ArticalOnStock)dataGridInvoice.SelectedItem;

            txtNaziv.Text = selectedArticle.naziv;
            txtSifra.Text = selectedArticle.sifra;
            txtCena.Text = selectedArticle.cena.ToString();
            txtKolicina.Text = "1";

        }
    }
}
