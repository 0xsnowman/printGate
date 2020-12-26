using Newtonsoft.Json.Linq;
using Printgate.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Printgate.ViewModel;

namespace Printgate.View
{
    /// <summary>
    /// Interaction logic for ReservationView.xaml
    /// </summary>
    public partial class ReservationView : Window
    {
        private ReservationViewModel viewModel;

        public ReservationView()
        {
            InitializeComponent();
        }

        public ReservationView(ReservationViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = viewModel;
            ProgressBar.Visibility = Visibility.Hidden;
        }

        private void BeforeAsync()
        {
            ProgressBar.Visibility = Visibility.Visible;
            Mouse.OverrideCursor = Cursors.Wait;
            TakeAwayConfirmButton.IsEnabled = false;
            TakeAwayRejectButton.IsEnabled = false;
            TakeAwayWelcomeButton.IsEnabled = false;
            TableConfirmButton.IsEnabled = false;
            TableRejectButton.IsEnabled = false;
            TableWelcomeButton.IsEnabled = false;
        }
        private void AfterAsync()
        {
            ProgressBar.Visibility = Visibility.Hidden;
            Mouse.OverrideCursor = null;
            TakeAwayConfirmButton.IsEnabled = true;
            TakeAwayRejectButton.IsEnabled = true;
            TakeAwayWelcomeButton.IsEnabled = true;
            TableConfirmButton.IsEnabled = true;
            TableRejectButton.IsEnabled = true;
            TableWelcomeButton.IsEnabled = true;
        }

        private TableReservation SelectedTableReservationItemId()
        {
            TableReservation item;
            try
            {
                item = TableReservationListView.SelectedItems[0] as TableReservation;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
            
            return item;
        }

        //Table reservation
        private async void TableConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var item = SelectedTableReservationItemId();
            if (item != null)
            {
                BeforeAsync();
                await viewModel.SendTableDataToServer(item, "confirm");
                AfterAsync();
            }
        }

        private async void TableRejectButton_Click(object sender, RoutedEventArgs e)
        {
            var item = SelectedTableReservationItemId();
            if (item != null)
            {
                BeforeAsync();
                await viewModel.SendTableDataToServer(item, "reject");
                AfterAsync();
            }
        }

        private async void TableWelcomeButton_Click(object sender, RoutedEventArgs e)
        {
            var item = SelectedTableReservationItemId();
            if (item != null)
            {
                BeforeAsync();
                await viewModel.SendTableDataToServer(item, "welcome");
                AfterAsync();
            }
        }

        private TakeAwayReservation SelectedTakeAwayReservationItemId()
        {
            TakeAwayReservation item;
            try
            {
                item = TakeAwayReservationListView.SelectedItems[0] as TakeAwayReservation;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }

            return item;
        }
        //Take away reservation
        private async void TakeAwayConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var item = SelectedTakeAwayReservationItemId();
            if (item != null)
            {
                BeforeAsync();
                await viewModel.SendTakeAwayDataToServer(item, "confirm");
                AfterAsync();
            }
        }

        private async void TakeAwayRejectButton_Click(object sender, RoutedEventArgs e)
        {
            var item = SelectedTakeAwayReservationItemId();
            if (item != null)
            {
                BeforeAsync();
                await viewModel.SendTakeAwayDataToServer(item, "reject");
                AfterAsync();
            }
        }

        private async void TakeAwayWelcomeButton_Click(object sender, RoutedEventArgs e)
        {
            var item = SelectedTakeAwayReservationItemId();
            if (item != null)
            {
                BeforeAsync();
                await viewModel.SendTakeAwayDataToServer(item, "welcome");
                AfterAsync();
            }
        }

        private RoomReservation SelectedRoomReservationItemId()
        {
            RoomReservation item;
            try
            {
                item = RoomReservationListView.SelectedItems[0] as RoomReservation;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }

            return item;
        }

        //Room reservation
        private async void RoomConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var item = SelectedRoomReservationItemId();
            if (item != null)
            {
                BeforeAsync();
                await viewModel.SendRoomDataToServer(item, "confirm");
                AfterAsync();
            }
        }

        private async void RoomRejectButton_Click(object sender, RoutedEventArgs e)
        {
            var item = SelectedRoomReservationItemId();
            if (item != null)
            {
                BeforeAsync();
                await viewModel.SendRoomDataToServer(item, "reject");
                AfterAsync();
            }
        }

        private async void RoomWelcomeButton_Click(object sender, RoutedEventArgs e)
        {
            var item = SelectedRoomReservationItemId();
            if (item != null)
            {
                BeforeAsync();
                await viewModel.SendRoomDataToServer(item, "welcome");
                AfterAsync();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}
