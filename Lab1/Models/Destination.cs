using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1_ICtaTP
{
    public partial class Destination
    {
        public Destination()
        {
            Journeys = new HashSet<Journey>();
        }

        public int DestinationId { get; set; }
        public string DestinationName { get; set; }

        public virtual ICollection<Journey> Journeys { get; set; }
    }
}
