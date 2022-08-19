using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRAMEDRAG.Engine.Helpers
{
    public delegate void Vector2Delegate(Vector2 vec);
    public delegate void VoidDelegate();

    public delegate void MouseDelegate(Vector2 position, MouseButton button);
    public delegate void MouseStateDelegate(MouseState state);
    public delegate void MousePositionStateDelegate(MouseState state, Vector2 enginePosition);

    public delegate void KeyboardStateDelegate(KeyboardState state, Keys[] keys);

    public delegate void CompareDelegate<T, U>(T current, U previous);
}
