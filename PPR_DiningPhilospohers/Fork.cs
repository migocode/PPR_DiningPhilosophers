using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPR_DiningPhilospohers
{
    public record Fork
    {
        public int ForkNumber { get; }
        public bool IsUsed { get; set; }

        public Fork(int forkNumber)
        {
            ForkNumber = forkNumber;
        }
    }
}
