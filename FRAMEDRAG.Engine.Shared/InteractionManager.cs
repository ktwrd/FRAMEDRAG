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
        public override void Update(GameTime gameTime)
        {
            UpdateKeyboardState();
            UpdateMouseState();
        }
        private void UpdateKeyboardState()
        {
            var kstate = Keyboard.GetState();
            if (KeyboardState != null)
            {

            }
            KeyboardState = kstate;
        }
        private void UpdateMouseState()
        {
            var mstate = Mouse.GetState();
            if (MouseState != null)
            {

            }
            MouseState = mstate;
        }
    }
}
