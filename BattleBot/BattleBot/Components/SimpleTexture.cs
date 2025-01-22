using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Components
{
    internal class SimpleTexture : EngineCore.Component 
    {
        // about as simple as possible. Could account for spritesheets or something but that's not the point right now.
        public Texture2D texture;
        public Color tint;

        public Texture2D Texture { get; internal set; }
        public Color Tint { get; internal set; }
    }
}
