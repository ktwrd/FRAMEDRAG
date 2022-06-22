using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FRAMEDRAG.Engine;
using FRAMEDRAG.Engine.Textures;

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

            LoadImage(@"C:\Work\_ExampleData\garfield.jpg");
            var testTexture = new Texture(TextureCache[@"garfield"]);
            var testSprite = new DVDSprite(testTexture);
            testSprite.Scale.X = 0.1f;
            testSprite.Scale.Y = 0.1f;
            Stage.AddChild(testSprite);
        }
    }
}
