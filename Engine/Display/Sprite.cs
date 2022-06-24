using FRAMEDRAG.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine.Display
{
    public class Sprite : Container
    {
        public Sprite(Textures.Texture texture)
        {
            Texture = texture;
            UpdateFrame = true;
        }


        public FRectangle LocalBoundsRect;
        public Bounds LocalBounds;
        public virtual FRectangle GetBounds(FRectangle? rect)
        {
            if (Children.Count < 1)
            {
                if (LocalBounds == null)
                    LocalBounds = new Bounds();
                LocalBounds.minX = Texture.BaseTexture.Width * -Anchor.X;
                LocalBounds.minY = Texture.BaseTexture.Height * -Anchor.Y;
                LocalBounds.maxX = Texture.BaseTexture.Width * (1 - Anchor.X);
                LocalBounds.maxY = Texture.BaseTexture.Height * (1 - Anchor.Y);
                if (rect == null)
                {
                    if (LocalBoundsRect == null)
                        LocalBoundsRect = new FRectangle();
                    rect = LocalBoundsRect;
                }
            }
            return base.getLocalBounds(rect);
        }

        public bool UpdateFrame = false;
        public Vector2 Anchor;
        public Engine.Textures.Texture Texture;

        private float width = 0f;
        public float Width
        {
            get
            {
                return Scale.X * Texture.Frame.Width;
            }
            set
            {
                Scale.X = value / Texture.Frame.Width;
                width = value;
            }
        }
        private float height = 0f;
        public float Height
        {
            get
            {
                return Scale.Y * Texture.Frame.Height;
            }
            set
            {
                Scale.Y = value / Texture.Frame.Height;
                height = value;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, EngineGame engine)
        {
            var rect = new Rectangle(
                    Convert.ToInt32(Position.X),
                    Convert.ToInt32(Position.Y),
                    Convert.ToInt32(Width),
                    Convert.ToInt32(Height));
            spriteBatch.Draw(
                Texture.BaseTexture,
                rect,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                Convert.ToSingle(ZIndex/int.MaxValue));
            base.Draw(spriteBatch, engine);
        }
    }
}
