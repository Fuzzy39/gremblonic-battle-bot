using EngineCore.Util;
using EngineCore.Util.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineCore.Rendering
{


    /// <summary>
    /// An IRenderer represents an object that draws textures.
    /// </summary>
    public interface Renderer
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="destinationRectangle"></param>
        /// <param name="color"></param>
        /// <param name="rotation">rotation around center of destination in radians</param>
        /// <param name="priority">higher priority draw calls will be rendered on top of lower priorities. Default is 0.</param>
        /// 
        public void Draw(Texture2D texture, RotatedRect position, Rectangle sourceRect, Color color, byte priority);

        public void Draw(Texture2D texture, RotatedRect position, Rectangle sourceRect, Color color)
        {
            Draw(texture, position, new(new(), new(texture.Width, texture.Height)), color, 0);
        }

        public void Draw(Texture2D texture, RotatedRect position, Color color, byte depth)
        {
            Draw(texture, position, new(new(), new(texture.Width, texture.Height)), color, depth);
        }

        public void Draw(Texture2D texture, RotatedRect position, Color color)
        {
            Draw(texture, position, new(new(), new(texture.Width, texture.Height)), color);
        }

        public void Draw(Texture2D texture, RectangleF position, Color color, byte depth)
        {
            Draw(texture, new RotatedRect(position.Location, position.Size, Angle.FromRadians(0), new(0)), color, depth);
        }

        public void Draw(Texture2D texture, RectangleF position, Color color)
        {
            Draw(texture, new RotatedRect(position.Location, position.Size, Angle.FromRadians(0), new(0)), color);
        }

        public void Draw(Texture2D texture, Rectangle position, Color color, byte depth)
        {
            Draw(texture, new RectangleF(position), color, depth);
        }

        public void Draw(Texture2D texture, Rectangle position, Color color)
        {
            Draw(texture, new RectangleF(position), color);
        }

        public void Draw(Texture2D texture, RectangleF position, Direction d, Color color, byte depth)
        {
            Draw(texture, new RotatedRect(position.Location, position.Size, d.ToAngle(), new(.5f)), color, depth);
        }


        // primary method



        /// <summary>
        /// 
        /// </summary>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="height">The height of a line of text.</param>
        /// <param name="color"></param>
        /// 
        public void DrawString(FontFamily font, string text, Vector2 position, float height, Color color);

        public void DrawString(FontFamily font, string text, Point position, float height, Color color)
        {
            DrawString(font, text, position.ToVector2(), height, color);
        }

        

    }
}
