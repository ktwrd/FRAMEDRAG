using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;

namespace FRAMEDRAG.Engine.Primitives
{
    public class GraphicsPath
    {
        public static GraphicsPath Empty()
        {
            return new GraphicsPath();
        }
        public Vector2[] Points = Array.Empty<Vector2>();
        public float LineWidth = 0;
        public Color LineColor = Color.Black;
        public float LineAlpha = 1;
        public Color FillColor = Color.White;
        public float FillAlpha = 1;
        public bool Filling = false;
        public GraphicsMethods Method;
    }
    public enum GraphicsMethods
    {
        POLY,
        RECT,
        CIRC,
        ELIP
    }
    public class Graphics : Container
    {
        public Graphics()
        {
            Renderable = true;
        }

        public bool Dirty = false;
        public bool ClearDirty = false;

        private bool Filling = false;
        public Color FillColor = Color.White;
        public float FillAlpha = 1f;

        public Color LineColor = Color.Black;
        public float LineAlpha = 1f;
        public float LineWidth = 0f;

        private List<GraphicsPath> GraphicsData = new List<GraphicsPath>();
        private GraphicsPath currentPath = GraphicsPath.Empty();

        public void LineStyle(float? w, Color? c, float? a)
        {
            if (currentPath.Points.Length == 0)
                GraphicsData.RemoveAt(0);

            LineWidth = (float)(w == null ? 0f : w);
            LineColor = (Color)(c == null ? Color.Black : c);
            LineAlpha = (float)(a == null ? Alpha : a);

            currentPath.LineColor = LineColor;
            currentPath.LineAlpha = LineAlpha;
            currentPath.LineWidth = LineWidth;

            currentPath.FillColor = FillColor;
            currentPath.FillAlpha = FillAlpha;

            currentPath.Filling = Filling;
            currentPath.Points = Array.Empty<Vector2>();
            currentPath.Method = GraphicsMethods.POLY;

            GraphicsData.Add(currentPath);
        }
        public void MoveTo(float x, float y)
        {
            MoveTo(new Vector2(x, y));
        }
        public void MoveTo(Vector2 position)
        {
            if (currentPath.Points.Length == 0)
                GraphicsData.RemoveAt(0);


            currentPath.LineColor = LineColor;
            currentPath.LineAlpha = LineAlpha;
            currentPath.LineWidth = LineWidth;

            currentPath.FillColor = FillColor;
            currentPath.FillAlpha = FillAlpha;

            currentPath.Filling = Filling;
            currentPath.Points = Array.Empty<Vector2>();
            currentPath.Method = GraphicsMethods.POLY;

            currentPath.Points[currentPath.Points.Length] = position;

            GraphicsData.Add(currentPath);
        }

        public void LineTo(float x, float y)
        {
            LineTo(new Vector2(x, y));
        }
        public void LineTo(Vector2 position)
        {
            currentPath.Points[currentPath.Points.Length] = position;
            Dirty = true;
        }

        public void BeginFill(Color? color, float? alpha)
        {
            Filling = true;
            FillColor = (Color)(color == null ? Color.Black : color);
            FillAlpha = (float)(alpha == null ? Alpha : alpha);
        }
        public void EndFill()
        {
            Filling = false;
            FillColor = Color.Black;
            FillAlpha = 1;
        }

        public void DrawCircle(Vector2 position, float radius)
        {
            if (currentPath.Points.Length == 0)
                GraphicsData.RemoveAt(0);

            currentPath.LineColor = LineColor;
            currentPath.LineAlpha = LineAlpha;
            currentPath.LineWidth = LineWidth;

            currentPath.FillColor = FillColor;
            currentPath.FillAlpha = FillAlpha;

            currentPath.Filling = Filling;
            currentPath.Points = Array.Empty<Vector2>();
            currentPath.Method = GraphicsMethods.CIRC;

            currentPath.Points[currentPath.Points.Length] = position;
            currentPath.Points[currentPath.Points.Length] = new Vector2(radius, radius);

            GraphicsData.Add(currentPath);

            Dirty = true;
        }
        public void DrawCircle(float x, float y, float radius)
        {
            DrawCircle(new Vector2(x, y), radius);
        }

        public void DrawElipse(Vector2 position, Vector2 size)
        {
            if (currentPath.Points.Length == 0)
                GraphicsData.RemoveAt(0);

            currentPath.LineColor = LineColor;
            currentPath.LineAlpha = LineAlpha;
            currentPath.LineWidth = LineWidth;

            currentPath.FillColor = FillColor;
            currentPath.FillAlpha = FillAlpha;

            currentPath.Filling = Filling;
            currentPath.Points = Array.Empty<Vector2>();
            currentPath.Method = GraphicsMethods.ELIP;

            currentPath.Points[currentPath.Points.Length] = position;
            currentPath.Points[currentPath.Points.Length] = size;

            GraphicsData.Add(currentPath);

            Dirty = true;
        }

        public void Clear()
        {
            LineWidth = 0;
            Filling = false;
            Dirty = true;
            ClearDirty = true;
            GraphicsData = new List<GraphicsPath>();
        }
    }
}
