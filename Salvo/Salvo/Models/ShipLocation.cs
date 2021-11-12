using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public class ShipLocation
    {
        public long Id { get; set; }

        public string Location { get; set; }
        //Para la relacion con ship
        public long ShipId { get; set; }

        public Ship Ship { get; set; }
    }
}
