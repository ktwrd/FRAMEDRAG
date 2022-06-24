using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
            this.engine = engine;
            TextOverlay = new Text(@"", engine.DefaultFont);
            TextOverlay.Position = Vector2.Zero;
        }
        private EngineGame engine;

        public Text TextOverlay;

        public double FPS = 0f;
        public double FPS_Max = 0f;
        public double FPS_Min = double.MaxValue;
        public override void Update(GameTime gameTime)
        {
            if (engine.Attributes.showfps < 1) return;
            FPS = 1000.0f / gameTime.ElapsedGameTime.TotalMilliseconds;
            if (FPS == double.PositiveInfinity || FPS == double.NegativeInfinity) return;
            if (FPS_Min > FPS)
                FPS_Min = FPS;
            if (FPS_Max < FPS)
                FPS_Max = FPS;
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
                ResetFPSValues();
        }
        private void ResetFPSValues()
        {
            FPS_Max = 0f;
            FPS_Min = double.MaxValue;
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            if (engine.Attributes.showfps > 0)
            {
                if (TextOverlay.Parent == null)
                    engine.Stage.AddChild(TextOverlay);
                string[] lines = new string[]
                {
                    $"FPS      : {Math.Round(FPS, 2).ToString().PadLeft(10)}",
                };
                if (engine.Attributes.showfps > 1)
                {
                    lines = new string[]
                    {
                        $"FPS      : {Math.Round(FPS, 2).ToString().PadLeft(10)}",
                        $"FPS (Max): {Math.Round(FPS_Max, 2).ToString().PadLeft(10)}",
                        $"FPS (Min): {Math.Round(FPS_Min, 2).ToString().PadLeft(10)}",
                    };
                }
                TextOverlay.SetText(string.Join("\n", lines));
            }
            else
            {
                if (TextOverlay.Parent != null)
                    TextOverlay.Parent.RemoveChild(TextOverlay);
            }
        }
    }
}
