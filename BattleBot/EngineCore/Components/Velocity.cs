using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore.Components
{
    public class Velocity : Component
    {
        /// <summary>
        /// In units/second.
        /// </summary>
        public Vector2 Value { get; set; }
    }
}
