using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1_ICtaTP
{
    public partial class DeparturePoint
    {
        public DeparturePoint()
        {
            Journeys = new HashSet<Journey>();
        }

        public int DeparturePointId { get; set; }
        public string DeparturePointName { get; set; }

        public virtual ICollection<Journey> Journeys { get; set; }
    }
}
