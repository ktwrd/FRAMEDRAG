using Microsoft.Xna.Framework;

namespace FRAMEDRAG.ChessExample;

public class ChessActionController
{
    public ChessBoard Board;
    public ChessActionController(ChessBoard board)
    {
        Board = board;
    }

    public bool IsMoveValid(ChessPiece piece, Vector2 from, Vector2 to)
    {
        
        return false;
    }
}