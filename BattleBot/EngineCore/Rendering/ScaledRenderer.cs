using EngineCore.Util;
using EngineCore.Util.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore.Rendering
{


    internal class ScaledRenderer : BatchRenderer
    {

        protected GraphicsDevice gd;
        protected SpriteBatch spriteBatch;


        private bool hasDrawntoPrimary = false;
        private bool hasTarget = false;
        private bool doLetterboxing = false;


        private Point size;
        private RenderTarget2D virtualWindow;

        public Vector2 Size
        {
            get { return size.ToVector2(); }
        }

        private Point WindowSize
        {
            get { return gd.Viewport.Bounds.Size; }
        }

        /// <summary>
        /// The maximum size in real pixels of the renderable frame if the aspect ratio must be maintained.
        /// </summary>
        private Rectangle MaxUsableBounds
        {
            get
            {
                float virtualAspect = Size.X/ Size.Y;
                float realAspect = (float)WindowSize.X / WindowSize.Y;
                if (virtualAspect == realAspect) { return new( new(0,0), WindowSize); }

                float X, Y, width, height;
                if(realAspect >= virtualAspect)
                {
                    // the real aspect ratio is less tall/ wider than the virtual aspect.
                
                    height = WindowSize.Y;
                    width = virtualAspect * height;
                    X = (WindowSize.X - width) / 2.0f;
                    Y = 0;
                }
                else
                {
                    width = WindowSize.X;
                    height = (1/virtualAspect) * width;
                    Y = (WindowSize.Y - height) / 2.0f;
                    X = 0;
                }

                return new RectangleF(X, Y, width, height).toRectangle();

            }
        }

        /// <summary>
        /// Whether this ScaledRenderer should maintain it's aspect ratio (given by size) instead of stretching the rendered contents.
        /// Black bars will be rendered if true to maintain aspect ratio if the window's aspect ratio differs.
        /// </summary>
        public bool DoLetterboxing
        {
            get { return doLetterboxing; }
            set {  doLetterboxing = value; }
        }


        public ScaledRenderer(GraphicsDevice gd, Point size, bool doLetterboxing)
        {

            this.doLetterboxing = doLetterboxing;

            this.gd = gd;
            spriteBatch = new SpriteBatch(gd);

            this.size = size;
            virtualWindow = new RenderTarget2D(gd, size.X, size.Y);
            
        }

        public virtual void Draw(Texture2D texture, RotatedRect destination, Rectangle source, Color color, int depth)
        {




            spriteBatch.Draw(
                texture,
                destination.AsRectangle,
                source,
                color,
                destination.Rotation.Radians,
                new(0),
                SpriteEffects.None, depth
            );

            if (!hasTarget)
            {
                hasDrawntoPrimary = true;
            }

        }

        public virtual void DrawString(FontFamily font, string text, Vector2 position, float height, Color color)
        {
            font.Draw(spriteBatch, text, height, position, color);
        }

        public void Begin()
        {

         
            if (hasTarget)
            {
                throw new InvalidOperationException("Has a render target.");
            }
            gd.SetRenderTarget(virtualWindow);
            gd.Clear(Color.Black);
            spriteBatch.Begin();
        }

        public void End()
        {
            if (hasTarget)
            {
                throw new InvalidOperationException("Has a render target.");
            }
            spriteBatch.End();
            gd.SetRenderTarget(null);

            // render to the real screen (coward, you won't!)
            spriteBatch.Begin();
            Rectangle bounds = doLetterboxing ? MaxUsableBounds : new Rectangle(new(0, 0), WindowSize);
            spriteBatch.Draw(virtualWindow, bounds, Color.White);
            spriteBatch.End();


            hasDrawntoPrimary = false;

        }


        public virtual RenderTarget2D CreateTarget(Point size)
        {
            return new RenderTarget2D(gd, size.X, size.Y);
        }

        public virtual void StartTarget(RenderTarget2D target)
        {

            if (hasTarget)
            {
                throw new InvalidOperationException("Already has a target");
            }

            if (hasDrawntoPrimary)
            {
                throw new InvalidOperationException("A render target cannot be created after graphics has been drawn to the window.");
            }


            gd.SetRenderTarget(target);
            spriteBatch.Begin();
            gd.Clear(Color.Transparent);
            hasTarget = true;

        }


        public virtual void EndTarget()
        {
            if (!hasTarget)
            {
                throw new InvalidOperationException("A target has not begun.");
            }



            spriteBatch.End();
            gd.SetRenderTarget(null);
            hasTarget = false;



        }

    }
}
