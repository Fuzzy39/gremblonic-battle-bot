using BattleBot.Components;
using EngineCore;
using EngineCore.Rendering;
using EngineCore.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Systems
{
    internal class CameraRenderingSystem : EngineCore.System
    {

        /// <summary>
        /// Abstracts a Camera Entity.
        /// </summary>
        private class Camera
        {

            private Entity entity;

            public Entity Entity { get { return entity; } }

            public Camera(Entity e)
            {
                entity = e;
                if (!e.HasComponent<CameraComponent>() || !e.HasComponent<PixelBounds>())
                {
                    throw new ArgumentException("This Entity " + e.ToString() + " Cannot be used as a camera.");
                }
            }

            public static bool IsCameraEntity(Entity e)
            {
                return e.HasComponent<CameraComponent>() && e.HasComponent<PixelBounds>();

            }

            public RotatedRect WorldBounds
            {
                get
                {
                    RotatedRect toReturn = new RotatedRect(
                        new Vector2(0, 0),
                        PixelBounds.Size / Component.Scale,
                        -Component.Rotation,
                        new(.5f, .5f)
                    );

                    toReturn.Center = Component.Position;

                    return toReturn;
                }
            }

            public CameraComponent Component
            {
                get
                {
                    return entity.FindComponent<CameraComponent>();
                }
            }

            public RotatedRect PixelBounds
            {
                get
                {
                    return entity.FindComponent<PixelBounds>().Bounds;
                }

            }

            public RotatedRect ToPixelBounds(RotatedRect objectBounds)
            {
                Vector2 size = objectBounds.Size * Component.Scale;
                Angle rotation = Component.Rotation + objectBounds.Rotation;


                RotatedRect RenderBounds = new RotatedRect(new RectangleF(0, 0, PixelBounds.Width, PixelBounds.Height), Angle.FromRadians(0), new());

                // a glorious transformation. Hopefully it works.
                Vector2 internalRep = WorldBounds.ToInternalRepresentation(objectBounds.TopLeft);

                Vector2 pos = RenderBounds.FromInternalRepresentation(internalRep);

                // oh god hopefully this doesn't explode.
                return new(pos, size, rotation, new());


            }


            public void Render(Renderer renderer, Entity renderable)
            {
                // we must convert world bou
                RotatedRect renderableWorldBounds = renderable.FindComponent<WorldBounds>().Bounds;
                RotatedRect spriteBounds = ToPixelBounds(renderableWorldBounds);

                SimpleTexture st = renderable.FindComponent<SimpleTexture>();
                renderer.Draw(st.Texture, spriteBounds, st.Tint, st.Depth);
            }

        }

        private BatchRenderer renderer;

        private List<Camera> cameras;
        private List<Entity> renderables;


        public bool IsRunning { get; set; }

        public CameraRenderingSystem(Engine e)
        {
            // initalize members
            renderer = e.Renderer;
            cameras = new List<Camera>();
            renderables = new();

            // all systems should make this call!
            e.OnSystemCreated(this);
        }

        

        public void OnEntityChanged(Entity e)
        {
            // camera
            if (Camera.IsCameraEntity(e))
            {
                if (cameras.Any(cam => cam.Entity == e)) return;
                
                // add the new camera
                Camera cam = new Camera(e);
                cameras.Add(cam);
                return;
            }

            cameras.RemoveAll(cam => cam.Entity == e);

            // renderable
            if (isRenderable(e))
            {
                if(!renderables.Contains(e)) renderables.Add(e);
                return;
            }

            renderables.Remove(e);
        }

        public void OnEntityDestroyed(Entity e)
        {
            if (Camera.IsCameraEntity(e))
            {
                cameras.RemoveAll(cam => cam.Entity == e);
                return;
            }

            renderables.Remove(e);
        }

        private bool isRenderable(Entity e)
        {
            return e.HasComponent<WorldBounds>() && e.HasComponent<SimpleTexture>();
        }



        public void Update(GameTime time) { }

        public void PreDraw(GameTime gameTime)
        {
            // save textures to draw later. we have to operate on a seperate render target for each camera, after all.
            foreach (Camera cam in cameras)
            {
                RenderCamera(cam);
            }

        }

        public void Draw(GameTime gameTime)
        {
            // actually draw results to the screen
            foreach (Camera cam in cameras)
            {
                renderer.Draw(cam.Component.RenderTarget, cam.PixelBounds, Color.White, cam.Component.Depth);
            }

        }

        private void RenderCamera(Camera camera)
        {

            // create a target to render to.
            RenderTarget2D target = camera.Component.RenderTarget;

            // if the target does not exist or the camera's changed size, we need to make a new target
            if (target == null || target.Bounds.Size != camera.PixelBounds.Size.ToPoint())
            {
                target = renderer.CreateTarget(camera.PixelBounds.Size.ToPoint());
                camera.Component.RenderTarget = target;
            }

            renderer.StartTarget(target);

            // render to the target
            foreach (Entity toRender in renderables)
            {
                RotatedRect bounds = toRender.FindComponent<WorldBounds>().Bounds;
                //if (camera.WorldBounds.Intersects(bounds))
                {   
                    camera.Render(renderer, toRender);
                }
            }

            renderer.EndTarget();


        }


    }
}
