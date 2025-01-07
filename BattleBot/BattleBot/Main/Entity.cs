using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Main
{
    internal class Entity
    {

        private List<Component> components;


        public Entity()
        {
            components = new();
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Type must be a component</param>
        /// <returns></returns>
        public IEnumerable<Component> findComponent(Type type)
        {
            Debug.Assert(type.IsSubclassOf(typeof(Component)));
            return components.Where( (Component comp) => { return comp.GetType().Equals(type); } );
        }

        public void Destroy()
        {
            foreach(Component c in components)
            {
                c.Destroy();
            }

        }
    }
}
