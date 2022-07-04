using Microsoft.Xna.Framework;

namespace FRAMEDRAG.ChessExample;

public class PieceMoveOffset
{
    public Vector2 pos;
    public Piece piece;
}

public enum DirectionType
{
    Up,
    Down,
    Left,
    Right,
    UpRight,
    UpLeft,
    DownRight,
    DownLeft
}
public class ChessActionController
{
    public ChessBoard Board;
    public ChessActionController(ChessBoard board)
    {
        Board = board;
    }

    public Dictionary<Piece, Vector2[]> PieceOffsets = new Dictionary<Piece, Vector2[]>()
    {
        {
            Piece.Knight,
            new Vector2[]
            {
                new Vector2(-1, 2),
                new Vector2(-1, -2),
                new Vector2(1, 2),
                new Vector2(1, -2),
                new Vector2(-2, 1),
                new Vector2(-2, -1),
                new Vector2(2, 1),
                new Vector2(2, -1)
            }
        },
        {
            Piece.King,
            new Vector2[]
            {
                /* bottom right to top left */
                new Vector2(-1, 1),
                new Vector2(-2, 2),
                new Vector2(-3, 3),
                new Vector2(-4, 4),
                new Vector2(-5, 5),
                new Vector2(-6, 6),
                new Vector2(-7, 7),
                
                /* bottom left to top right */
                new Vector2(1, 1),
                new Vector2(2, 2),
                new Vector2(3, 3),
                new Vector2(4, 4),
                new Vector2(5, 5),
                new Vector2(6, 6),
                new Vector2(7, 7),
                
                /* top left to bottom right */
                new Vector2(1, -1),
                new Vector2(2, -2),
                new Vector2(3, -3),
                new Vector2(4, -4),
                new Vector2(5, -5),
                new Vector2(6, -6),
                new Vector2(7, -7),
                
                /* top right to bottom left */
                new Vector2(-1, -1),
                new Vector2(-2, -2),
                new Vector2(-3, -3),
                new Vector2(-4, -4),
                new Vector2(-5, -5),
                new Vector2(-6, -6),
                new Vector2(-7, -7),
                
                /* top to bottom */
                new Vector2(0, 1),
                new Vector2(0, 2),
                new Vector2(0, 3),
                new Vector2(0, 4),
                new Vector2(0, 5),
                new Vector2(0, 6),
                new Vector2(0, 7),
                
                /* bottom to top */
                new Vector2(0, -1),
                new Vector2(0, -2),
                new Vector2(0, -3),
                new Vector2(0, -4),
                new Vector2(0, -5),
                new Vector2(0, -6),
                new Vector2(0, -7),
                
                /* left to right */
                new Vector2(1, 0),
                new Vector2(2, 0),
                new Vector2(3, 0),
                new Vector2(4, 0),
                new Vector2(5, 0),
                new Vector2(6, 0),
                new Vector2(7, 0),
                
                /* right to left */
                new Vector2(-1, 0),
                new Vector2(-2, 0),
                new Vector2(-3, 0),
                new Vector2(-4, 0),
                new Vector2(-5, 0),
                new Vector2(-6, 0),
                new Vector2(-7, 0),
            }
        },
        {
            Piece.Bishop,
            new Vector2[]
            {
                /* bottom right to top left */
                new Vector2(-1, 1),
                new Vector2(-2, 2),
                new Vector2(-3, 3),
                new Vector2(-4, 4),
                new Vector2(-5, 5),
                new Vector2(-6, 6),
                new Vector2(-7, 7),
                
                /* bottom left to top right */
                new Vector2(1, 1),
                new Vector2(2, 2),
                new Vector2(3, 3),
                new Vector2(4, 4),
                new Vector2(5, 5),
                new Vector2(6, 6),
                new Vector2(7, 7),
                
                /* top left to bottom right */
                new Vector2(1, -1),
                new Vector2(2, -2),
                new Vector2(3, -3),
                new Vector2(4, -4),
                new Vector2(5, -5),
                new Vector2(6, -6),
                new Vector2(7, -7),
                
                /* top right to bottom left */
                new Vector2(-1, -1),
                new Vector2(-2, -2),
                new Vector2(-3, -3),
                new Vector2(-4, -4),
                new Vector2(-5, -5),
                new Vector2(-6, -6),
                new Vector2(-7, -7),
            }
        },
        {
            Piece.Pawn,
            new Vector2[]
            {
                new Vector2(1, 1),
                new Vector2(-1, 1),
            }
        },
        {
            Piece.Queen,
            new Vector2[]
            {
                /* bottom right to top left */
                new Vector2(-1, 1),
                new Vector2(-2, 2),
                new Vector2(-3, 3),
                new Vector2(-4, 4),
                new Vector2(-5, 5),
                new Vector2(-6, 6),
                new Vector2(-7, 7),
                
                /* bottom left to top right */
                new Vector2(1, 1),
                new Vector2(2, 2),
                new Vector2(3, 3),
                new Vector2(4, 4),
                new Vector2(5, 5),
                new Vector2(6, 6),
                new Vector2(7, 7),
                
                /* top left to bottom right */
                new Vector2(1, -1),
                new Vector2(2, -2),
                new Vector2(3, -3),
                new Vector2(4, -4),
                new Vector2(5, -5),
                new Vector2(6, -6),
                new Vector2(7, -7),
                
                /* top right to bottom left */
                new Vector2(-1, -1),
                new Vector2(-2, -2),
                new Vector2(-3, -3),
                new Vector2(-4, -4),
                new Vector2(-5, -5),
                new Vector2(-6, -6),
                new Vector2(-7, -7),
                
                /* top to bottom */
                new Vector2(0, 1),
                new Vector2(0, 2),
                new Vector2(0, 3),
                new Vector2(0, 4),
                new Vector2(0, 5),
                new Vector2(0, 6),
                new Vector2(0, 7),
                
                /* bottom to top */
                new Vector2(0, -1),
                new Vector2(0, -2),
                new Vector2(0, -3),
                new Vector2(0, -4),
                new Vector2(0, -5),
                new Vector2(0, -6),
                new Vector2(0, -7),
                
                /* left to right */
                new Vector2(1, 0),
                new Vector2(2, 0),
                new Vector2(3, 0),
                new Vector2(4, 0),
                new Vector2(5, 0),
                new Vector2(6, 0),
                new Vector2(7, 0),
                
                /* right to left */
                new Vector2(-1, 0),
                new Vector2(-2, 0),
                new Vector2(-3, 0),
                new Vector2(-4, 0),
                new Vector2(-5, 0),
                new Vector2(-6, 0),
                new Vector2(-7, 0),
            }
        },
        {
            Piece.Rook,
            new Vector2[]
            {
                
                /* top to bottom */
                new Vector2(0, 1),
                new Vector2(0, 2),
                new Vector2(0, 3),
                new Vector2(0, 4),
                new Vector2(0, 5),
                new Vector2(0, 6),
                new Vector2(0, 7),
                
                /* bottom to top */
                new Vector2(0, -1),
                new Vector2(0, -2),
                new Vector2(0, -3),
                new Vector2(0, -4),
                new Vector2(0, -5),
                new Vector2(0, -6),
                new Vector2(0, -7),
                
                /* left to right */
                new Vector2(1, 0),
                new Vector2(2, 0),
                new Vector2(3, 0),
                new Vector2(4, 0),
                new Vector2(5, 0),
                new Vector2(6, 0),
                new Vector2(7, 0),
                
                /* right to left */
                new Vector2(-1, 0),
                new Vector2(-2, 0),
                new Vector2(-3, 0),
                new Vector2(-4, 0),
                new Vector2(-5, 0),
                new Vector2(-6, 0),
                new Vector2(-7, 0),
            }
        }
    };

