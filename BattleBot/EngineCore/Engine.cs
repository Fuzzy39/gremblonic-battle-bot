using EngineCore.Rendering;
using EngineCore.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore
{
    public sealed class Engine
    {

        private readonly BatchRenderer renderer;
        private readonly List<Service> systems;
        private bool isInitialized;
        

        public BatchRenderer Renderer { get { return renderer; } }

        public Engine(GraphicsDevice graphics)
        {
            renderer = new ScaledRenderer(graphics, new(1600, 900), true);
            systems = [];
            isInitialized = false;

            new PixelRenderingService(this);
            new CameraRenderingService(this);
            new InputService(this);

        }

        internal void OnEntityChanged(Entity e) 
        { 
            if(!isInitialized)
            {
                throw new InvalidOperationException("Entities may not be created before engine initalization.");
            }


            foreach(Service s in systems)
            {
                s.OnEntityChanged(e);
            }
        }

        internal void OnEntityDestroyed(Entity e)
        {
            foreach(Service s in systems)
            {
                s.OnEntityDestroyed(e);
            }
        }

        public void OnSystemCreated(Service system)
        {
            if(isInitialized)
            {
                throw new InvalidOperationException("Systems may not be created after engine initialization.");
            }

            systems.Add(system);
        }

        public void Initialize() { isInitialized = true; }

        public void Update(GameTime time)
        {

            if (!isInitialized)
            {
                throw new InvalidOperationException("Can't update before initialization.");
            }

            foreach (Service s in systems)
            {
                s.Update(time);
            }

    
        }

        public void Draw(GameTime time)
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("Can't draw before initialization.");
            }

     
            foreach(Service s in systems)
            {
                s.PreDraw(time);
            }

           
            renderer.Begin();
            foreach (Service s in systems)
            {
                s.Draw(time);
            }

            renderer.End();
        }
    }
}
