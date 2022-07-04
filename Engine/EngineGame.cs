﻿using FRAMEDRAG.Engine.Display;
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

        public int VirtualWidth = 800;
        public int VirtualHeight = 450;

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
            graphicsDevice.ApplyChanges();
            UpdateWindowSize();

            IsFixedTimeStep = false;
            TargetElapsedTime = TimeSpan.FromSeconds(1f / TargetFramerate);

            Stage = new Stage(this);

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            GraphicsDevice.PresentationParameters.MultiSampleCount = 0;
            scene = new RenderTarget2D(GraphicsDevice, VirtualWidth, VirtualHeight, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            Window.AllowUserResizing = true;

            base.Initialize();
        }

        public Vector2 EngineCursorPosition { get; private set; }
        public Vector2 WindowCursorPosition { get; private set; }

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
            UpdateWindowSize(0,0);
        }
        public void UpdateWindowSize(int width, int height)
        {
            /*graphicsDevice.PreferredBackBufferWidth = width;
            graphicsDevice.PreferredBackBufferHeight = height;
            graphicsDevice.ApplyChanges();*/
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

        public float outputAspect
        {
            get
            {
                return Window.ClientBounds.Width / Window.ClientBounds.Height;
            }
        }
        public float targetAspect
        {
            get
            {
                return VirtualWidth / VirtualHeight;
            }
        }

        internal KeyboardState? previousKeyboard = null;
        private RenderTarget2D scene;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(scene);
            spriteBatch.Begin(SpriteSortMode.BackToFront);
            Stage.Draw(spriteBatch, this);
            spriteBatch.End();

            base.Draw(gameTime);

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

            GraphicsDevice.SetRenderTarget(null);

            // clear to get black bars
            GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            // draw a quad to get the draw buffer to the back buffer
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointWrap);
            spriteBatch.Draw(scene, dst, Color.White);
            spriteBatch.End();
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
