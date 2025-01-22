using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore
{
    public sealed class Entity
    {

        private List<Component> components;
        private Engine engine;
        private bool isEditing;


        public Entity(Engine engine)
        {
            components = new();
            this.engine = engine;
            isEditing = true;
            
        }

        /// <summary>
        /// Call this before adding multiple components to an entity.
        /// </summary>
        public void StartEditing() 
        {
            isEditing = true;
        }

        /// <summary>
        /// This method should be called after an entity is created and its components are added,
        /// or when you are done adding components to an entity afte startEditing has been called.
        /// </summary>
        public void StopEditing()
        {
            isEditing = false;
            engine.OnEntityChanged(this);
        }

        public void AddComponent(Component component)
        {
            if(HasComponent(component.GetType()))
            {
                throw new InvalidOperationException("Entity: " + this + " Already has a component of type '" + component.GetType().Name+"'.");
            }

            components.Add(component);
            if(!isEditing) engine.OnEntityChanged(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Type must be a component</param>
        /// <returns></returns>
        public T? FindComponent<T>() where T : Component 
        {
            // previously a debug assert was here, but given this is part of the engine interfacing with user code, we ought to throw an exception instead.

            IEnumerable<Component> list = components.Where((comp) => { return comp is T; });

            if (list.Count() == 0) return default(T);
            return (T)list.First();
        }


        public bool HasComponent<T>() where T: Component
        { 
            return FindComponent<T>() != null;
        }


        internal Component? FindComponent(Type component)
        {

            Debug.Assert(component.IsAssignableTo(typeof(Component)));

            IEnumerable<Component> list = components.Where((comp) => { return comp.GetType().IsAssignableTo(component); });

            if (list.Count() == 0) return null;
            return list.First();
        }


        internal bool HasComponent(Type component)
        {
            return FindComponent(component) != null;
        }

        public override string ToString()
        {
            if(components.Count == 0)
            {
                return "{}";
            }
            string toReturn = "{";
            foreach (Component component in components)
            {
                toReturn += component.GetType().Name+" ,";
            }
            toReturn = toReturn.Substring(0, toReturn.Length-2);
            return toReturn + "}";
        }

        public void RemoveComponent(Component component)
        {
            if(!components.Contains(component))
            {
                throw new ArgumentException("Component is not a member of this Entity", "component");
            }

            components.Remove(component);
            // this gets called regardless of whether you're editing the entity, since system updates would cause crashes expecting the component to be there.
            // We could make a copy list when editing of components to resolve this, don't know if it will come up though.
            engine.OnEntityChanged(this);
        }

        public void Destroy()
        {
            engine.OnEntityDestroyed(this);
            components.Clear(); // probably not neccesarry but whatever.
        }
    }
}
