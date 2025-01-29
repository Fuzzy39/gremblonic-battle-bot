using Microsoft.Xna.Framework.Input;
using System;

namespace EngineCore.Components
{
    public class InputComponent : Component
    {
        required public Action action;
        required public Keys key;
    }
}
