using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmInstances
{
    class PlayerReceivedXPEventArgs : EventArgs
    {
        public int experience_type = -1;
        public int before = -1;
        public int current = -1;
    }
}
