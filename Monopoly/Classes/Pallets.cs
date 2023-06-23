using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly.Classes
{
    internal class Pallets
    {
        public int ID { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float Depth { get; set; }

        public float Volume
        {
            get
            {
                var thisPalleteBoxes = ListClass.BoxesList.Where(f => f.PalleteID == ID).ToList();
                return thisPalleteBoxes.Count != 0 ? thisPalleteBoxes.Sum(f => f.Volume) + (Width * Height * Depth) : Width * Height * Depth;
            }
        }

        public float Weight 
        {
            get
            {
                var thisPalleteBoxes = ListClass.BoxesList.Where(f => f.PalleteID == ID).ToList();
                return thisPalleteBoxes.Count != 0 ?  thisPalleteBoxes.Sum(f => f.Weight) + 30 : 30;
            }
        }

        public DateOnly ExpirationDate
        {
            get
            {
                var thisPalleteBox = ListClass.BoxesList.Where(f => f.PalleteID == ID).ToList();
                var firstBox = thisPalleteBox.OrderBy(x => x.ExpirationDate).First();
                return thisPalleteBox != null ? firstBox.ExpirationDate : new DateOnly(0, 0, 0);
            }
        }

    }
}
