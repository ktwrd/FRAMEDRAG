using FRAMEDRAG.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine.Display
{
    public enum MouseButton
    {
        Left,
        Middle,
        Right
    }
    public class Container : DisplayObject
    {
        public List<DisplayObject> Children = new List<DisplayObject>();

        public virtual void UpdateBounds()
        {

        }
        public FRectangle LocalBoundsRect;
        public Bounds LocalBounds;
        public virtual FRectangle GetLocalBounds(FRectangle rect)
        {
            if (rect == null)
            {
                if (LocalBoundsRect == null)
                    LocalBoundsRect = new FRectangle();
                rect = LocalBoundsRect;
            }
            if (LocalBounds == null)
                LocalBounds = new Bounds();

            return GetBounds(false, rect);
        }
        public FRectangle BoundsRect;
        public Bounds Bounds;
        public virtual FRectangle GetBounds(bool? skipUpdate, FRectangle? rect)
        {
            UpdateBounds();
            if (rect == null)
            {
                if (BoundsRect == null)
                    BoundsRect = new FRectangle();
                rect = BoundsRect;
            }
            return this.Bounds.GetRectangle(rect);
        }

        public DisplayObject GetChildAt(int index)
        {
            if (index < 1 || index > Children.Count)
                throw new IndexOutOfRangeException($@"Index {index} is less than 1 or greater than the amount of children ({Children.Count})");
            return Children[index];
        }
        public void AddChild(DisplayObject child)
        {
            if (child.Parent != null)
                child.Parent.RemoveChild(child);

            child.Parent = this;
            Children.Add(child);

            if (Stage != null)
            {
                var tmpChild = child;
                do
                {
                    if (tmpChild.Interactive)
                        Stage.Dirty = true;
                    tmpChild.Stage = Stage;
                    tmpChild = tmpChild._iNext;
                }
                while (tmpChild != null);
            }

            var childFirst = child.First;
            var childLast = child.Last;
            DisplayObject? previousObject = Last;
            DisplayObject? nextObject = previousObject?._iNext;

            var updateLast = this;
            var prevLast = previousObject;

            while (updateLast != null)
            {
                if (updateLast.Last == prevLast)
                    updateLast.Last = child.Last;
                updateLast = updateLast.Parent;
            }

            if (nextObject != null)
            {
                nextObject._iPrev = childLast;
                childLast._iNext = nextObject;
            }
            if (childFirst != null)
                childFirst._iPrev = previousObject;
            if (previousObject != null)
                previousObject._iNext = childFirst;
        }
        public void RemoveChild(DisplayObject child)
        {
            var index = Children.IndexOf(child);
            if (index != -1)
            {
                var childFirst = child.First;
                var childLast = child.Last;

                var nextObject = childLast._iNext;
                var previousObject = childFirst._iPrev;

                if (nextObject != null)
                    nextObject._iPrev = previousObject;
                previousObject._iNext = nextObject;

                if (Last == childLast)
                {
                    var tempLast = childFirst._iPrev;
                    var updateLast = this;
                    while (updateLast.Last == childLast.Last)
                    {
                        updateLast.Last = tempLast;
                        updateLast = updateLast.Parent;
                        if (updateLast == null) break;
                    }
                }

                childLast._iNext = null;
                childLast._iPrev = null;

                if (Stage != null)
                {
                    var tmpChild = child;
                    do
                    {
                        if (tmpChild.Interactive)
                            Stage.Dirty = true;
                        tmpChild.Stage = null;
                        tmpChild = tmpChild._iNext;
                    }
                    while (tmpChild != null);
                }
                child.Parent = null;
                Children.RemoveAt(index);
            }
            else
            {
                throw new Exception(@"The supplied DisplayObject must be a child");
            }
        }
        public new void UpdateTransform()
        {
            if (!Visible)
                return;
            base.UpdateTransform();
            foreach (var child in Children)
            {
                child.UpdateTransform();
            }
        }
        public static DisplayObject[] GetChildrenTree(Container container, bool? notRoot)
        {
            var childs = new List<DisplayObject>();
            foreach (var child in container.Children)
            {
                childs.Add(child);
                if (child.GetType().IsAssignableFrom(typeof(Container)))
                    childs.Concat(GetChildrenTree((Container)child, true));
            }


            if ((bool)!notRoot)
            {
                SortedDictionary<int, List<DisplayObject>> dlk = new();
                var sortedChilds = new List<DisplayObject>();
                foreach (var c in childs)
                {
                    if (!dlk.ContainsKey(c.ZIndex))
                        dlk.Add(c.ZIndex, new List<DisplayObject>());
                    dlk[c.ZIndex].Add(c);
                }

                foreach (var pair in dlk)
                {
                    sortedChilds = new List<DisplayObject>(sortedChilds.Concat(pair.Value));
                }

                childs = sortedChilds;
            }
            return childs.ToArray();
        }
        public override void Draw(SpriteBatch spriteBatch, EngineGame engine)
        {
            foreach (var child in Children)
            {
                child.Draw(spriteBatch, engine);
            }
            base.Draw(spriteBatch, engine);
        }
        protected bool IsCursorInteracting = false;
        private MouseState? previousMouseState = null;
        public virtual void InteractiveCheck(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
            if (mouse.X > Bounds.minX && mouse.X < Bounds.maxX)
                if (mouse.Y > Bounds.minY && mouse.Y < Bounds.maxY)
                {
                    IsCursorInteracting = true;
                    if (mouse.LeftButton == ButtonState.Pressed || mouse.MiddleButton == ButtonState.Pressed || mouse.RightButton == ButtonState.Pressed)
                        MouseDown(mouse, keyboard);
                    if (previousMouseState != null)
                    {
                        if (mouse.LeftButton == ButtonState.Released && previousMouseState?.LeftButton == ButtonState.Pressed)
                            MouseUp(MouseButton.Left, (MouseState)previousMouseState, mouse);
                        if (mouse.RightButton == ButtonState.Released && previousMouseState?.RightButton == ButtonState.Pressed)
                            MouseUp(MouseButton.Right, (MouseState)previousMouseState, mouse);
                        if (mouse.MiddleButton == ButtonState.Released && previousMouseState?.MiddleButton == ButtonState.Pressed)
                            MouseUp(MouseButton.Middle, (MouseState)previousMouseState, mouse);
                    }
                    MouseHover(mouse, keyboard);
                    previousMouseState = mouse;
                    return;
                }
            IsCursorInteracting = false;
        }

        protected virtual void MouseDown(MouseState mouse, KeyboardState keyboard)
        { }
        protected virtual void MouseUp(MouseButton button, MouseState previous, MouseState current)
        { }
        protected virtual void MouseHover(MouseState mouse, KeyboardState keyboard)
        { }
    }
}
