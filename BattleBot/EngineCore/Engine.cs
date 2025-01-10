using EngineCore.Rendering;
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

        private IBatchRenderer renderer;
        private List<System> systems;
        private bool isInitialized;
        

        public IBatchRenderer Renderer { get { return renderer; } }

        public Engine(GraphicsDevice graphics)
        {
            renderer = new ScaledRenderer(graphics, new(1600, 900), true);
            systems = new List<System>();
            isInitialized = false;
        }

        internal void OnEntityChanged(Entity e) 
        { 
            if(!isInitialized)
            {
                throw new InvalidOperationException("Entities may not be created before engine initalization.");
            }


            foreach(System s in systems)
            {
                s.onEntityChanged(e);
            }
        }

        internal void OnEntityDestroyed(Entity e)
        {
            foreach(System s in systems)
            {
                s.onEntityDestroyed(e);
            }
        }

        internal void OnSystemCreated(System system)
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
                s.update(time);
            }
        }

        public void Draw(GameTime time)
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("Can't draw before initialization.");
            }
            renderer.Begin();
          

            foreach (System s in systems)
            {
                s.draw(time);
            }

            renderer.End();
        }
    }
}
