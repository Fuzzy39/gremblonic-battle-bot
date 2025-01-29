using EngineCore.Util;
using EngineCore.Util.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EngineCore.Rendering
{
    /// <summary>
    /// A Basic Renderer accomplishes the tasks of a renderer with a spritebatch.
    /// No bells and whistles. Just adds a layer of abstraction above the spritebatch.
    /// </summary>
    internal class BasicRenderer : BatchRenderer
    {

        protected GraphicsDevice gd;
        protected SpriteBatch spriteBatch;


        private bool hasDrawntoPrimary = false;
        protected bool hasTarget = false;


        public Vector2 Size
        {
            get
            {
                return gd.Viewport.Bounds.Size.ToVector2();
            }
        }

        public BasicRenderer(GraphicsDevice gd)
        {
            this.gd = gd;
            spriteBatch = new SpriteBatch(gd);



        }

        public void Draw(Texture2D texture, RotatedRect destination, Rectangle source, Color color, byte depth)
        {

            spriteBatch.Draw(
                texture,
                destination.AsRectangle,
                source,
                color,
                destination.Rotation.Radians,
                new(0),
                SpriteEffects.None, 
                depth/255f
            );

            if (!hasTarget)
            {
                hasDrawntoPrimary = true;
            }

        }

        public void DrawString(FontFamily font, string text, Vector2 position, float height, Color color)
        {
            font.Draw(spriteBatch, text, height, position, color);
        }

        void BatchRenderer.Begin()
        {
            if (hasTarget)
            {
                throw new InvalidOperationException("Has a render target.");
            }
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
        }

        public void End()
        {
            if (hasTarget)
            {
                throw new InvalidOperationException("Has a render target.");
            }
            spriteBatch.End();
            hasDrawntoPrimary = false;
        }


        public RenderTarget2D CreateTarget(Point size)
        {
            return new RenderTarget2D(gd, size.X, size.Y);
        }

        public void StartTarget(RenderTarget2D target)
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
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            gd.Clear(Color.Transparent);
            hasTarget = true;

        }


        public void EndTarget()
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
