using FRAMEDRAG.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    public class InteractionManager : Component
    {
        private EngineGame Engine;
        public InteractionManager(EngineGame engine)
            : base(engine)
        {
            Engine = engine;
        }

        public KeyboardState? KeyboardState;
        public MouseState? MouseState;
        public override void FixedFastUpdate(GameTime gameTime)
        {
            var tarr = new Task[]
            {
                new Task(delegate{UpdateKeyboardState(); }),
                new Task(delegate{UpdateMouseState(); }),
            };
            foreach (var i in tarr)
                i.Start();
            Task.WhenAll(tarr).Wait();
            base.FixedFastUpdate(gameTime);
        }

        public new event MouseStateDelegate MouseUp;
        public new event MouseStateDelegate MouseDown;
        public event MouseStateDelegate MousePress;
        public event KeyboardStateDelegate KeyUp;
        public event KeyboardStateDelegate KeyDown;

        private void UpdateKeyboardState()
        {
            var kstate = Keyboard.GetState();
            if (KeyboardState != null)
            {
                var pressed = kstate.GetPressedKeys();
                KeyDown?.Invoke(kstate, pressed);
                Keys[] missingKeys = KeyboardState?.GetPressedKeys().Where((v) => !pressed.Contains(v)).ToArray() ?? Array.Empty<Keys>();
                if (missingKeys.Length > 0)
                    KeyUp?.Invoke(kstate, missingKeys);
            }
            KeyboardState = kstate;
        }
        private void UpdateMouseState()
        {
            var mstate = Mouse.GetState();
            if (MouseState != null)
            {
                bool InvokedUp = false;
                bool InvokedDown = false;
                bool InvokedPress = false;
                if (mstate.LeftButton == ButtonState.Released && MouseState?.LeftButton == ButtonState.Pressed && !InvokedUp)
                {
                    MouseUp?.Invoke(mstate);
                    InvokedUp = true;
                }
                if (mstate.LeftButton == ButtonState.Pressed)
                {
                    if (MouseState?.LeftButton == ButtonState.Released && !InvokedPress)
                    {
                        MousePress?.Invoke(mstate);
                        InvokedPress = true;
                    }
                    if (!InvokedDown)
                    {
                        MouseDown?.Invoke(mstate);
                        InvokedDown = true;
                    }
                }
                if (mstate.MiddleButton == ButtonState.Released && MouseState?.MiddleButton == ButtonState.Pressed && !InvokedUp)
                {
                    MouseUp?.Invoke(mstate);
                    InvokedUp = true;
                }
                if (mstate.MiddleButton == ButtonState.Pressed)
                {
                    if (MouseState?.MiddleButton == ButtonState.Released && !InvokedPress)
                    {
                        MousePress?.Invoke(mstate);
                        InvokedPress = true;
                    }
                    if (!InvokedDown)
                    {
                        MouseDown?.Invoke(mstate);
                        InvokedDown = true;
                    }
                }
                if (mstate.RightButton == ButtonState.Released && MouseState?.RightButton == ButtonState.Pressed && !InvokedUp)
                {
                    MouseUp?.Invoke(mstate);
                    InvokedUp = true;
                }
                if (mstate.RightButton == ButtonState.Pressed)
                {
                    if (MouseState?.RightButton == ButtonState.Released && !InvokedPress)
                    {
                        MousePress?.Invoke(mstate);
                        InvokedPress = true;
                    }
                    if (!InvokedDown)
                    {
                        MouseDown?.Invoke(mstate);
                        InvokedDown = true;
                    }
                }
            }
            MouseState = mstate;
        }
    }
}
