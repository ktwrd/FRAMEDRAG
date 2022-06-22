using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine.Display
{
    public class Stage : Container
    {
        public Stage(EngineGame engine)
        {
            Engine = engine;
            HitArea = new Rectangle(new Point(0, 0), new Point(100000, 100000));
        }
        public EngineGame Engine;

        // TODO Interaction Manager
        public object? InteractionManager;

        public bool Dirty = true;

        internal DisplayObject[] __childrenAdded = Array.Empty<DisplayObject>();
        internal DisplayObject[] __childrenRemoved = Array.Empty<DisplayObject>();

        public Stage _Stage;
        public bool WorldVisible = true;

        public new void UpdateTransform()
        {
            WorldAlpha = 1;
            foreach (var child in Children)
                child.UpdateTransform();
            if (Dirty)
            {
                Dirty = false;
                // InteractionManager.Dirty = true;
            }

            if (Interactive)
            {
                //InteractionManager.Update();
            }
        }
        public Color BackgroundColor = Color.Black;
        public void SetBackgroundColor(Color? color)
        {
            var c = Color.Black;
            if (color != null)
                c = (Color)color;
            BackgroundColor = c;
        }
        public Vector2 GetMousePosition()
        {
            //return InteractionManager.Mouse.Global;
            return Vector2.Zero;
        }
    }
}
