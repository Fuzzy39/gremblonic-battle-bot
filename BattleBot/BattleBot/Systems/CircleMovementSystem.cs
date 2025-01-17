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
    internal class CircleMovementSystem : EngineCore.System
    {
        public CircleMovementSystem(Engine e) : base(e)
        {
            addRequiredComponent(typeof(PixelCoords));
            addRequiredComponent(typeof(PointRotation));
            PrimaryComponent = typeof(PointRotation);
            initialize(e);
        }

        protected override void draw(GameTime gameTime)
        {
            // no drawing
        }

        protected override void update(GameTime gameTime)
        {
            // some updating
            foreach (Entity entity in entities)
            {
                PixelCoords coords = (PixelCoords)entity.FindComponent<PixelCoords>().First();
                PointRotation pointRotation = (PointRotation)entity.FindComponent<PointRotation>().First();

                // update rotation
                float radians = (float)gameTime.ElapsedGameTime.TotalSeconds * pointRotation.rotationalVelcocity;
                //Console.WriteLine(radians);
                pointRotation.theta += radians;

                // update entity coords.
                Vector2 pos = new Vector2(MathF.Cos(pointRotation.theta), MathF.Sin(pointRotation.theta));
                pos *= pointRotation.radius;
                pos += pointRotation.center.ToVector2();
                pos -= (coords.bounds.Center - coords.bounds.BoundingBox.Location);
                coords.bounds.SetBoundingBoxLocation(pos);

                coords.bounds.RotateBy((float)gameTime.ElapsedGameTime.TotalSeconds * pointRotation.otherRotationVelocity, new(.5f, .5f));
            }
           
        }
    }
}
