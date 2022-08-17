using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine.Core
{
    public class FRectangle
    {
        public FRectangle()
            : this (0,0,0,0)
        { }
        public FRectangle(double X, double Y, double Width, double Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }

        public double X = 0f;
        public double Y = 0f;
        public double Width = 0f;
        public double Height = 0f;
    }
}
