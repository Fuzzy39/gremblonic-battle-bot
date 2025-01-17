using EngineCore.Util.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Components
{
    /// <summary>
    /// a description of the entities bounds in a game-world.
    /// </summary>
    internal class WorldBounds : EngineCore.Component
    {

        /// <summary>
        /// a description of the entities position in a game-world, in 'tiles'.
        /// </summary>
        public RotatedRect Bounds { get; set; }
    }

}
