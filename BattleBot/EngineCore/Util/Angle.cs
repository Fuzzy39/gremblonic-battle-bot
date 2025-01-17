using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore.Util
{
    public class Angle
    {

        // always between -pi and pi.
        private float radians;

        public float Degrees
        {
            get
            {
                return (radians / MathHelper.Pi * 180f);
            }

            set
            {
                radians = MathHelper.WrapAngle(value / 180f * MathHelper.Pi);
            }
        }

        public float Radians
        {
            get { return radians; }
            set { radians = MathHelper.WrapAngle(value); }
        }


        private Angle()
        {
            radians = 0;
        }

        public static Angle FromRadians(float radians)
        {
            Angle a = new Angle();
            a.Radians = radians;
            return a;
        }

        public static Angle FromDegrees(float degrees)
        {
            Angle a = new Angle();
            a.Degrees = degrees;
            return a;
        }


        public static Angle operator +(Angle a) => a;
        public static Angle operator -(Angle a) => FromRadians(-a.Radians);

        public static Angle operator +(Angle a, Angle b)
            => FromRadians(a.radians + b.radians);

        public static Angle operator -(Angle a, Angle b)
            => a + (-b);

        public static Angle operator *(float m, Angle a)
            => FromRadians(m * a.radians);

        public static Angle operator *(Angle a, float m)
          => FromRadians(m * a.radians);

        public static Angle operator /(Angle a, float m)
         => FromRadians(a.radians / m);

    }
}
