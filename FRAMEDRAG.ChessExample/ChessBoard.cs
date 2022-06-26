using FRAMEDRAG.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.ChessExample
{
    public enum Piece
    {
        Unknown = -1,
        Pawn,
        Bishop,
        Knight,
        Rook,
        Queen,
        King
    }
    public enum PieceTeam
    {
        Unknown = -1,
        White,
        Black
    }

    public class ChessBoard : Component
    {
        public ChessBoard(EngineGame engine) : base(engine)
        {
        }
    }
}
