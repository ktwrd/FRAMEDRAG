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
        protected override void Initialize()
        {
            base.Initialize();

            LoadImage(@"C:\Work\_ExampleData\garfield.jpg");
            var testTexture = new Texture(TextureCache[@"garfield"]);
            var testSprite = new DVDSprite(testTexture);
            testSprite.Scale.X = 0.5f;
            testSprite.Scale.Y = 0.5f;
            Stage.AddChild(testSprite);
        }
    }
}
