using FRAMEDRAG.Engine;
using FRAMEDRAG.Engine.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.ChessExample
{
    public class ChessPiece : Component
    {
        public Container PieceContainer;
        public ChessPiece(ChessGame engine) : base(engine)
        {

        }
    }
}
