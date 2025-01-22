using EngineCore.Util;
using Microsoft.Xna.Framework;
using static System.MathF;

namespace EngineCore.Util.Graphics
{
    public class RotatedRect
    {

        // A rotated rectangle is a rectangleF that can be rotated, I guess?



        public float X { get; private set; }
        public float Y { get; private set; }

        public float Width { get; set; }
        public float Height { get; set; }


        /// <summary>
        ///  The clockwise Rotation in radians, about the top left corner
        ///  Should be between -pi and pi.
        /// </summary>
        public float Rotation { get; private set; }


        /// <summary>
        /// The size of the rectangle. These widths and heights will not change or switch as the rectangle is rotated.
        /// </summary>
        public Vector2 Size
        {
            get { return new Vector2(Width, Height); }
        }

        /// <summary>
        /// The size of the rectangle, width and height swapping when it is rotated sideways.
        /// </summary>
        public Vector2 AdjustedSize
        {
            get
            {

                if (DirectionUtil.FromRadians(Rotation).IsHorizontal())
                {
                    return new(Height, Width);
                }
                else
                {
                    return new(Width, Height);
                }
            }
        }


        public RectangleF AsRectangleF
        {
            get
            {
                float w = Width;
                float h = Height;
                return new RectangleF(X, Y, w, h);
            }
        }

        public Rectangle AsRectangle
        {
            get
            {
                return AsRectangleF.toRectangle();
            }
        }

        public Vector2 TopLeft
        {
            get
            {
                return PositionOfSizeRelativePoint(0, 0);
            }

            set { SetPositionOfSizeRelativePoint(0, 0, value); }
        }

        public Vector2 TopCenter
        {

            get
            {
                return PositionOfSizeRelativePoint(.5f, 0f);
            }

            set { SetPositionOfSizeRelativePoint(.5f, 0, value); }
        }

        public Vector2 TopRight
        {
            get
            {
                return PositionOfSizeRelativePoint(1f, 0f);
            }
            set { SetPositionOfSizeRelativePoint(1f, 0, value); }
        }



        public Vector2 CenterLeft
        {
            get
            {
                return PositionOfSizeRelativePoint(0, .5f);
            }
            set { SetPositionOfSizeRelativePoint(0, .5f, value); }
        }

        public Vector2 Center
        {
            get
            {
                return PositionOfSizeRelativePoint(.5f, .5f);
            }
            set { SetPositionOfSizeRelativePoint(.5f, .5f, value); }
        }

        public Vector2 CenterRight
        {
            get
            {
                return PositionOfSizeRelativePoint(1, .5f);
            }

            set { SetPositionOfSizeRelativePoint(1, .5f, value); }
        }



        public Vector2 BottomLeft
        {
            get
            {
                return PositionOfSizeRelativePoint(0, 1);
            }

            set { SetPositionOfSizeRelativePoint(0, 1f, value); }
        }

        public Vector2 BottomCenter
        {
            get
            {
                return PositionOfSizeRelativePoint(.5f, 1f);
            }

            set { SetPositionOfSizeRelativePoint(.5f, 1f, value); }
        }



        public Vector2 BottomRight
        {
            get
            {
                return PositionOfSizeRelativePoint(1f, 1f);
            }

            set { SetPositionOfSizeRelativePoint(1f, 1f, value); }
        }






        public RectangleF BoundingBox
        {
            get
            {
                float X1 = MiscUtil.PickLeast(TopLeft.X, TopRight.X, BottomLeft.X, BottomRight.X);
                float X2 = MiscUtil.PickGreatest(TopLeft.X, TopRight.X, BottomLeft.X, BottomRight.X);

                float Y1 = MiscUtil.PickLeast(TopLeft.Y, TopRight.Y, BottomLeft.Y, BottomRight.Y);
                float Y2 = MiscUtil.PickGreatest(TopLeft.Y, TopRight.Y, BottomLeft.Y, BottomRight.Y);

                return new RectangleF(X1, Y1, X2 - X1, Y2 - Y1);

            }
        }


