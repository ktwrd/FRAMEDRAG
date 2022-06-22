using Engine.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class EngineGame : Game
    {
        public GraphicsDeviceManager graphicsDevice;
        public SpriteBatch spriteBatch;

        public double FixedUpdateTime = 1f / 30f;
        public double FixedFastUpdateTime = 1f / 500f;

        public double TargetFramerate = 1000f;
        public float MouseSensitivity = 6f;

        public Dictionary<string, Texture2D> TextureCache;

        public Stage Stage;

        public EngineGame()
        {
            TextureCache = new Dictionary<string, Texture2D>();

            graphicsDevice = new GraphicsDeviceManager(this);
        }

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
            graphicsDevice.PreferredBackBufferHeight = 1080;
            graphicsDevice.PreferredBackBufferWidth = 1920;
            graphicsDevice.HardwareModeSwitch = true;
            graphicsDevice.GraphicsProfile = GraphicsProfile.HiDef;
            graphicsDevice.SynchronizeWithVerticalRetrace = true;
            graphicsDevice.ApplyChanges();

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1f / TargetFramerate);

            Stage = new Stage(this);

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            checkFixedUpdate(gameTime);
            checkFixedFastUpdate(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        // FixedUpdate related
        private double fixedUpdateTimer = 0;
        private void checkFixedUpdate(GameTime gameTime)
        {
            while (fixedUpdateTimer >= FixedUpdateTime)
            {
                FixedUpdate(gameTime);
                fixedUpdateTimer -= FixedUpdateTime;
            }
        }
        protected void FixedUpdate(GameTime gameTime)
        { }
        // FixedFastUpdate related
        private double fixedfastUpdateTimer = 0;
        private void checkFixedFastUpdate(GameTime gameTime)
        {
            while (fixedfastUpdateTimer >= FixedFastUpdateTime)
            {
                FixedFastUpdate(gameTime);
                fixedfastUpdateTimer -= FixedFastUpdateTime;
            }
        }
        protected void FixedFastUpdate(GameTime gameTime)
        { }
    }
}
