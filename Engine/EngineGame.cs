using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuakeConsole;
using SpriteFontPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public bool vsync
        { get{ return engine.IsFixedTimeStep; } set{ engine.IsFixedTimeStep = value; } }

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
    public class EngineGame : Game
    {
        public SpriteFont DefaultFont;

        public GraphicsDeviceManager graphicsDevice;
        public SpriteBatch spriteBatch;

        public double FixedUpdateTime = 1f / 15f;
        public double FixedFastUpdateTime = 1f / 500f;

        public double TargetFramerate = 1000000f;
        public float MouseSensitivity = 6f;

        public ConsoleComponent qConsole;
        internal DConsole _console;

        public Dictionary<string, Texture2D> TextureCache;

        public Stage Stage;

        public int ClientShowFPS = 0;
        public EngineGameAttributes Attributes;

        public static byte[] streamToByteArray(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
        public EngineGame()
        {
            Attributes = new EngineGameAttributes(this);
            TextureCache = new Dictionary<string, Texture2D>();

            graphicsDevice = new GraphicsDeviceManager(this);

            qConsole = new ConsoleComponent(this)
            {
                LogInput = cmd => Console.WriteLine(cmd)
            };
            Components.Add(qConsole);
            _console = new DConsole(this);
        }

        public int WindowWidth = 1600;
        public int WindowHeight = 900;

        public void LoadImage(string location)
        {
            var key = Path.GetFileNameWithoutExtension(location);
            if (TextureCache.ContainsKey(key))
                return;
            var stream = new FileStream(location, FileMode.Open);
            var tex = Texture2D.FromStream(GraphicsDevice, stream);
            TextureCache.Add(key.ToLower(), tex);
        }

        protected override void Initialize()
        {
            graphicsDevice.HardwareModeSwitch = true;
            graphicsDevice.GraphicsProfile = GraphicsProfile.HiDef;
            graphicsDevice.SynchronizeWithVerticalRetrace = true;
            UpdateWindowSize();

            IsFixedTimeStep = false;
            TargetElapsedTime = TimeSpan.FromSeconds(1f / TargetFramerate);

            Stage = new Stage(this);

            base.Initialize();
        }
        #region Window Size
        public void UpdateWindowSize()
        {
            UpdateWindowSize(WindowWidth, WindowHeight);
        }
        public void UpdateWindowSize(int width, int height)
        {
            graphicsDevice.PreferredBackBufferWidth = width;
            graphicsDevice.PreferredBackBufferHeight = height;
            graphicsDevice.ApplyChanges();
        }
        #endregion
        protected override void LoadContent()
        {
            var fontstream = streamToByteArray(Assembly.GetAssembly(typeof(EngineCursor)).GetManifestResourceStream(@"FRAMEDRAG.Engine.BuiltinAssets.font.ttf"));
            var baked = TtfFontBaker.Bake(
                fontstream,
                16,
                460,
                90,
                new[] { CharacterRange.BasicLatin });
            DefaultFont = baked.CreateSpriteFont(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Components.Add(new CursorOverlay(this));
            Components.Add(new StatsOverlay(this));

            base.LoadContent();
        }
        
        private List<DisplayObject> WalkedObjects = new List<DisplayObject>();

        internal KeyboardState? previousKeyboard = null;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            Stage.Draw(spriteBatch, this);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        #region Update
        protected override void Update(GameTime gameTime)
        {
            checkFixedUpdate(gameTime);
            checkFixedFastUpdate(gameTime);


            foreach (var comp in Components.OfType<Engine.Component>())
            {
                comp.Update(gameTime);
            }

            var keyboard = Keyboard.GetState();
            if (previousKeyboard != null)
            {
                if ((bool)previousKeyboard?.IsKeyUp(Keys.OemTilde) && keyboard.IsKeyDown(Keys.OemTilde))
                {
                    qConsole.ToggleOpenClose();
                }
            }

            previousKeyboard = keyboard;

            base.Update(gameTime);
        }
        #endregion
        
        #region FixedUpdate
        // FixedUpdate related
        private double fixedUpdateTimer = 0;
        private void checkFixedUpdate(GameTime gameTime)
        {
            fixedUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds;
            while (fixedUpdateTimer >= FixedUpdateTime)
            {
                FixedUpdate(gameTime);
                fixedUpdateTimer -= FixedUpdateTime;
            }
        }
        protected virtual void FixedUpdate(GameTime gameTime)
        {
            WalkedObjects = new List<DisplayObject>(Container.GetChildrenTree(Stage, false));

            foreach (var comp in Components.OfType<Engine.Component>())
            {
                comp.FixedUpdate(gameTime);
            }
        }
        #endregion
        
        #region FixedFastUpdate
        // FixedFastUpdate related
        private double fixedfastUpdateTimer = 0;
        private void checkFixedFastUpdate(GameTime gameTime)
        {
            fixedfastUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds;
            while (fixedfastUpdateTimer >= FixedFastUpdateTime)
            {
                FixedFastUpdate(gameTime);
                fixedfastUpdateTimer -= FixedFastUpdateTime;
            }
        }
        protected virtual void FixedFastUpdate(GameTime gameTime)
        {
            foreach (var displayObject in WalkedObjects)
            {
                displayObject.Update(gameTime, this);
            }
            foreach (var comp in Components.OfType<Engine.Component>())
            {
                comp.FixedFastUpdate(gameTime);
            }
        }
        #endregion
    }
}
