using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine.Loaders
{
    public class AnimatedGIF
    {
        private int currentFrame = 0;
        private long currentTick = 0;
        private bool isPaused = false;
        private bool isStopped = false;
        private Texture2D[] textureArr = Array.Empty<Texture2D>();
        public static AnimatedGIF FromStream(Stream input, EngineGame engine)
        {
            return FromStream(new BinaryReader(input), engine);
        }
        public static AnimatedGIF FromStream(BinaryReader input, EngineGame engine)
        {
            int frameCount = input.ReadInt32();
            var frames = new List<Texture2D>();
            var graphicsDevice = engine.GraphicsDevice;
            for (int i = 0; i < frameCount; i++)
            {
                SurfaceFormat format = (SurfaceFormat)input.ReadInt32();
                int width = input.ReadInt32();
                int height = input.ReadInt32();
                int levelCount = input.ReadInt32();
                frames[i] = new Texture2D(graphicsDevice, width, height, false, format);
                for (int j = 0; j < levelCount; i++)
                {
                    int count = input.ReadInt32();
                    byte[] data = input.ReadBytes(count);
                    Rectangle? rect = null;
                    frames[i].SetData(j, rect, data, 0, data.Length);
                }
            }
            input.Close();
            return FromTextures(frames.ToArray());
        }
        public static AnimatedGIF FromTextures(Texture2D[] frames)
        {
            return new AnimatedGIF()
            {
                textureArr = frames
            };
        }

        public Texture2D GetTexture()
        {
            return textureArr[currentFrame];
        }
        public Texture2D GetTexture(int frameIndex)
        {
            return textureArr[frameIndex];
        }

        public void Pause()
        {
            isPaused = true;
        }
        public void Play()
        {
            currentFrame = 0;
            isPaused = false;
            isStopped = false;
        }
        public void Resume()
        {
            isPaused = false;
        }
        public void Stop()
        {
            currentFrame = 0;
            isPaused = false;
            isStopped = true;
        }

        public override string ToString()
        {
            return $@"Playing frame {currentFrame} of {textureArr.Length} -- {100f * currentFrame / textureArr.Length}%";
        }

        public void Update(long elapsedTicks)
        {
            if (!isPaused && !isStopped)
            {
                currentTick += elapsedTicks;
                if (currentTick >= 0xf4240L)
                {
                    currentTick = 0L;
                    currentFrame++;
                    if (currentFrame >= textureArr.Length)
                    {
                        currentFrame = 0;
                    }
                }
            }
        }

        public int CurrentFrame
        {
            get
            {
                return currentFrame;
            }
        }
        public int FrameCount
        {
            get
            {
                return textureArr.Length;
            }
        }
        public int Height
        {
            get
            {
                return textureArr[0].Height;
            }
        }
        public int Width
        {
            get
            {
                return textureArr[0].Width;
            }
        }
    }
}
