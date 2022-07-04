using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    public class ResourceManager : Component
    {
        private EngineGame engine;
        public ResourceManager(EngineGame engine)
            : base(engine)
        {
            this.engine = engine;
        }
        private Dictionary<string, Texture2D> TextureCache = new Dictionary<string, Texture2D>();
        private Dictionary<string, SpriteFont> FontCache = new Dictionary<string, SpriteFont>();
        private Dictionary<string, Stream> StreamCache = new Dictionary<string, Stream>();
        private Dictionary<string, SoundEffect> SoundEffectCache = new Dictionary<string, SoundEffect>();
        internal void LoadContent()
        {
            // Get a list of all of the builtin assets
            var engineAssembly = Assembly.GetAssembly(typeof(ResourceManager));

            var dirtyResourceNames = engineAssembly.GetManifestResourceNames();

            // Sort the resources
            foreach (var name in dirtyResourceNames)
            {
                var cleanName = name.Replace(@"FRAMEDRAG.Engine.", @"Engine.");
                var splitArr = cleanName.Split(@".");
                cleanName = String.Join(@".", splitArr.SkipLast(1).ToArray());
                var extension = splitArr[splitArr.Length - 1].ToLower();
                var stream = engineAssembly.GetManifestResourceStream(name);
                if (stream == null)
                    continue;
                if (StreamCache.ContainsKey(cleanName))
                    StreamCache.Add(cleanName, stream);
                switch (extension)
                {
                    case @"png":
                    case @"jpg":
                    case @"bmp":
                    case @"tif":
                    case @"dds":
                        TextureCache.Add(cleanName, Texture2D.FromStream(engine.GraphicsDevice, stream));
                        break;
                    case @"wav":
                        SoundEffectCache.Add(cleanName, SoundEffect.FromStream(stream));
                        break;
                }
            }
        }

        public Texture2D? GetTexture(string key) {
            if (TextureCache.ContainsKey(key))
                return TextureCache[key];
            return null;
        }
    }
}
