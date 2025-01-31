using EngineCore.Components;
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

namespace EngineCore.Services
{
    internal class CameraRenderingService : Service
    {

        public static readonly EntityType CameraRenderable = e => e.HasComponent<MapBounds>() && e.HasComponent<SimpleTexture>();
        public static readonly EntityType Chunk = e => e.HasComponent<MapBounds>() && e.HasComponent<ChunkData>();


        private readonly BatchRenderer renderer;

        private readonly List<Camera> cameras;
        private readonly Dictionary<int, List<Entity>> renderables;


        public bool IsRunning { get; set; }

        public CameraRenderingService(Engine e)
        {
            // initalize members
            renderer = e.Renderer;
            cameras = []; // huh, neat, this calls the constructor.
            renderables = [];

            // all systems should make this call!
            e.OnSystemCreated(this);
        }



        public void OnEntityChanged(Entity e)
        {
            // camera
            if (e.IsOfType(Camera.EntityType))
            {
                if (cameras.Any(cam => cam.Entity == e)) return;

                // add the new camera
                Camera cam = new(e);
                cameras.Add(cam);
                if (!renderables.ContainsKey(cam.Component.MapID))
                { 
                    renderables.Add(cam.Component.MapID, []);
                }
                return;
            }

            cameras.RemoveAll(cam => cam.Entity == e);

            if (!e.HasComponent<MapBounds>())
            {
                // not renderable.
                return;
            }

            int mapID = e.FindComponent<MapBounds>()!.MapID;

            // renderable
            if (e.IsOfType(CameraRenderable) || e.IsOfType(Chunk))
            {
                if (!renderables.ContainsKey(mapID)) { renderables.Add(mapID, []); }
                if (!renderables[mapID].Contains(e)) renderables[mapID].Add(e);
                return;
            }

            if (renderables.ContainsKey(mapID))
            {
                renderables[e.FindComponent<MapBounds>()!.MapID].Remove(e);
            }
            
        }

        public void OnEntityDestroyed(Entity e)
        {
            if (e.IsOfType(Camera.EntityType))
            {
                cameras.RemoveAll(cam => cam.Entity == e);
                return;
            }

            renderables[e.FindComponent<MapBounds>()!.MapID].Remove(e);
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
                // RenderTarget should not be null here since PreDraw should instantiate it
                renderer.Draw(cam.Component.RenderTarget!, cam.PixelBounds, Color.White, cam.Component.Priority);
            }

        }

        private void RenderCamera(Camera camera)
        {

            // create a target to render to.
            RenderTarget2D? target = camera.Component.RenderTarget;

            // if the target does not exist or the camera's changed size, we need to make a new target
            if (target == null || target.Bounds.Size != camera.PixelBounds.Size.ToPoint())
            {
                target = renderer.CreateTarget(camera.PixelBounds.Size.ToPoint());
                camera.Component.RenderTarget = target;
            }

            renderer.StartTarget(target);

            // render to the target
            foreach (Entity toRender in renderables[camera.Component.MapID])
            {
                RotatedRect bounds = toRender.FindComponent<MapBounds>()!.Bounds;
               
                if (camera.WorldBounds.Intersects(bounds))
                {
                    if (toRender.IsOfType(CameraRenderable))
                    {
                        camera.Render(renderer, toRender);
                        continue;
                    }

                    camera.RenderChunk(renderer, toRender);
                }
            }

            renderer.EndTarget();


        }


    }
}
