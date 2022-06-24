using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FRAMEDRAG.Engine.Loaders;

namespace FRAMEDRAG.Engine.Display
{
    public class AnimatedSprite : DisplayObject
    {
        public int CurrentFrame { get; private set; }
        public long CurrentTick { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsStopped { get; private set; }
        internal Texture2D[] TextureArray = Array.Empty<Texture2D>();

        public static AnimatedSprite GIFFromStream(Stream stream, EngineGame engine)
        {
            return new AnimatedSprite()
            {
                TextureArray = AnimatedGIF.ArrayFromStream(new BinaryReader(stream), engine)
            };
        }
        public static AnimatedSprite FromFrameArray(Texture2D[] textureArray)
        {
            return new AnimatedSprite()
            {
                TextureArray = textureArray
            };
        }

        public Texture2D GetTexture()
        {
            return TextureArray[CurrentFrame];
        }
        public Texture2D GetTexture(int frameIndex)
        {
            return TextureArray[frameIndex];
        }
        public void Pause()
        {
            IsPaused = false;
        }
        public void Play()
        {
            CurrentFrame = 0;
            IsPaused = false;
            IsStopped = false;
        }
        public void Resume()
        {
            IsPaused = false;
        }
        public void Stop()
        {
            CurrentFrame = 0;
            IsPaused = false;
            IsStopped = true;
        }
        public override string ToString()
        {
            return $@"Playing frame {CurrentFrame} of {TextureArray.Length} -- {100f * CurrentFrame / TextureArray.Length}%";
        }

        private Vector2? PreviousPosition;
        private Rectangle CachedRectangle = new Rectangle(0,0,0,0);
        public override void Draw(SpriteBatch spriteBatch, EngineGame engine)
        {
            if (Visible)
            {
                spriteBatch.Draw(
                    TextureArray[CurrentFrame],
                    CachedRectangle,
                    Color.White);
            }
        }
        public override void Update(GameTime gameTime, EngineGame engine)
        {
            if (PreviousPosition != null)
            {
                if (PreviousPosition?.X != Position.X)
                    CachedRectangle.X = Convert.ToInt32(Math.Round(Position.X));
                if (PreviousPosition?.Y != Position.Y)
                    CachedRectangle.Y = Convert.ToInt32(Math.Round(Position.Y));
                CachedRectangle.Width = TextureArray[CurrentFrame].Width;
                CachedRectangle.Height = TextureArray[CurrentFrame].Height;
            }
            PreviousPosition = Position;
        }
    }
}
