using EngineCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore.Components
{
    /// <summary>
    /// a description of the entities bounds in a game-world.
    /// </summary>
    public class WorldBounds : Component
    {

        /// <summary>
        /// a description of the entities position in a game-world, in 'tiles'.
        /// </summary>
        required public RotatedRect Bounds { get; set; }
    }

}
