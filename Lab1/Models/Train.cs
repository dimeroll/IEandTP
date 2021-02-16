using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1_ICtaTP
{
    public partial class Train
    {
        public Train()
        {
            Journeys = new HashSet<Journey>();
            TrainWorkers = new HashSet<TrainWorker>();
        }

        public int TrainId { get; set; }
        public int TrainTypeId { get; set; }
        public string AdditionalInfo { get; set; }

        public virtual TrainType TrainType { get; set; }
        public virtual ICollection<Journey> Journeys { get; set; }
        public virtual ICollection<TrainWorker> TrainWorkers { get; set; }
    }
}
