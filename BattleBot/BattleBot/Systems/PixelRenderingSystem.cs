using BattleBot.Components;
using EngineCore;
using EngineCore.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Systems
{
    internal class PixelRenderingSystem : EngineCore.BasicSystem
    {

        private Renderer renderer;

        public PixelRenderingSystem(Engine e) : base(e)
        {
            renderer = e.Renderer;
            AddRequiredComponent(typeof(PixelBounds));
            AddRequiredComponent(typeof(SimpleTexture));
            Initialize(e);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Entity entity in entities)
            {
                PixelBounds coords = entity.FindComponent<PixelBounds>();
                SimpleTexture texture = entity.FindComponent<SimpleTexture>();
                renderer.Draw(texture.Texture, coords.Bounds, texture.Tint);
            }
        }
    }
}
