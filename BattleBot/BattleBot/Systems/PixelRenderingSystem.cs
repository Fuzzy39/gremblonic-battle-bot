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
    internal class PixelRenderingSystem : EngineCore.System
    {

        private Renderer renderer;

        public PixelRenderingSystem(Engine e) : base(e)
        {
            renderer = e.Renderer;
            addRequiredComponent(typeof(PixelCoords));
            addRequiredComponent(typeof(SimpleTexture));
            initialize(e);
        }

        protected override void draw(GameTime gameTime)
        {
            foreach(Entity entity in entities)
            {
                PixelCoords coords = (PixelCoords)entity.FindComponent<PixelCoords>().First();
                SimpleTexture texture = (SimpleTexture)entity.FindComponent<SimpleTexture>().First();
                renderer.Draw(texture.texture, coords.bounds, texture.tint);
            }
        }

        protected override void update(GameTime gameTime)
        {
           // no update.
        }
    }
}
