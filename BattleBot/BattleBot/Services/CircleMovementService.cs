using BattleBot.Components;
using EngineCore;
using EngineCore.Components;
using EngineCore.Rendering;
using EngineCore.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Services
{
    internal class CircleMovementService : ServiceBase
    {

        // The Entity Type that this service cares about. More complex services may have seperate distinct types.
        public static readonly EntityType EntityType = e => e.HasComponent<MapBounds>() && e.HasComponent<PointRotation>();
        public CircleMovementService(Engine e) : base(e)
        {
            entities.Add(EntityType, []);
        }

        public override void Update(GameTime gameTime)
        {
            // some updating
            foreach (Entity entity in entities[EntityType])
            {
                MapBounds coords = entity.FindComponent<MapBounds>();
                PointRotation pointRotation = entity.FindComponent<PointRotation>();

                // update rotation
                float radians = (float)gameTime.ElapsedGameTime.TotalSeconds * pointRotation.rotationalVelcocity;
                //Console.WriteLine(radians);
                pointRotation.theta += radians;

                // update entity coords.
                Vector2 pos = new Vector2(MathF.Cos(pointRotation.theta), MathF.Sin(pointRotation.theta));
                pos *= pointRotation.radius;
                pos += pointRotation.center.ToVector2();
                pos -= coords.Bounds.Center - coords.Bounds.BoundingBox.Location;
                coords.Bounds.SetBoundingBoxLocation(pos);

                coords.Bounds.RotateBy(Angle.FromRadians((float)gameTime.ElapsedGameTime.TotalSeconds * pointRotation.otherRotationVelocity), new(.5f, .5f));
            }

        }
    }
}
