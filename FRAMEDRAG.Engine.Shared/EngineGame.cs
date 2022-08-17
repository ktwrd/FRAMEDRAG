using FRAMEDRAG.Engine.Display;
using FRAMEDRAG.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuakeConsole;
using SpriteFontPlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    public enum TextureDrawMode
    {
        Repeat,
        Stretch,
        Fit,
        Single
    }
    public delegate void MouseDelegate(Vector2 position, MouseButton button);
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
        public DConsole _console;

        public Dictionary<string, Texture2D> TextureCache;

        public Stage Stage;

        public int ClientShowFPS = 0;
        public EngineGameAttributes Attributes;

        public InteractionManager Interaction;
        internal OnScreenDisplay DebugOverlay;

        public event MouseDelegate MouseDownEvent;
        public event MouseDelegate MouseUpEvent;

        protected virtual void OnMouseDown(Vector2 position, MouseButton button) => MouseDownEvent?.Invoke(position, button);
        protected virtual void OnMouseUp(Vector2 position, MouseButton button) => MouseUpEvent?.Invoke(position, button);

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

            Window.AllowUserResizing = true;

            base.Initialize();
        }

        public Vector2 EngineCursorPosition { get; private set; }
        public Vector2 WindowCursorPosition { get; private set; }

        #region Window Size
        public void UpdateWindowSize()
        {
            UpdateWindowSize(VirtualWidth, VirtualHeight);
        }
        public void UpdateWindowSize(int width, int height, bool updateWindowSize=true)
        {
            VirtualWidth = width;
            VirtualHeight = height;
            if (updateWindowSize)
                RestoreWindowSize();
        }
        public void RestoreWindowSize()
        {
            graphicsDevice.PreferredBackBufferWidth = VirtualWidth;
            graphicsDevice.PreferredBackBufferHeight = VirtualHeight;
            graphicsDevice.ApplyChanges();
        }
        #endregion
        protected virtual Stage TargetCursorStage() => this.Stage;
        public Stage GetTargetCursorStage() => TargetCursorStage();
        public ResourceManager ResourceMan;
        protected override void LoadContent()
        {
            var asm = Assembly.GetAssembly(typeof(EngineCursor));
            var res = asm.GetManifestResourceNames();
            var targetStream = asm.GetManifestResourceStream("FRAMEDRAG.Engine.BuiltinAssets.font.ttf");
            if (targetStream == null)
                targetStream = asm.GetManifestResourceStream("FRAMEDRAG.Engine.DX.BuiltinAssets.font.ttf");
            if (targetStream == null)
                targetStream = asm.GetManifestResourceStream("FRAMEDRAG.Engine.GL.BuiltinAssets.font.ttf");
            byte[]? fontstream = GeneralHelper.StreamToByteArray(targetStream);
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

            /*Components.Add(new CursorOverlay(this, Stage));*/
            Components.Add(new CursorOverlay(this, TargetCursorStage()));
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

        public Vector2 EngineToScreen(Vector2 engineLocation)
        {
            var scaledStageScreenSize = new Vector2(
                StageScreenSize.X / (float)VirtualWidth,
                StageScreenSize.Y / (float)VirtualHeight);
            return engineLocation * scaledStageScreenSize;
        }
        public Vector2 ScreenToEngine(Vector2 screenLocation)
        {
            var scaledStageScreenSize = new Vector2(
                VirtualWidth / (float)StageScreenSize.X,
                VirtualHeight / (float)StageScreenSize.Y);
            return screenLocation * scaledStageScreenSize;
        }

        public Vector2 ScaledMousePosition = Vector2.Zero;
        public Vector2 ScaledMousePositionPrevious = Vector2.Zero;

        protected KeyboardState? previousKeyboard = null;
        protected MouseState? previousMouse = null;
        public Vector2 StageScreenPosition = Vector2.Zero;
        public Vector2 StageScreenSize = Vector2.Zero;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Stage.Draw(spriteBatch, this);


            base.Draw(gameTime);
        }
        protected void BaseDraw(GameTime gameTime) => base.Draw(gameTime);
        
        
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
            var mouse = Mouse.GetState();
            if (previousKeyboard != null)
            {
                if ((bool)previousKeyboard?.IsKeyUp(Keys.OemTilde) && keyboard.IsKeyDown(Keys.OemTilde))
                {
                    qConsole.ToggleOpenClose();
                }
            }

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                OnMouseDown(ScaledMousePosition, MouseButton.Left);
            }
            else if (mouse.LeftButton == ButtonState.Released && previousMouse?.LeftButton == ButtonState.Pressed)
            {
                OnMouseUp(ScaledMousePosition, MouseButton.Left);
            }

            if (mouse.MiddleButton == ButtonState.Pressed)
            {
                OnMouseDown(ScaledMousePosition, MouseButton.Middle);
            }
            else if (mouse.MiddleButton == ButtonState.Released && previousMouse?.MiddleButton == ButtonState.Pressed)
            {
                OnMouseUp(ScaledMousePosition, MouseButton.Middle);
            }
            if (mouse.RightButton == ButtonState.Pressed)
            {
                OnMouseDown(ScaledMousePosition, MouseButton.Right);
            }
            else if (mouse.RightButton == ButtonState.Released && previousMouse?.RightButton == ButtonState.Pressed)
            {
                OnMouseUp(ScaledMousePosition, MouseButton.Right);
            }
            ScaledMousePosition = new Vector2(mouse.Position.X, mouse.Position.Y);
            ScaledMousePositionPrevious = new Vector2(previousMouse?.Position.X ?? 0f, previousMouse?.Position.Y ?? 0f);


            previousMouse = mouse;
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
