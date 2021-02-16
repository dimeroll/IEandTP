using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1_ICtaTP
{
    public partial class Journey
    {
        public Journey()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int JourneyId { get; set; }
        public int TrainId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int DeparturePointId { get; set; }
        public int DestinationId { get; set; }

        public virtual DeparturePoint DeparturePoint { get; set; }
        public virtual Destination Destination { get; set; }
        public virtual Train Train { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
