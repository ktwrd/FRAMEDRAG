using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    public class StatsOverlay : Component
    {

        public Container Container = new Container();
        public StatsOverlay(EngineGame engine)
            : base(engine)
        {
            TextOverlay = new Text(@"", engine.DefaultFont);
            TextOverlay.Position = Vector2.Zero;
            engine.Stage.AddChild(TextOverlay);
        }

        public Text TextOverlay;

        public double FPS = 0f;
        public override void Update(GameTime gameTime)
        {
            FPS = 1f / gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            TextOverlay.SetText($@"FPS: {Math.Round(FPS, 2)}");
        }
    }
}
