using System;
using System.Collections.Generic;
using System.Text;

namespace FRAMEDRAG.Engine
{
    public class ViewportEngineGame : EngineGame
    {
        public ViewportContainer Viewport;

        protected override void LoadContent()
        {
            base.LoadContent();
            Viewport = new ViewportContainer(this);
        }
    }
}
