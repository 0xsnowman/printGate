using System;
using System.Collections.Generic;
using System.Text;

namespace Printgate.Model
{
    public class TakeAwayPrinter
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        private string printerName = string.Empty;

        public string PrinterName
        {
            get
            {
                return printerName;
            }
            set
            {
                printerName = value;
            }
        }
    }
}
