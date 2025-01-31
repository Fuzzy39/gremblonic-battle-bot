using EngineCore.Components;
using EngineCore.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore.Rendering
{
    /// <summary>
    /// Abstracts a Camera Entity.
    /// </summary>
    public class Camera
    {


        // The Entity Type that this service cares about. More complex services may have seperate distinct types.
        public static readonly EntityType EntityType = new(e => e.HasComponent<CameraComponent>() && e.HasComponent<PixelBounds>());

        private readonly Entity entity;

        public Entity Entity { get { return entity; } }

        public Camera(Entity e)
        {
            entity = e;
            if (!e.IsOfType(EntityType))
            {
                throw new ArgumentException("This Entity " + e.ToString() + " Cannot be used as a camera.");
            }
        }

        public RotatedRect WorldBounds
        {
            get
            {
                RotatedRect toReturn = new(
                    new Vector2(0, 0),
                    PixelBounds.Size / Component.Scale,
                    -Component.Rotation,
                    new(.5f, .5f)
                );

                toReturn.Center = Component.Position;

                return toReturn;
            }
        }

        public CameraComponent Component
        {
            get
            {
                return entity.FindComponent<CameraComponent>()!;
            }
        }

        public RotatedRect PixelBounds
        {
            get
            {
                return entity.FindComponent<PixelBounds>()!.Bounds;
            }

        }

        public RotatedRect ToPixelBounds(RotatedRect objectBounds)
        {
            Vector2 size = objectBounds.Size * Component.Scale;
            Angle rotation = Component.Rotation + objectBounds.Rotation;


            RotatedRect RenderBounds = new(new RectangleF(0, 0, PixelBounds.Width, PixelBounds.Height), Angle.FromRadians(0), new());

            // a glorious transformation. Hopefully it works.
            Vector2 internalRep = WorldBounds.ToInternalRepresentation(objectBounds.TopLeft);

            Vector2 pos = RenderBounds.FromInternalRepresentation(internalRep);

            // oh god hopefully this doesn't explode.
            return new(pos, size, rotation, new());


        }


        public void Render(Renderer renderer, Entity renderable)
        {
            // we must convert world bou
            RotatedRect renderableWorldBounds = renderable.FindComponent<MapBounds>()!.Bounds;
            RotatedRect spriteBounds = ToPixelBounds(renderableWorldBounds);

            SimpleTexture st = renderable.FindComponent<SimpleTexture>()!;
            renderer.Draw(st.Texture, spriteBounds, st.Tint, st.Priority);
        }


        public void RenderChunk(Renderer renderer, Entity chunk)
        {

            RotatedRect chunkPixelBounds = ToPixelBounds(chunk.FindComponent<MapBounds>()!.Bounds);

            ChunkData data = chunk.FindComponent<ChunkData>()!;


            Point size = new(data.Tiles.Count, data.Tiles[0].Count);
            Vector2 pixelSize = new(
                (1.0f / size.X) * chunkPixelBounds.Width, 
                (1.0f / size.Y) * chunkPixelBounds.Height);

            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    if (data.Tiles[x][y] == null) continue;

                    Vector2 internalPosition = new( ((float)x)/size.X, ((float)y)/ size.Y );
                    Vector2 position = chunkPixelBounds.FromInternalRepresentation(internalPosition);
                    RotatedRect tilePixelBounds = new RotatedRect(new RectangleF(position, pixelSize), chunkPixelBounds.Rotation, new(0));

                    SimpleTexture st = data.Tiles[x][y]!.FindComponent<SimpleTexture>()!;
                    renderer.Draw(st.Texture, tilePixelBounds, st.Tint, st.Priority);

                }
            }
        }

    }
}
