using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    internal class OnScreenDisplay : Component
    {
        static Vector2 padding = new Vector2(0, 0);
        const float width = 700;

        static Color bg_black = new Color(0, 0, 0, 180);
        static Color bg_new = new Color(50, 176, 255, 180);
        private fdTraceListener listener;
        private EngineGame Engine;
        public OnScreenDisplay(EngineGame engine) : base(engine)
        {
            Engine = engine;
            Visible = true;
            listener = new fdTraceListener();

            Trace.Listeners.Remove(@"Default");
            Trace.Listeners.Add(listener);
            this.Container = new Container();
            this.Container.ZIndex = int.MaxValue - 10;
            Engine.Stage.AddChild(Container);
        }
        internal bool Visible { get; set; }
        internal void ToggleShow()
        {
            Visible = !Visible;
/*            NotificationManager.ShowMessage("Debug messages are now " + (Visible ? "visible" : "hidden") + ". Press F11 to toggle.");*/
        }
        private Container? Container;

        public override void Update(GameTime gameTime)
        {
            if (Container != null)
                Container.Alpha = Visible ? 1 : 0;

            foreach (string s in listener.GetPendingMessages())
            {
                if (s == null || s.Length == 0) continue;

                /*Logger.Log(s, LoggingTarget.Debug, LogLevel.Debug);*/

                var text = new Text(s, Engine.DefaultFont)
                {
                    FontColor = Color.White,
                    ZIndex = int.MaxValue - 1
                };
                var txtSize = text.MeasureText();

                Container?.Children.ForEach(item =>
                {
                    item.Position.Y -= txtSize.Y;
                });

                //Remove any sprites above the screen
                if (Container?.Children.Count > 0 && Container?.Children[0].Position.Y < (Engine.graphicsDevice.PreferredBackBufferHeight / 2))
                    Container?.RemoveChild(Container.Children[0]);
                Container?.AddChild(text);
                text.Position.X = 0;
                text.Position.Y = Engine.graphicsDevice.PreferredBackBufferHeight - txtSize.Y;
            }
            base.Update(gameTime);
        }
        public void Clear()
        {
            Container.RemoveChild();
        }
    }
    public class fdTraceListener : TraceListener
    {
        List<string> pendingLines = new List<string>();

        public override void Write(string message)
        {
            System.Console.WriteLine(message);
            lock (pendingLines)
                pendingLines.Add(message);
        }

        public override void WriteLine(string message)
        {
            System.Console.WriteLine(message);
            lock (pendingLines)
                pendingLines.Add(message);
        }

        internal IEnumerable<string> GetPendingMessages()
        {
            lock (pendingLines)
            {
                List<string> pending = new List<string>(pendingLines);
                pendingLines.Clear();
                return pending;
            }
        }

        public override void Fail(string message)
        {
            System.Console.WriteLine(message);
            Fail(message, null);
        }

        public override void Fail(string message, string detailMessage)
        {
            System.Console.WriteLine(message + @"\n" + detailMessage);
            lock (pendingLines)
            {
                pendingLines.Add($@"ASSERT: {message} (hold shift to break)");
                if (detailMessage != null)
                    pendingLines.Add(detailMessage);

                if (Debugger.IsAttached && (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift)))
                    Debugger.Break();
            }
        }
    }
}
