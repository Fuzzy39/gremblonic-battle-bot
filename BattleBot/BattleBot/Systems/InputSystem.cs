﻿
using BattleBot.Components;
using EngineCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace BattleBot.Systems
{
    internal class InputSystem : EngineCore.BasicSystem
    {
        public InputSystem(Engine e) : base(e)
        {
            AddRequiredComponent(typeof(InputComponent));
            Initialize(e);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Entity entity in entities)
            {
                InputComponent input = entity.FindComponent<InputComponent>();
                if (Keyboard.GetState().IsKeyDown(input.key))
                {
                    input.action.Invoke();
                }
            }
        }
    }
}
