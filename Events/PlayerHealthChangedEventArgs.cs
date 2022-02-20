using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmInstances
{
    class PlayerHealthChangedEventArgs : EventArgs
    {
        public int before = 0;
        public int current = 0;
    }
}
