using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmInstances
{
    class PlayerStaminaChangedEventArgs : EventArgs
    {
        public float before = 0;
        public float current = 0;
    }
}
