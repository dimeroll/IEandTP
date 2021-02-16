using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1_ICtaTP
{
    public partial class Ticket
    {
        public int TicketId { get; set; }
        public int Place { get; set; }
        public int PassengerId { get; set; }
        public int JourneyId { get; set; }

        public virtual Journey Journey { get; set; }
        public virtual Passenger Passenger { get; set; }
    }
}
