using FRAMEDRAG.Engine.Display;
using FRAMEDRAG.Engine.Textures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    internal class DVDSprite : Sprite
    {
        public DVDSprite(Texture texture) : base(texture)
        {
        }

        private Vector2 change = new Vector2(1f, 1f);
        public override void Update(GameTime gameTime, EngineGame engine)
        {
            if (Position.X <= 1)
                change.X = 1f;
            else if (Position.X + Width >= engine.graphicsDevice.PreferredBackBufferWidth)
                change.X = -1f;

            if (Position.Y <= 1)
                change.Y = 1f;
            else if (Position.Y + Height >= engine.graphicsDevice.PreferredBackBufferHeight)
                change.Y = -1f;

            Position += change;
        }
    }
}
