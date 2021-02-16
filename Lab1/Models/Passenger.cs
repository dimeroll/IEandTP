using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1_ICtaTP
{
    public partial class Passenger
    {
        public Passenger()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int PassportId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
