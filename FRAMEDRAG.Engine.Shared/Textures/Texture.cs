using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine.Textures
{
    public class Texture
    {
        public Texture(Texture2D texture)
        {
            Rectangle frame = new Rectangle(0, 0, 1, 1);
            BaseTexture = texture;

            if (noFrame)
                frame = new Rectangle(0, 0, texture.Width, texture.Height);
            SetFrame(frame);
        }

        public bool noFrame = true;
        public Texture2D BaseTexture;
        public Rectangle Frame;
        public Vector2 Trim;
        public Texture Scope;

        public bool UpdateFrame = false;

        public int Width = 1;
        public int Height = 1;

        public void SetFrame(Rectangle frame)
        {
            Frame = frame;
            Width = frame.Width;
            Height = frame.Height;

            if (frame.X + frame.Width > BaseTexture.Width || frame.Y + frame.Height > BaseTexture.Height)
                throw new Exception(@"Texture Error: Frame does not fit inside of the base texture dimensions");
            UpdateFrame = true;
        }
    }
}
