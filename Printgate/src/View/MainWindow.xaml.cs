using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using System.Printing;
using Printgate.ViewModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Printgate.Model;
using System.Media;
using Winforms = System.Windows.Forms;

namespace Printgate.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        private ReservationView reservView;

        private ReservationViewModel reservViewModel;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            viewModel = new MainWindowViewModel();
            DataContext = viewModel;

            AddTakeAwayPrinters();
            SetupSystemTrayIcon();

            SetCheckBoxStateEvent(CheckBoxGroup.Children);

            reservViewModel = new ReservationViewModel(viewModel.Settings);
            reservViewModel.PropertyChanged += ReservationItem_Changed;
            reservViewModel.StartGate();
        }

        private void SetCheckBoxStateEvent(UIElementCollection collection)
        {
            foreach (var item in collection)
            {
                if (item.GetType() == typeof(CheckBox))
                {
                    (item as CheckBox).Checked += CheckBox_Changed;
                    (item as CheckBox).Unchecked += CheckBox_Changed;
                }
                if (item.GetType() == typeof(Grid))
                {
                    //SetCheckBoxStateEvent((item as Grid).Children);
                }
            }

            TableAlarmCheck.IsEnabled = viewModel.Settings.IsTablePopup;
            TakeAlarmCheck.IsEnabled = viewModel.Settings.IsTakePopup;
            RoomAlarmCheck.IsEnabled = viewModel.Settings.IsRoomPopup;
        }

        private void AddTakeAwayPrinters()
        {
            var printers = new List<TakeAwayPrinter>(viewModel.Settings.TakeAwayPrinters);
            foreach (var printer in printers)
            {
                AddPrinter(printer.CategoryName, printer.PrinterName);
            }

            foreach (var item in PrinterList.Children)
            {
                var grid = item as Grid;
                foreach (var combo in grid.Children)
                {
                    if (combo.GetType() == typeof(ComboBox))
                    {
                        (combo as ComboBox).SelectionChanged += ComboBox_SelectionChanged;
                    }
                }
            }
        }

        private void AddPrinterButton_Click(object sender, RoutedEventArgs e)
        {
            AddPrinter();
        }

        private void AddPrinter(string selectedCategory = "", string selectedPrinter = "")
        {
            // Create Stackpanel
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            // Create Food Category Combobox
            var categories = new ComboBox();
            categories.Margin = new Thickness(0, 0, 10, 16);
            categories.DisplayMemberPath = "Name";
            categories.SetBinding(ComboBox.ItemsSourceProperty, new Binding("FoodCategories") { Source = viewModel });
            categories.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(selectedCategory))
            {
                var found = viewModel.FoodCategories.First(it => it.Name == selectedCategory);
                if (found != null)
                    categories.SelectedValue = found;
            }
            Grid.SetColumn(categories, 0);
            grid.Children.Add(categories);

            // Create Printers Combobox
            var printers = new ComboBox();
            printers.Margin = new Thickness(10, 0, 0, 16);
            printers.SetBinding(ComboBox.ItemsSourceProperty, new Binding("Printers.PrinterNames") { Source = viewModel });
            printers.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(selectedPrinter))
                printers.SelectedValue = selectedPrinter;
            printers.DropDownOpened += new EventHandler((object sender, EventArgs e) =>
            {
                viewModel.Printers.GetPrinterList();
            });
            Grid.SetColumn(printers, 1);
            grid.Children.Add(printers);

            PrinterList.Children.Add(grid);
        }

        private void SetupSystemTrayIcon()
        {
            try
            {
                var icon = new Winforms.NotifyIcon();
                icon.Icon = new System.Drawing.Icon("printer.ico");
                icon.Visible = true;
                icon.BalloonTipText = "Printgate";
                icon.BalloonTipTitle = "Printgate";
                icon.BalloonTipIcon = Winforms.ToolTipIcon.Info;
                icon.ShowBalloonTip(1500);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void SaveSettings()
        {
            viewModel.SaveSettings(PrinterList.Children);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();

        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveSettings();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveSettings();
        }

        private void CheckBox_Changed(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void TablePopupCheck_Click(object sender, RoutedEventArgs e)
        {
            TableAlarmCheck.IsEnabled = (TablePopupCheck.IsChecked == true);
            SaveSettings();
        }

        private void TakePopupCheck_Click(object sender, RoutedEventArgs e)
        {
            TakeAlarmCheck.IsEnabled = (TakePopupCheck.IsChecked == true);
            SaveSettings();
        }

        private void RoomPopupCheck_Click(object sender, RoutedEventArgs e)
        {
            RoomAlarmCheck.IsEnabled = (RoomPopupCheck.IsChecked == true);
            SaveSettings();
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SaveSettings();
        }

        private void ReservationItem_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TableItems" && viewModel.Settings.IsTablePopup)
            {
                Dispatcher.Invoke(() => StartReservationWindow(viewModel.Settings.IsTableAlarm));
            }
            if (e.PropertyName == "TakeAwayItems" && viewModel.Settings.IsTakePopup)
            {
                Dispatcher.Invoke(() => StartReservationWindow(viewModel.Settings.IsTakeAlarm));
            }
            if (e.PropertyName == "RoomItems" && viewModel.Settings.IsRoomPopup)
            {
                Dispatcher.Invoke(() => StartReservationWindow(viewModel.Settings.IsRoomAlarm));
            }
        }

        private void StartReservationWindow(bool alram)
        {
            if (alram)
                Alram();

            if (reservView == null)
                reservView = new ReservationView(reservViewModel);

            if (reservView.IsVisible == false)
                reservView.Show();
        }

        private void Alram()
        {
            try
            {
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = "bird.wav";
                player.Play();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
