using EngineCore.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore.Services
{
    internal class VelocityService : ServiceBase
    {

        // The Entity Type that this service cares about. More complex services may have seperate distinct types. Feel free to change the name.
        public static readonly EntityType EntityType = e => e.HasComponent<Velocity>() && e.HasComponent<MapBounds>();


        public VelocityService(Engine e) : base(e)
        {
            entities.Add(EntityType, []);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Entity entity in entities[EntityType])
            {
                Vector2 vel = entity.FindComponent<Velocity>()!.Value;
                float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 toAdd = new(vel.X * seconds, vel.Y*seconds);

                entity.FindComponent<MapBounds>()!.Bounds.Center += toAdd;

            }
        }
    }
}
