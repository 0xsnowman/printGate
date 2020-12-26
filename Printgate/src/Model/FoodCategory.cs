using System;
using System.Collections.Generic;
using System.Text;

namespace Printgate.Model
{
    public class FoodCategory
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public FoodCategory()
        { 
        
        }

        public FoodCategory(int ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }
    }
}
