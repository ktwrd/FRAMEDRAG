using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    public class Text : Container
    {
        public Text(string text, SpriteFont font)
            : base()
        {
            TextContent = text;
            Font = font;
        }

        internal string TextContent = @"";
        public void SetText(string content)
        {
            TextContent = content;
            UpdateText();
        }

        internal string[] lines = Array.Empty<string>();
        internal int[] lineHeight = Array.Empty<int>();
        internal int[] lineWidth = Array.Empty<int>();
        public SpriteFont Font;
        public Vector2 Size = Vector2.Zero;
        public void UpdateText()
        {
            var lines = Regex.Split(TextContent, @"(?:\r\n|\r|\n)");

            var lineWidths = new List<int>();
            var lineHeights = new List<int>();
            var maxLineSize = Vector2.Zero;
            foreach (var line in lines)
            {
                var lineWidth = Font.MeasureString(line);
                lineWidths.Add(Convert.ToInt32(lineWidth.X));
                lineHeights.Add(Convert.ToInt32(lineWidth.Y));
                if (lineWidth.X > maxLineSize.X)
                    maxLineSize.X = lineWidth.X;
                if (lineWidth.Y > maxLineSize.Y)
                    maxLineSize.Y = lineWidth.Y;
            }
            Size = new Vector2(
                maxLineSize.X,
                lineHeights.Sum()
            );

            lineHeight = lineHeights.ToArray();
            lineWidth = lineWidths.ToArray();
            this.lines = lines;
        }

        #region Text Drawing
        public Color FontColor = Color.White;
        public Texture2D BlankTexture = null;
        public override void Draw(SpriteBatch spriteBatch, EngineGame engine)
        {
            if (BlankTexture == null)
            {
                var t = new Texture2D(engine.GraphicsDevice, 1, 1);
                t.SetData(new Color[] { Color.White });
                BlankTexture = t;
            }
            var offset = Vector2.Zero;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                var pos = GlobalPosition() + offset;
                spriteBatch.DrawString(
                    engine.DefaultFont,
                    line,
                    pos,
                    FontColor,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    Convert.ToSingle(ZIndex / int.MaxValue));
                offset += new Vector2(0, lineHeight[i]);
            }
        }
        #endregion
    }
}
