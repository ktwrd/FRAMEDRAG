using FRAMEDRAG.Engine.Display;
using FRAMEDRAG.Engine.Helpers;
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
    public class EngineGame : Game
    {
        public SpriteFont DefaultFont;

        public GraphicsDeviceManager graphicsDevice;
        public SpriteBatch spriteBatch;

        public double FixedUpdateTime = 1f / 15f;
        public double FixedFastUpdateTime = 1f / 500f;

        public int TargetFramerate = 5000;
        public float MouseSensitivity = 6f;

        public ConsoleComponent qConsole;
        internal DConsole _console;

        public Dictionary<string, Texture2D> TextureCache;

        public Stage Stage;

        public int ClientShowFPS = 0;
        public EngineGameAttributes Attributes;

        public InteractionManager Interaction;
        internal OnScreenDisplay DebugOverlay;
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
            GraphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None
            };
            graphicsDevice.HardwareModeSwitch = true;
            graphicsDevice.GraphicsProfile = GraphicsProfile.HiDef;
            graphicsDevice.SynchronizeWithVerticalRetrace = false;
            UpdateWindowSize();

            IsFixedTimeStep = false;
            TargetElapsedTime = TimeSpan.FromSeconds(1f / TargetFramerate);

            Stage = new Stage(this);

            base.Initialize();
        }
        #region Window Size
        public float WindowRatio
        {
            get
            {
                return graphicsDevice.PreferredBackBufferWidth / 900f;
            }
        }
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
        public ResourceManager ResourceMan;
        protected override void LoadContent()
        {
            byte[]? fontstream = GeneralHelper.StreamToByteArray(Assembly.GetAssembly(typeof(EngineCursor)).GetManifestResourceStream(@"FRAMEDRAG.Engine.BuiltinAssets.font.ttf"));
            if (fontstream != null)
            {
                var baked = TtfFontBaker.Bake(
                    fontstream,
                    16,
                    460,
                    90,
                    new[] { CharacterRange.BasicLatin });
                DefaultFont = baked.CreateSpriteFont(GraphicsDevice);
            }
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ResourceMan = new ResourceManager(this);

            Components.Add(new CursorOverlay(this));
            Components.Add(new StatsOverlay(this));
            Interaction = new InteractionManager(this);
            Components.Add(Interaction);
            Components.Add(ResourceMan);

            DebugOverlay = new OnScreenDisplay(this);
            Components.Add(DebugOverlay);

            ResourceMan.LoadContent();

            base.LoadContent();
        }
        
        private List<DisplayObject> WalkedObjects = new List<DisplayObject>();

        internal KeyboardState? previousKeyboard = null;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Attributes.framebuffercolor);
            spriteBatch.Begin(SpriteSortMode.BackToFront);
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
