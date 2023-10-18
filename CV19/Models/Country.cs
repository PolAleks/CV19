using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV19.Models
{
    internal class PlaceInfo
    {
        public string Name { get; set; }

        public Point Location { get; set; }

        // Кол-во подтвержденных случаев
        public IEnumerable<ConfirmedCount> Counts { get; set; }
    }

}
