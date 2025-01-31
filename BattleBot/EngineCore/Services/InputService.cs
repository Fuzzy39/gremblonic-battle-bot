using EngineCore.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace EngineCore.Services
{
    internal class InputService : ServiceBase
    {


        // The Entity Type that this service cares about. More complex services may have seperate distinct types. Feel free to change the name.
        public static readonly EntityType EntityType = e => e.HasComponent<InputComponent>();


        public InputService(Engine e) : base(e)
        {
            // a service's entites are stored in a dictionary of entity lists, with one entry in the dictionary for each type of entity we need to manage.
            // this is useful for more complex services, like CameraRenderingService, which manages both CameraRenderable entities, as well as Camera entities.
            entities.Add(EntityType, []);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Entity entity in entities[EntityType])
            {
                InputComponent input = entity.FindComponent<InputComponent>()!;
                if (Keyboard.GetState().IsKeyDown(input.key))
                {
                    input.action.Invoke();
                }
            }
        }
    }
}
