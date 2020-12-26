using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace Printgate.Model
{
    public class GateSettings : BaseModel
    {
        public string JoomlaServer { get; set; }
        public string TablePrinter { get; set; }

        public List<TakeAwayPrinter> TakeAwayPrinters = new List<TakeAwayPrinter>();

        public string BeautifulPrinter { get; set; }
        public bool IsExportXml { get; set; }
        public bool IsAlarmIfNoConnection { get; set; }
        public bool IsWelcomeMessage { get; set; }
        public bool IsFullAddress { get; set; }
        public bool IsTablePopup { get; set; }
        public bool IsTableAlarm { get; set; }
        public bool IsTakePopup { get; set; }
        public bool IsTakeAlarm { get; set; }
        public bool IsRoomPopup { get; set; }
        public bool IsRoomAlarm { get; set; }

        public string TableWelcomeMessage { get; set; }
        public string FoodEnjoyMessage { get; set; }
        public string RoomBookingMessage { get; set; }
        
        public GateSettings()
        {
            
        }
    }
}
