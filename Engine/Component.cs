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
    }
}
