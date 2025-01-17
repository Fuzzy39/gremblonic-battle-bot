using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Components
{
    /// <summary>
    /// it is not intended that this component survives.
    /// If we've got a camera and keyboard input and this is still around, axe it.
    /// </summary>
    internal class PointRotation : EngineCore.Component
    {
        public Point center = new Point(0,0);
        public float radius = 0;
        public float theta = 0;
        public float rotationalVelcocity = 0; // in radians/second
        public float otherRotationVelocity = 0; // around the center of the entity, not the center defined here.

    }
}
