using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Display
{
    public class Sprite : Container
    {
        public Sprite(Textures.Texture texture)
        {
            Texture = texture;
            UpdateFrame = true;
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
    }
}
