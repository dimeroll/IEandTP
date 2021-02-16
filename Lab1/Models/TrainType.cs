using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1_ICtaTP
{
    public partial class TrainType
    {
        public TrainType()
        {
            Trains = new HashSet<Train>();
        }

        public int TrainTypeId { get; set; }
        public string TrainTypeName { get; set; }

        public virtual ICollection<Train> Trains { get; set; }
    }
}
