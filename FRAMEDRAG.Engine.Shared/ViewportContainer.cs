using FRAMEDRAG.Engine.Display;
using FRAMEDRAG.Engine.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRAMEDRAG.Engine
{
    public class ViewportTransformState
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Scale = Vector2.One;
    }
    public class ViewportContainer : Component
    {
        public ViewportContainer(EngineGame engine) : base(engine)
        {
            engine.Stage.AddChild(Container);
        }

        public bool Pause = false;
        public bool Moving = false;
        public bool Zooming = false;

        public event VoidDelegate MovingEnd;
        public event VoidDelegate ZoomingEnd;
        public event VoidDelegate FrameEnd;

        public ViewportTransformState LastViewport = new ViewportTransformState();
        public Container Container = new Container();

        public int ScreenWidth => Game.graphicsDevice.PreferredBackBufferWidth;
        public int ScreenHeight => Game.graphicsDevice.PreferredBackBufferHeight;
        public Vector2 ScreenSize => new Vector2(Game.graphicsDevice.PreferredBackBufferWidth, Game.graphicsDevice.PreferredBackBufferHeight);

        public override void Update(GameTime gameTime)
        {
            if (!Pause)
            {
                if (LastViewport.Position != Container.Position)
                {
                    Moving = true;
                }
                else if (Moving)
                {
                    MovingEnd?.Invoke();
                    Moving = false;
                }

                if (LastViewport.Scale != Container.Scale)
                {
                    Zooming = true;
                }
                else if (Zooming)
                {
                    ZoomingEnd?.Invoke();
                    Zooming = false;
                }
            }

            LastViewport = new ViewportTransformState()
            {
                Position = Container.Position,
                Scale = Container.Scale
            };
            FrameEnd?.Invoke();

            base.Update(gameTime);
        }

        public Vector2 ToScreen(Vector2 position) => Container.ToGlobal(position);
        public double WorldWidth => Container.GetBounds().Width / Container.Scale.X;
        public double WorldHeight => Container.GetBounds().Height / Container.Scale.Y;

        public Vector2 ToWorld(Vector2 pos) => Container.GlobalToLocal(pos);

        public double WorldScreenWidth => WorldWidth * Container.Scale.X;
        public double WorldScreenHeight => WorldHeight * Container.Scale.Y;
        public Vector2 WorldScreen => new Vector2(Convert.ToSingle(WorldScreenWidth), Convert.ToSingle(WorldScreenHeight));
        public Vector2 CenterPosition => new Vector2(
            Convert.ToSingle((WorldScreenWidth / 2f) - (Container.Position.X / Container.Scale.X)),
            Convert.ToSingle((WorldScreenHeight / 2f) - (Container.Position.Y / Container.Scale.Y)));

        public void MoveCenter(Vector2 position)
        {
            var newPosition = ((WorldScreen / 2f) - position) * Container.Scale;

            if (Container.Position != newPosition)
            {
                Container.Position = newPosition;
            }
        }
        public void MoveCorner(Vector2 position)
        {
            var newPosition = position * Container.Scale;
            if (Container.Position != newPosition)
            {
                Container.Position = newPosition;
            }
        }

        public double ScreenWidthInWorldPixels => ScreenWidth / Container.Scale.X;
        public double ScreenHeightInWorldPixels => ScreenHeight / Container.Scale.Y;
        public Vector2 ScreenInWorldPixels => new Vector2(
            (float)ScreenWidthInWorldPixels,
            (float)ScreenHeightInWorldPixels);

        public double FindFitWidth(double width) => ScreenWidth / width;
        public double FindFitHeight(double height) => ScreenHeight / height;
        public double FindFitVec(Vector2 pos)
        {
            var res = ScreenSize / pos;
            return Math.Min(res.X, res.Y);
        }
        public double FindConverVec(Vector2 pos)
        {
            var res = ScreenSize / pos;
            return Math.Max(res.X, res.Y);
        }
    }
}
