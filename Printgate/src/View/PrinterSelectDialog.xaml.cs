using Printgate.Model;
using Printgate.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Printgate.View
{
    /// <summary>
    /// Interaction logic for PrinterSelectDialog.xaml
    /// </summary>
    public partial class PrinterSelectDialog : Window
    {
        private PrinterSelectViewModel viewModel;

        public PrinterSelectDialog()
        {
            InitializeComponent();
        }

        public PrinterSelectDialog(PrinterSelectViewModel viewModel)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            this.viewModel = viewModel;
            DataContext = viewModel;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedCategory == null)
            {
                MessageBox.Show("Please select a food category.", "Printgate");
                return;
            }
            if (viewModel.SelectedPrinter == null)
            {
                MessageBox.Show("Please select a printer.", "Printgate");
                return;
            }

            this.DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
