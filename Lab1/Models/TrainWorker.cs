using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1_ICtaTP
{
    public partial class TrainWorker
    {
        public int WorkerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int TrainId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Train Train { get; set; }
    }
}
