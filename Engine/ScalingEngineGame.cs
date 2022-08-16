using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    public class ScalingEngineGame : EngineGame
    {
        public Stage OverlayStage;
        protected override Stage TargetCursorStage() => this.OverlayStage;
        protected override void Initialize()
        {
            OverlayStage = new Stage(this);
            base.Initialize();
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            scene = new RenderTarget2D(GraphicsDevice, VirtualWidth, VirtualHeight, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
        }
        public new void UpdateWindowSize(int width, int height)
        {
            base.UpdateWindowSize(width, height);
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            scene = new RenderTarget2D(GraphicsDevice, VirtualWidth, VirtualHeight, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
        }
        protected override void LoadContent()
        {
            base.LoadContent();

            BackgroundTexture = ResourceMan.GetTexture(@"Engine.BuiltinAssets.graygrid");
        }
        private RenderTarget2D scene;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(scene);

            Stage.Draw(spriteBatch, this);

            BaseDraw(gameTime);

            float outputAspect = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
            float preferredAspect = VirtualWidth / (float)VirtualHeight;

            Rectangle dst;

            if (outputAspect <= preferredAspect)
            {
                // output is taller than it is wider, bars on top/bottom
                int presentHeight = (int)((Window.ClientBounds.Width / preferredAspect) + 0.5f);
                int barHeight = (Window.ClientBounds.Height - presentHeight) / 2;

                dst = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                int presentWidth = (int)((Window.ClientBounds.Height * preferredAspect) + 0.5f);
                int barWidth = (Window.ClientBounds.Width - presentWidth) / 2;

                dst = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
            }
            StageScreenPosition.X = dst.X;
            StageScreenPosition.Y = dst.Y;
            StageScreenSize.X = dst.Width;
            StageScreenSize.Y = dst.Height;
            var currentMouse = Mouse.GetState();
            ScaledMousePosition = ScreenToEngine(new Vector2(currentMouse.Position.X - dst.X, currentMouse.Position.Y - dst.Y));

            GraphicsDevice.SetRenderTarget(null);

            // clear to get black bars
            GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            spriteBatch.Begin(SpriteSortMode.BackToFront);
            // draw background texture
            DrawBackground();
            spriteBatch.End();
            // draw a quad to get the draw buffer to the back buffer
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp);
            // scaled output
            spriteBatch.Draw(scene, dst, Color.White);
            // overlay
            spriteBatch.End();
            OverlayStage.Draw(spriteBatch, this);
            ScaledMousePositionPrevious = ScaledMousePosition;
        }
        internal void DrawBackground()
        {
            if (BackgroundTexture == null)
                return;
            var srcRectangle = new Rectangle(0, 0, BackgroundTexture.Width, BackgroundTexture.Height);

            var position = Vector2.Zero;
            var scale = Vector2.One;
            if (BackgroundTextureDrawMode == TextureDrawMode.Stretch)
            {
                scale.X = (float)Window.ClientBounds.Width / srcRectangle.Width;
                scale.Y = (float)Window.ClientBounds.Height / srcRectangle.Height;
                position = Vector2.Zero;
                spriteBatch.Draw(BackgroundTexture, position, srcRectangle, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
            else if (BackgroundTextureDrawMode == TextureDrawMode.Fit)
            {
                scale.X = (float)Window.ClientBounds.Width / srcRectangle.Width;
                scale.Y = (float)Window.ClientBounds.Height / srcRectangle.Height;
                if (scale.X > scale.Y)
                    scale.Y = scale.X;
                if (scale.Y > scale.X)
                    scale.X = scale.Y;
                position = Vector2.Zero;
                spriteBatch.Draw(BackgroundTexture, position, srcRectangle, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
            else if (BackgroundTextureDrawMode == TextureDrawMode.Repeat)
            {
                double og_sx = (float)Window.ClientBounds.Width / srcRectangle.Width;
                double og_sy = (float)Window.ClientBounds.Height / srcRectangle.Height;

                double sy;
                double sx;
                if (og_sx > og_sy)
                {
                    sx = og_sx;
                    sy = og_sx;
                }
                else
                {
                    sx = og_sy;
                    sy = og_sy;
                }

                double repeatXCount = Math.Ceiling(sx);
                double repeatYCount = Math.Ceiling(sy);
                for (int x = 0; x < repeatXCount; x++)
                {
                    for (int y = 0; y < repeatYCount; y++)
                    {
                        Vector2 targetPosition = new Vector2((float)BackgroundTexture.Width * x, (float)BackgroundTexture.Height * y);
                        spriteBatch.Draw(
                            BackgroundTexture,
                            targetPosition,
                            srcRectangle,
                            Color.White,
                            0,
                            Vector2.Zero,
                            scale,
                            SpriteEffects.None,
                            0);
                    }
                }
            }
            else
            {
                spriteBatch.Draw(BackgroundTexture, position, srcRectangle, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
        }
        public TextureDrawMode BackgroundTextureDrawMode = TextureDrawMode.Repeat;
        public Texture2D? BackgroundTexture;
    }
}
