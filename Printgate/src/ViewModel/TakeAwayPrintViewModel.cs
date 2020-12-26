using Printgate.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Data;

namespace Printgate.ViewModel
{
    public class TakeAwayPrintViewModel
    {
        public string SelectedPrinter { get; set; }

        public CollectionView Printers { get; set; }

        public FoodCategory SelectedCategory { get; set; }

        public CollectionView FoodCategories { get; set; }

        public TakeAwayPrintViewModel(IEnumerable<string> printerList, IEnumerable<FoodCategory> categories)
        {
            Printers = new CollectionView(printerList);
            FoodCategories = new CollectionView(categories);
        }
    }
}
