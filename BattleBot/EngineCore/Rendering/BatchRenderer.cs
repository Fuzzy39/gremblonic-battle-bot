using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineCore.Rendering
{
    public interface BatchRenderer : Renderer
    {

        public Vector2 Size
        {
            get;
        }

        public float Width
        {
            get
            {
                return Size.X;
            }
        }

        public float Height
        {
            get
            {
                return Size.Y;
            }
        }




        public void Begin();
        public void End();


        /// <summary>
        /// Creates a new render target, but does not start drawing to it.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public RenderTarget2D CreateTarget(Point size);

        /// <summary>
        /// All future draw calls will be drawn to the givenRenderTarget
        /// </summary>
        /// <param name="target"></param>
        public void StartTarget(RenderTarget2D target);

        /// <summary>
        /// Stops Drawing to a RenderTarget if one was being drawn to.
        /// </summary>
        public void EndTarget();

    }
}
