using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Classes
{
    internal class Boxes
    {
        public int ID { get; set; }

        public DateTime ManufactureDate { get; set; }

        public int PalleteID { get; set; }

        public float Weight { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float Depth { get; set; }

        public float Volume => (Width * Height * Depth) / 10000;

        public DateOnly ExpirationDate
        {
            get
            {
                if (ManufactureDate != new DateTime(0001, 1, 1))
                {
                    return DateOnly.FromDateTime(ManufactureDate.AddDays(100));

                }
                else
                {
                    return new DateOnly(2023, 6, 29);
                }
            }
        }
        
    }
}
