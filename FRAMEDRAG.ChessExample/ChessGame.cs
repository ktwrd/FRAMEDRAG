using FRAMEDRAG.Engine;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.ChessExample
{
    public class ChessGame : EngineGame
    {
        protected override void LoadContent()
        {
            base.LoadContent();

            GameBoard = new ChessBoard(this);
            Components.Add(GameBoard);
        }
        protected override void Initialize()
        {
            base.Initialize();
            UpdateWindowSize(640, 480);
        }
        public ChessBoard GameBoard;
    }
}
