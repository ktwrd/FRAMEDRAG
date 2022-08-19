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


    }
}
