using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1_ICtaTP
{
    public partial class Role
    {
        public Role()
        {
            TrainWorkers = new HashSet<TrainWorker>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<TrainWorker> TrainWorkers { get; set; }
    }
}
