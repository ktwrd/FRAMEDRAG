using FRAMEDRAG.Engine;
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

            GameBoard = new ChessBoard();
            Components.Add(GameBoard);
        }
        public ChessBoard GameBoard;
    }
}
