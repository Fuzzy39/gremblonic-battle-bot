using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore.Components
{
    public class SimpleTexture : Component
    {
        // about as simple as possible. Could account for spritesheets or something but that's not the point right now.
        public required Texture2D Texture { get;  set; }
        public required Color Tint { get;  set; }

        public byte Priority { get;  set; }
    }
}
