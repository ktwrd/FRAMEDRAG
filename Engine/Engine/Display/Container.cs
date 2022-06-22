using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Display
{
    public class Container : DisplayObject
    {
        public List<DisplayObject> Children = new List<DisplayObject>();

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

    }
}
