using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FRAMEDRAG.Engine;
using FRAMEDRAG.Engine.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FRAMEDRAG.DVDExample
{
    internal class DVDGame : EngineGame
    {
        public DVDGame()
            :base()
        {
            WindowWidth = 1280;
            WindowHeight = 720;
        }
        protected override void Initialize()
        {
            base.Initialize();

            foreach (var file in Directory.GetFiles(@"C:\Work\_ExampleData\"))
            {
                LoadImage(file);
            }
            AddSpriteThing();
        }
        protected override void LoadContent()
        {
            base.LoadContent();

            countText = new Text($@"Count: {spritecount}", DefaultFont);
            countText.Position.X = 300;
            countText.ZIndex = int.MaxValue;
            Stage.AddChild(countText);

        }
        private Text countText;
        private int spritecount = 0;
        private int currentTextureIndex = 0;
        private void AddSpriteThing()
        {
            var testTexture = new Texture(TextureCache.ToArray()[currentTextureIndex].Value);
            currentTextureIndex++;
            if (currentTextureIndex >= TextureCache.Count)
                currentTextureIndex = 0;
            var testSprite = new DVDSprite(testTexture);
            testSprite.Scale.X = 200f / testTexture.Width;
            testSprite.Scale.Y = 200f / testTexture.Height;
            Stage.AddChild(testSprite);
            spritecount++;
            countText.SetText($@"Count: {spritecount}");
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void FixedUpdate(GameTime gameTime)
        {
            base.FixedUpdate(gameTime);

            var state = Keyboard.GetState();
            if (state.GetPressedKeys().Contains(Keys.Enter))
                AddSpriteThing();
        }
    }
}
