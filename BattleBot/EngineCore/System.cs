using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore
{
    public abstract class System
    {

        private bool isRunning;
        private bool isInitalized; 

        /// <summary>
        /// The types of components needed for the system to be able to interact with an entity.
        /// </summary>
        private List<Type> requiredComponents;

        /// <summary>
        /// A Component Type, which, if present in an entity, should always indicate this system be active on the entity.
        /// If an Entity has this component but otherwise fails the requirements for the system to run on it, an exception will be thrown.
        /// </summary>
        private Type? primaryComponent;

        protected List<Entity> entities;
       

        public bool IsRunning
        { 
            get { return isRunning; }
            set 
            {
                if (!isInitalized) 
                { 
                    throw new InvalidOperationException("System must be initialized before starting. Call initialize(Engine e).");
                }
                    
                isRunning = value; 
            }
        }
        
        protected Type? PrimaryComponent
        {
            get { return primaryComponent; }
            set { 
                if (isInitalized) throw new InvalidOperationException("RequiredComponents may not be modified after initalization"); 

                if(value!=null && !value.IsAssignableTo(typeof(Component)))
                {
                    throw new InvalidOperationException("System '" + this.GetType().FullName + "': Primary component '"
              + value.FullName + "' does not derrive from " + typeof(Component).FullName + ".");
                }

                primaryComponent = value;
            }

        }



        public System(Engine e) 
        {
            isInitalized = false;
            isRunning = true;
            entities = new();
            requiredComponents = new();
        }

        protected void AddRequiredComponent(Type t)
        {
            if(isInitalized)
            {
                throw new InvalidOperationException("RequiredComponents may not be modified after initalization");
            }

            if(!t.IsAssignableTo(typeof(Component)))
            {
                throw new InvalidOperationException("System '" + this.GetType().FullName + "': Required component '"
                + t.FullName + "' does not derrive from " + typeof(Component).FullName + ".");
            }

            requiredComponents.Add(t);
        }

        protected void AddRequiredComponents(List<Type> types)
        {

            if (isInitalized)
            {
                throw new InvalidOperationException("RequiredComponents may not be modified after iniitalization");
            }

            foreach (Type t in types)
            {
                if (!t.IsAssignableTo(typeof(Component)))
                {
                    throw new InvalidOperationException("System '" + this.GetType().FullName + "': Required component '"
                    + t.FullName + "' does not derrive from " + typeof(Component).FullName + ".");
                }
            }

            requiredComponents.AddRange(types);
        }


        protected void Initialize(Engine e)
        {
            if (requiredComponents.Count == 0)
            {
                throw new InvalidOperationException("System '"+ this.GetType().FullName+"': RequiredComponents is null or empty.");
            }


            if(primaryComponent !=null && !requiredComponents.Contains(primaryComponent))
            {
                throw new InvalidOperationException("System '" + this.GetType().FullName + "': primaryComponent '"
                        + primaryComponent.FullName + "' is not in requiredComponents.");
            }

            e.OnSystemCreated(this);
            isInitalized = true;
            isRunning = true;
        }


       
        internal protected abstract void Update(GameTime gameTime);

        /// <summary>
        /// Predrawing is for drawing to render targets before rendering the final image. Do NOT draw any sprites or graphics to the window during a predraw call.
        /// </summary>
        /// <param name="gameTime"></param>
        internal protected virtual void PreDraw(GameTime gameTime) { }

        internal protected abstract void Draw(GameTime gameTime);
        

        internal void OnEntityChanged(Entity e)
        {
            if(EntityMeetsRequirements(e))
            {
                if (!entities.Contains(e))
                {
                    entities.Add(e);
                }
                return;
            }

            entities.Remove(e);
        }

        /// <summary>
        /// Returns whether an entity should be iterated over by this system.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private bool EntityMeetsRequirements(Entity e)
        {
            bool toReturn = true;
            foreach(Type component in requiredComponents)
            {
                if (!e.HasComponent(component))
                {
                    toReturn = false;
                    break;
                }
            }

            if(!toReturn && primaryComponent!=null && e.HasComponent(primaryComponent))
            {
                throw new InvalidOperationException("Entity " + e.ToString() + " has primary component " + primaryComponent.Name +
                    ", but is missing other requirements for this system. Did you forget to add them?");
            }

            return toReturn;
           
        }


        internal void OnEntityDestroyed(Entity e)
        {

            entities.Remove(e);
            
        }
    }
}
