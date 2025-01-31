using EngineCore.Components;
using EngineCore.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore.Services
{
    internal class PixelRenderingService : ServiceBase
    {

        // The Entity Type that this service cares about. More complex services may have seperate distinct types.
        public static readonly EntityType EntityType = e => e.HasComponent<PixelBounds>() && e.HasComponent<SimpleTexture>();

        private readonly Renderer renderer;


        public PixelRenderingService(Engine e) : base(e)
        {
            renderer = e.Renderer;
            entities.Add(EntityType, []);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Entity entity in entities[EntityType])
            {
                PixelBounds coords = entity.FindComponent<PixelBounds>()!;
                SimpleTexture texture = entity.FindComponent<SimpleTexture>()!;
                renderer.Draw(texture.Texture, coords.Bounds, texture.Tint, texture.Priority);
            }
        }
    }
}
