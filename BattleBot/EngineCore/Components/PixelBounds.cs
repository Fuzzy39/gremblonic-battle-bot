using EngineCore.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore.Components
{
    /// <summary>
    /// a position on the screen.
    /// </summary>
    public class PixelBounds : Component
    {
        /// <summary>
        /// a description of the entities position on the screen, in (virtual) pixels.
        /// </summary>
        required public RotatedRect Bounds { get; set; }
    }
}
