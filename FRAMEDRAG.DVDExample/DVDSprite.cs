using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FRAMEDRAG.Engine;
using FRAMEDRAG.Engine.Display;
using FRAMEDRAG.Engine.Textures;

namespace FRAMEDRAG.DVDExample
{
    internal class DVDSprite : Sprite
    {
        public DVDSprite(Texture texture) : base(texture)
        {
            Origin = Vector2.Zero;
        }

        private Vector2 change = new Vector2(1f, 1f);
        public override void Update(GameTime gameTime, EngineGame engine)
        {
            if (Position.X <= 0)
                change.X = 1f;
            else if (Position.X >= engine.InnerWindowSize.X)
                change.X = -1f;

            if (Position.Y <= 0)
                change.Y = 1f;
            else if (Position.Y >= engine.InnerWindowSize.Y)
                change.Y = -1f;

            Position += change;
        }
    }
}
