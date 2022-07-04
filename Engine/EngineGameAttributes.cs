using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    public class EngineGameAttributes
    {
        private EngineGame engine;
        public EngineGameAttributes(EngineGame engine)
        {
            this.engine = engine;
        }
        public int showfps = 0;
        public int debugtxt = 0;
        public Color framebuffercolor = Color.Black;
        public int fpsmax
        {
            get
            {
                return engine.TargetFramerate;
            }
            set
            {
                if (value <= 5000)
                {
                    engine.TargetFramerate = value;
                    engine.TargetElapsedTime = TimeSpan.FromSeconds(1f / engine.TargetFramerate);
                }
            }
        }
        public bool fixedtime
        { get { return engine.IsFixedTimeStep; } set { engine.IsFixedTimeStep = value; } }
        public bool vsync
        {
            get { return engine.graphicsDevice.SynchronizeWithVerticalRetrace;  }
            set
            {
                engine.graphicsDevice.SynchronizeWithVerticalRetrace = value;
                engine.graphicsDevice.ApplyChanges();
            }
        }

        public void log(string val)
        {
            Trace.WriteLine(val);
        }
        public void clearlog()
        {
            engine.DebugOverlay.Clear();
        }

        public bool fullscreen
        {
            get
            {
                return engine.graphicsDevice.IsFullScreen;
            }
            set
            {
                if (value != engine.graphicsDevice.IsFullScreen)
                    engine.graphicsDevice.ToggleFullScreen();
            }
        }
        public int windowWidth
        {
            get
            {
                return engine.graphicsDevice.PreferredBackBufferWidth;
            }
            set
            {
                if (value > 0)
                {
                    engine.graphicsDevice.PreferredBackBufferWidth = value;
                    engine.graphicsDevice.ApplyChanges();
                }
            }
        }
        public int windowHeight
        {
            get
            {
                return engine.graphicsDevice.PreferredBackBufferHeight;
            }
            set
            {
                if (value > 0)
                {
                    engine.graphicsDevice.PreferredBackBufferHeight = value;
                    engine.graphicsDevice.ApplyChanges();
                }
            }
        }
        public void quit()
        {
            Environment.Exit(0);
        }
    }
}