    public Dictionary<DirectionType, Vector2[]> DirectionLookupTable = new Dictionary<DirectionType, Vector2[]>()
    {
        {
            DirectionType.UpLeft,
            new []
            {
                /* bottom right to top left */
                new Vector2(-1, 1),
                new Vector2(-2, 2),
                new Vector2(-3, 3),
                new Vector2(-4, 4),
                new Vector2(-5, 5),
                new Vector2(-6, 6),
                new Vector2(-7, 7),
            }
        },
        {
            DirectionType.UpRight,
            new []
            {
                /* bottom left to top right */
                new Vector2(1, 1),
                new Vector2(2, 2),
                new Vector2(3, 3),
                new Vector2(4, 4),
                new Vector2(5, 5),
                new Vector2(6, 6),
                new Vector2(7, 7),
            }
        },
        {
            DirectionType.DownLeft,
            new []
            {
                /* top right to bottom left */
                new Vector2(-1, -1),
                new Vector2(-2, -2),
                new Vector2(-3, -3),
                new Vector2(-4, -4),
                new Vector2(-5, -5),
                new Vector2(-6, -6),
                new Vector2(-7, -7),
            }
        },
        {
            DirectionType.DownRight,
            new []
            {
                /* top left to bottom right */
                new Vector2(1, -1),
                new Vector2(2, -2),
                new Vector2(3, -3),
                new Vector2(4, -4),
                new Vector2(5, -5),
                new Vector2(6, -6),
                new Vector2(7, -7),
            }
        },
        {
            DirectionType.Up,
            new []
            {
                /* bottom to top */
                new Vector2(0, -1),
                new Vector2(0, -2),
                new Vector2(0, -3),
                new Vector2(0, -4),
                new Vector2(0, -5),
                new Vector2(0, -6),
                new Vector2(0, -7),
            }
        },
        {
            DirectionType.Down,
            new []
            {
                /* top to bottom */
                new Vector2(0, 1),
                new Vector2(0, 2),
                new Vector2(0, 3),
                new Vector2(0, 4),
                new Vector2(0, 5),
                new Vector2(0, 6),
                new Vector2(0, 7),
            }
        },
        {
            DirectionType.Left,
            new []
            {
                /* right to left */
                new Vector2(-1, 0),
                new Vector2(-2, 0),
                new Vector2(-3, 0),
                new Vector2(-4, 0),
                new Vector2(-5, 0),
                new Vector2(-6, 0),
                new Vector2(-7, 0),
            }
        },
        {
            DirectionType.Right,
            new []
            {
                /* left to right */
                new Vector2(1, 0),
                new Vector2(2, 0),
                new Vector2(3, 0),
                new Vector2(4, 0),
                new Vector2(5, 0),
                new Vector2(6, 0),
                new Vector2(7, 0),
            }
        }
    };
    public bool IsMoveValid(ChessPiece piece, Vector2 from, Vector2 to)
    {
        ChessPiece? currentKing = null;
        foreach (var p in Board.Pieces)
        {
            if (p.PieceType == Piece.King)
            {
                currentKing = p;
                break;
            }
        }

        if (piece.PieceType == Piece.King)
        {
            return false;
        }
        else
        {
            var existsInTable = false;
            var existsList = new List<Vector2>();
            foreach (var item in PieceOffsets[piece.PieceType])
            {
                var target = item + from;
                if (to == target)
                {
                    existsInTable = true;
                    existsList.Add(target);
                }
            }

            if (!existsInTable)
                return false;

            var isBlocked = false;
            foreach (var p in Board.Pieces)
            {
                var blk = false;
                foreach (var itm in existsList)
                {
                    if (p.BoardPosition == itm)
                        blk = true;
                }

                if (blk)
                    isBlocked = true;
            }

            return !isBlocked;
        }
        
        return false;
    }
}