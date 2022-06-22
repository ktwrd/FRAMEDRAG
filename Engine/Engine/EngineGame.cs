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

        public Dictionary<string, object> AssetTable;

        public EngineGame()
        {
            graphicsDevice = new GraphicsDeviceManager(this);

            graphicsDevice.PreferredBackBufferHeight = 720;
            graphicsDevice.PreferredBackBufferWidth = 1280;
            graphicsDevice.HardwareModeSwitch = false;
            graphicsDevice.GraphicsProfile = GraphicsProfile.HiDef;
            graphicsDevice.SynchronizeWithVerticalRetrace = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            checkFixedUpdate(gameTime);
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
