using System;
using System.Collections.Generic;
using System.Text;

namespace Printgate.Model
{
    public class RoomReservation
    {
        public long id { get; set; }
        public string ts { get; set; }
        public string custdata { get; set; }
        public string phone { get; set; }
        public string custmail { get; set; }
        public string status { get; set; }

        public string Name => custdata + "[" + phone + "]";

        public string Date => _gate.GetDataTimeFromTimeStamp(double.Parse(ts));

        public string Email => custmail;

        public string Status => status;

        private Gate _gate;
        public RoomReservation()
        {
            _gate = new Gate();
        }
    }
}