        private Vector2 PositionOfSizeRelativePoint(float x, float y)
        {
            float OwnX = x * Width;
            float OwnY = y * Height;

            // do a bit of trig
            float realX = Cos(-Rotation) * OwnX + Sin(-Rotation) * OwnY;
            float realY = Sin(Rotation) * OwnX + Cos(Rotation) * OwnY;

            return new Vector2(realX + X, realY + Y);

        }

        /// <summary>
        /// Returns the position of a point of the RotatedRect's internal reference frame, where its top-left corner is (0,0), and it's bottom right corner is (1,1)
        /// </summary>
        /// <returns></returns>
        public Vector2 ToInternalRepresentation(Vector2 absPos)
        {
            Vector2 relPos = absPos - new Vector2(X, Y);


            // we are essentially doing a projection of (external rep.) (0,1) and (1,0) onto our top and left edges,
            // but instead of taking a vector, we're just taking the fraction of length of those vectors. this creates a 
            // matrix to convert from external to internal rep.
            Vector2 XVector = TopRight - TopLeft;
            Vector2 YVector = BottomLeft - TopLeft;
            Vector2 absXtoInternal = new Vector2(XVector.X/XVector.LengthSquared(), YVector.X/YVector.LengthSquared());
            Vector2 absYtoInternal = new Vector2(XVector.Y/XVector.LengthSquared(), YVector.Y/YVector.LengthSquared());

             

            // now just take the relPos and convert it!
            // relpos * [ absXtoInt, absYtoInt ] = Intpos
            // matrix multiplication, basically
            float x = absXtoInternal.X * relPos.X + absYtoInternal.X * relPos.Y;
            float y = absXtoInternal.Y * relPos.X + absYtoInternal.Y*relPos.Y;

            return new Vector2(x, y);

        }


        public Vector2 FromInternalRepresentation(Vector2 internalPos)
        {
            return PositionOfSizeRelativePoint(internalPos.X, internalPos.Y);   
        }


        private void SetPositionOfSizeRelativePoint(float x, float y, Vector2 newPos)
        {
            Vector2 rel = PositionOfSizeRelativePoint(x, y) - new Vector2(X, Y);
            newPos = newPos - rel;
            X = newPos.X;
            Y = newPos.Y;
        }


        public void SetBoundingBoxLocation(Vector2 newLoc)
        {
            Vector2 oldLoc = BoundingBox.Location;

            Vector2 diff = newLoc - oldLoc;

            X += diff.X;
            Y += diff.Y;

        }



        // And finally, the constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"> the physiscal location of the rotation origin of this rectangle.</param>
        /// <param name="size"> the width and height of this rectangle </param>  
        /// <param name="rotation">the angle, in radians clockwise, that this rectangle is rotated around from rotation origin. </param>
        /// <param name="rotationOrigin">The point on the rectangle that is rotated about, from 0,0 (top left) to 1,1 (bottom right) </param>
        public RotatedRect(Vector2 location, Vector2 size, float rotation, Vector2 rotationOrigin)
        {

            Rotation = MathHelper.WrapAngle(rotation);

            Width = size.X;

            Height = size.Y;

            // we need to get the location, now.


            // let's pretend this makes sense. Cool? thanks.
            rotationOrigin *= size;

            float OriginLocationDist = rotationOrigin.Length();

            float deltaTheta = Atan(rotationOrigin.Y / rotationOrigin.X) + Rotation;
            if (float.IsNaN(deltaTheta))
            {
                deltaTheta = 0; // shouldn't matter because originlocdist = 0
            }
            X = location.X - Cos(deltaTheta) * OriginLocationDist;
            Y = location.Y - Sin(deltaTheta) * OriginLocationDist;


        }

        public RotatedRect(RectangleF rect, float rotation, Vector2 origin) : this(rect.Location, rect.Size, rotation, origin)
        {
        }

        public RotatedRect(Rectangle rect, float rotation, Vector2 origin) : this(new RectangleF(rect), rotation, origin)
        {
        }

        public RotatedRect(RectangleF rect, Direction d) : this(rect, d.ToRadians(), new(.5f))
        {

        }

        public RotatedRect(Rectangle rect, Direction d) : this(rect, d.ToRadians(), new(.5f))
        {

        }

