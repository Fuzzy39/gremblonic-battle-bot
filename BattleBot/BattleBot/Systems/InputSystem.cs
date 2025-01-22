
using BattleBot.Components;
using EngineCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace BattleBot.Systems
{
    internal class InputSystem : EngineCore.System
    {
        public InputSystem(Engine e) : base(e)
        {
            AddRequiredComponent(typeof(InputComponent));
            Initialize(e);
        }

        protected override void Draw(GameTime gameTime)
        {
            //No draw (angy face)
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (Entity entity in entities)
            {
                InputComponent input = (InputComponent)entity.FindComponent<InputComponent>();
                if (Keyboard.GetState().IsKeyDown(input.key))
                {
                    input.action.Invoke();
                }
            }
        }
    }
}
