using EngineCore.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Components
{
    /// <summary>
    /// a position on the screen.
    /// </summary>
    internal class PixelBounds : EngineCore.Component
    {
        /// <summary>
        /// a description of the entities position on the screen, in (virtual) pixels.
        /// </summary>
        public RotatedRect Bounds {  get; set; }
    }
}
