using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    public class Component : GameComponent
    {
        public Component(EngineGame game)
            : base(game)
        {
            Game = game;
            Game.MouseDownEvent += MouseDown;
            Game.MouseUpEvent += MouseUp;
        }
        public new EngineGame Game;
        public virtual void Draw(GameTime gameTime)
        { }
        public virtual void FixedUpdate(GameTime gameTime)
        { }
        public virtual void FixedFastUpdate(GameTime gameTime)
        { }
        public virtual void DrawSprite(SpriteBatch spriteBatch)
        { }

        public virtual void MouseDown(Vector2 position, MouseButton button)
        { }
        public virtual void MouseUp(Vector2 position, MouseButton button)
        { }
    }
}