        public RotatedRect(RotatedRect other)
        {
            X = other.X;
            Y = other.Y;
            Width = other.Width;
            Height = other.Height;
            Rotation = other.Rotation;
        }



        // valid rotation values are between 0 and pi/2. MAKE IT SO.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BoundingLocation"></param>
        /// <param name="size">Size of interior rectangle</param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static RotatedRect FromBoundingLocation(Vector2 BoundingLocation, Vector2 size, float rotation)
        {

            rotation = MathHelper.WrapAngle(rotation);

            float w = size.X;
            float h = size.Y;


            Vector2 loc = rotation switch
            {

                // How does this math work?
                // I made a desmos graph for it
                // did a bunch of geometry on a sheet of paper
                // just trust it
                // unless it doesn't work
                // then idk, good luck I guess

                // down
                < -PI / 2f => new(BoundingLocation.X + w * Cos(rotation + PI),
                                        BoundingLocation.Y + w * Sin(rotation + PI) + h * Cos(rotation + PI)),

                // left
                < 0 => new(BoundingLocation.X,
                           BoundingLocation.Y + w * Sin(-rotation)),

                // up
                < PI / 2f => new(BoundingLocation.X + h * Sin(rotation),
                                       BoundingLocation.Y),

                // right
                _ => new(BoundingLocation.X + w * Cos(PI - rotation) + h * Sin(PI - rotation),
                         BoundingLocation.Y + h * Cos(PI - rotation)),
            };

            RotatedRect toReturn = new RotatedRect(loc, size, 0f, new(0));
            toReturn.Rotation = rotation;
            return toReturn;
        }

        public static RotatedRect FromBoundingLocation(Point BoundingLocation, Point size, float rotation)
        {
            return FromBoundingLocation(BoundingLocation.ToVector2(), size.ToVector2(), rotation);
        }


        /// <summary>
        /// Rotates the rectangle by a number of radians about the relative origin.
        /// </summary>
        /// <param name="radians"></param>
        /// <param name="origin">the relative origin of rotation, where (0,0) is the position of the rectangle and (1,1) is the furthest corner.</param>
        public void RotateBy(float radians, Vector2 origin)
        {
            // step 1 find actual origin
            Vector2 originPos = PositionOfSizeRelativePoint(origin.X, origin.Y);

            RotateAbout(radians, originPos);
        }

        public void RotateAbout(float radians, Vector2 AbsoluteOrigin)
        {

            // step 2 rotate position
            X -= AbsoluteOrigin.X;
            Y -= AbsoluteOrigin.Y;
            float newX = X * Cos(radians) - Y * Sin(radians);
            float newY = Y * Cos(radians) + X * Sin(radians);
            X = newX + AbsoluteOrigin.X;
            Y = newY + AbsoluteOrigin.Y;

            // step 3 resolve new rotation value

            // we measure the angle between our position, the origin and what we think is the origin to find our missing angle
            //Vector2 falseOriginPos = PositionOfSizeRelativePoint(origin.X, origin.Y);

            Rotation = MathHelper.WrapAngle(Rotation + radians);
        }

        public bool Contains(Vector2 point)
        {
            RectangleF rectangleF = AsRectangleF;

            float relX = (point.X - X);
            float relY = (point.Y - Y);

            float x = relX * Cos(Rotation) - relY * Sin(Rotation) + X;
            float y = relX * Sin(Rotation) + relY * Cos(Rotation) + Y;

            Vector2 toCheck = new(x, y);
            return rectangleF.Contains(toCheck);

        }

        public bool Intersects(RotatedRect other)
        {
            // the goal is to check whether our corners intersect other, then vice versa
            if (other.Contains(TopLeft)) { return true; }
            if (other.Contains(BottomRight)) { return true; }
            if (other.Contains(TopRight)) { return true; }
            if (other.Contains(BottomLeft)) { return true; }

            if (Contains(other.TopLeft)) { return true; }
            if (Contains(other.BottomRight)) { return true; }
            if (Contains(other.TopRight)) { return true; }
            if (Contains(other.BottomLeft)) { return true; }

            return false;
        }


    }
}
