using EngineCore.Rendering;
using EngineCore.Systems;
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
        private readonly List<System> systems;
        private bool isInitialized;
        

        public BatchRenderer Renderer { get { return renderer; } }

        public Engine(GraphicsDevice graphics)
        {
            renderer = new ScaledRenderer(graphics, new(1600, 900), true);
            systems = [];
            isInitialized = false;

            new PixelRenderingSystem(this);
            new CameraRenderingSystem(this);
            new InputSystem(this);

        }

        internal void OnEntityChanged(Entity e) 
        { 
            if(!isInitialized)
            {
                throw new InvalidOperationException("Entities may not be created before engine initalization.");
            }


            foreach(System s in systems)
            {
                s.OnEntityChanged(e);
            }
        }

        internal void OnEntityDestroyed(Entity e)
        {
            foreach(System s in systems)
            {
                s.OnEntityDestroyed(e);
            }
        }

        public void OnSystemCreated(System system)
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

            foreach (System s in systems)
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

     
            foreach(System s in systems)
            {
                s.PreDraw(time);
            }

           
            renderer.Begin();
            foreach (System s in systems)
            {
                s.Draw(time);
            }

            renderer.End();
        }
    }
}
