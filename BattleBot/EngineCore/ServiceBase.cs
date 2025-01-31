using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore
{
    public abstract class ServiceBase : Service
    {
        public bool IsRunning { get; set; }
        protected Dictionary<EntityType, List<Entity>> entities;
      
        //protected List<EntityType> EntityTypes { get; private set; }

        /// <summary>
        /// Any class which implements ServiceBase should add lists to entities, for each EntityType that they care about.
        /// </summary>
        public ServiceBase(Engine e) 
        {
            IsRunning = true;
            entities = [];
            e.OnSystemCreated(this);

        }

        public void OnEntityChanged(Entity e)
        {

            foreach (KeyValuePair<EntityType, List<Entity>> entry in entities)
            {
                if(e.IsOfType(entry.Key))
                {
                    if(!entry.Value.Contains(e)) entry.Value.Add(e);
                    continue;
                }

                entry.Value.Remove(e);
            }
        }



        public void OnEntityDestroyed(Entity e)
        { 
            foreach(KeyValuePair<EntityType, List<Entity>> entry in entities)
            {
                entry.Value.Remove(e);
            }
        }


        ///// <summary>
        ///// Called when an entity looses a given type. Inhertiting classes should stop storing this entity.
        ///// </summary>
        ///// <param name="e">the entity who no longer has the type</param>
        ///// <param name="type">the type which was removed from the entity</param>
        //protected abstract void OnTypeRemoved(Entity e, EntityType type);

        ///// <summary>
        ///// Called when an entity gains a relevant type. inheriting classes should store this entity.
        ///// </summary>
        ///// <param name="e"></param>
        ///// <param name="type"></param>
        //protected abstract void OnTypeAdded(Entity e, EntityType type);


        public virtual void Update(GameTime gameTime) { }


        /// <summary>
        /// Predrawing is for drawing to render targets before rendering the final image. Do NOT draw any sprites or graphics to the window during a predraw call.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PreDraw(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }



    }
}
