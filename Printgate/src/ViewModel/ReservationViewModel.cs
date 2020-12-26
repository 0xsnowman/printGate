using Printgate.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Printgate.ViewModel
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<TableReservation> _tableItems;
        private ObservableCollection<TakeAwayReservation> _takeawayItems;
        private ObservableCollection<RoomReservation> _roomItems;

        private GateSettings settings;
        private Gate gate;

        private long maxTableReservationId = 0;
        private long maxTakeAwayReservationId = 0;
        private long maxRoomReservationId = 0;

        private string GET_TABLE_RESERVATION => GenerateUrl(maxTableReservationId, "restaurant");
        private string GET_TAKE_AWAY_RESERVATION => GenerateUrl(maxTakeAwayReservationId, "takeaway");
        private string GET_ROOM_RESERVATION => GenerateUrl(maxRoomReservationId, "room");

        public ReservationViewModel(GateSettings settings)
        {
            this.settings = settings;
            this.gate = new Gate(settings);

            _tableItems = new ObservableCollection<TableReservation>();
            _takeawayItems = new ObservableCollection<TakeAwayReservation>();
            _roomItems = new ObservableCollection<RoomReservation>();
        }

        public void StartGate()
        {
            Task tazk = new Task(GateEngine);
            tazk.Start();
        }

        private async void GateEngine()
        {
            try
            {
                var tableItems = await gate.GetTableDataFromServer(GET_TABLE_RESERVATION);
                if (tableItems.Count != 0)
                {
                    TableItems = new ObservableCollection<TableReservation>(tableItems);
                }

                var takeAwayItems = await gate.GetTakeAwayDataFromServer(GET_TAKE_AWAY_RESERVATION);
                if (takeAwayItems.Count != 0)
                {
                    TakeAwayItems = new ObservableCollection<TakeAwayReservation>(takeAwayItems);
                }

                var roomItems = await gate.GetRoomDataFromServer(GET_ROOM_RESERVATION);
                if (takeAwayItems.Count != 0)
                {
                    RoomItems = new ObservableCollection<RoomReservation>(roomItems);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            await Task.Delay(3000);
            StartGate();
        }

        public ObservableCollection<TableReservation> TableItems
        {
            get { return _tableItems; }
            set { _tableItems = value; OnPropertyChanged("TableItems"); }
        }

        public ObservableCollection<TakeAwayReservation> TakeAwayItems
        {
            get { return _takeawayItems; }
            set { _takeawayItems = value; OnPropertyChanged("TakeAwayItems"); }
        }

        public ObservableCollection<RoomReservation> RoomItems
        {
            get { return _roomItems; }
            set { _roomItems = value; OnPropertyChanged("RoomItems"); }
        }

        public void OnPropertyChanged(string propname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
        }

        public async Task<bool> SendTableDataToServer(TableReservation item, string action)
        {
            var url = GenerateUrl(item.id, action, "restaurant", settings.TableWelcomeMessage);
            var result = await gate.SetTableReservationToServer(url);
            if (result != null)
            {
                TableItems.Remove(item);
                return true;
            }
            return false;
        }
        public async Task<bool> SendTakeAwayDataToServer(TakeAwayReservation item, string action)
        {
            var url = GenerateUrl(item.id, action, "takeaway", settings.FoodEnjoyMessage);
            var result = await gate.SetTakeAwayReservationToServer(url);
            if (result != null)
            {
                TakeAwayItems.Remove(item);
                return true;
            }
            return false;
        }
        public async Task<bool> SendRoomDataToServer(RoomReservation item, string action)
        {
            var url = GenerateUrl(item.id, action, "room", settings.RoomBookingMessage);
            var result = await gate.SetTakeAwayReservationToServer(url);
            if (result != null)
            {
                RoomItems.Remove(item);
                return true;
            }
            return false;
        }

        private string GetServerUrl()
        {
            return string.Format("{0}/index.php?option=com_api&app=vikrestaurants&resource=reservation&format=raw", settings.JoomlaServer);
        }

        private string GenerateUrl(long id, string action = "", string type = "", string message = "")
        {
            return GetServerUrl() + "&id=" + id + "&action=" + action + "&type=" + type + "&message=" + message;
        }

        private string GenerateUrl(long id, string type)
        {
            return GetServerUrl() + "&id=" + id + "&type=" + type;
        }
    }
}
