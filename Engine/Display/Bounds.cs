using FRAMEDRAG.Engine.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine.Display
{
    public class Bounds
    {
        public double minX = double.PositiveInfinity;
        public double minY = double.PositiveInfinity;
        public double maxX = double.NegativeInfinity;
        public double maxY = double.NegativeInfinity;
        public FRectangle? Rectangle;
        public int updateID = -1;

        public bool isEmpty()
        {
            return minX > maxX || minY > maxY;
        }
        public void Clear()
        {
            minX = double.PositiveInfinity;
            minY = double.PositiveInfinity;
            maxX = double.NegativeInfinity;
            maxY = double.NegativeInfinity;
        }
        public FRectangle GetRectangle(FRectangle? rect)
        {
            if (minX > maxX || minY > maxY)
                return new FRectangle();

            if (rect == null)
                rect = new FRectangle(0, 0, 1, 1);
            rect.X = minX;
            rect.Y = minY;
            rect.Width = maxX - minX;
            rect.Height = maxY - minY;

            return rect;
        }
        public void AddPoint(Vector2 point)
        {
            minX = Math.Min(minX, point.X);
            maxX = Math.Max(maxX, point.X);
            minY = Math.Min(minY, point.Y);
            maxY = Math.Max(minY, point.Y);
        }
        public void AddBounds(Bounds bounds)
        {
            var mix = minX;
            var miy = minY;
            var max = maxX;
            var may = maxY;

            minX = bounds.minX < mix ? bounds.minX : mix;
            minY = bounds.minY < miy ? bounds.minY : miy;
            maxX = bounds.maxX < max ? bounds.maxX : max;
            maxY = bounds.maxY < may ? bounds.maxY : may;
        }
        public void AddBoundsMask(Bounds bounds, Bounds mask)
        {
            var _minX = bounds.minX > mask.minX ? bounds.minX : mask.minX;
            var _minY = bounds.minY > mask.minY ? bounds.minY : mask.minY;
            var _maxX = bounds.maxX < mask.maxX ? bounds.maxX : mask.maxX;
            var _maxY = bounds.maxY < mask.maxY ? bounds.maxY : mask.maxY;

            if (_minX <= _maxX && _minY <= _maxY)
            {
                var Xmin = minX;
                var Ymin = minY;
                var Xmax = maxX;
                var Ymax = maxY;
                minX = _minX < Xmin ? _minX : Xmin;
                minY = _minY < Ymin ? _minY : Ymin;
                maxX = _maxX > Xmax ? _maxX : Xmax;
                maxY = _maxY > Ymax ? _maxY : Ymax;
            }
        }
        public void AddBoundsArea(Bounds bounds, FRectangle area)
        {
            var _minX = bounds.minX > area.X ? bounds.minX : area.X;
            var _minY = bounds.minY > area.Y ? bounds.minY : area.Y;
            var _maxX = bounds.maxX < area.X + area.Width ? bounds.maxX : (area.X + area.Width);
            var _maxY = bounds.maxY < area.Y + area.Height ? bounds.maxY : (area.Y + area.Height);

            if (_minX <= _maxX && _minY <= _maxY)
            {
                var Xmin = minX;
                var Ymin = minY;
                var Xmax = maxX;
                var Ymax = maxY;
                minX = _minX < Xmin ? _minX : Xmin;
                minY = _minY < Ymin ? _minY : Ymin;
                maxX = _maxX > Xmax ? _maxX : Xmax;
                maxY = _maxY > Ymax ? _maxY : Ymax;
            }
        }
        public void Pad(double x, double y)
        {
            if (isEmpty())
            {
                minX -= x;
                maxX += x;
                minY -= y;
                maxY += y;
            }
        }
    }
}
