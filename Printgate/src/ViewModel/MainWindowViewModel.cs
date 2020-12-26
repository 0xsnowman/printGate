
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Printing;
using System.Printing.IndexedProperties;
using System.Collections;
using System.Diagnostics;
using Printgate.Model;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Data;
using Printgate.View;

namespace Printgate.ViewModel
{
    public class MainWindowViewModel
    {

        public Printers Printers { get; set; } = new Printers();

        public ObservableCollection<FoodCategory> FoodCategories { get; set; }

        public GateSettings Settings { get; set; }

        public CollectionView PrinterList { get; set; }

        public MainWindowViewModel()
        {
            Settings = LoadSettings() ?? new GateSettings();

            FoodCategories = new ObservableCollection<FoodCategory>();
            FoodCategories.Add(new FoodCategory(1, "Pizza and Spagetti "));
            FoodCategories.Add(new FoodCategory(2, "Pasta"));
            FoodCategories.Add(new FoodCategory(3, "Printgate"));
        }

        internal void SaveSettings(UIElementCollection printerCollection)
        {
            Settings.TakeAwayPrinters.Clear();
            foreach (var item in printerCollection)
            {
                var printer = new TakeAwayPrinter();

                var grid = item as Grid;
                var cbCategory = grid.Children[0] as ComboBox;
                var category = cbCategory.SelectedValue as FoodCategory;
                printer.CategoryId = category.ID;
                printer.CategoryName = category.Name;

                var cbPrinters = grid.Children[1] as ComboBox;
                printer.PrinterName = cbPrinters.SelectedValue as String;

                Settings.TakeAwayPrinters.Add(printer);
            }

            try
            {
                // serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(@"settings.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, Settings);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        internal GateSettings LoadSettings()
        {
            string fileName = @"settings.json";

            if (!File.Exists(fileName))
                return null;

            try
            {
                // Deserialize JSON directly from a file
                using (StreamReader file = File.OpenText(fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return (GateSettings)serializer.Deserialize(file, typeof(GateSettings));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }
    }
}
