using FRAMEDRAG.Engine;
using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public enum PieceStatus
    {
        Unknown = -1,
        Active,
        Stolen
    }

    public class ChessBoard : Component
    {
        public Container ChessContainer;
        public Sprite BoardSprite;
        public Dictionary<string, Texture2D> ChessPieceTextures = new Dictionary<string, Texture2D>();

        public int ChessPieceSize = 32;
        public static readonly int ChessBoardSize = 8;
        public ChessBoard(ChessGame engine) : base(engine)
        {
            Engine = engine;
            ChessContainer = new Container();
            // Get list of PNG's in the Assets folder
            foreach (var name in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), @"Assets")))
            {
                if (name.EndsWith(@".png"))
                {
                    var fname = Path.GetFileNameWithoutExtension(name);
                    ChessPieceTextures.Add(fname, Texture2D.FromFile(engine.GraphicsDevice, name));
                }
            }

            var chessBoardTexture2D = new Texture2D(
                engine.GraphicsDevice,
                (ChessPieceSize * ChessBoardSize) + 1,
                (ChessPieceSize * ChessBoardSize) + 1);
            List<Color> colorList = new List<Color>();
            for (int x = 0; x < chessBoardTexture2D.Width; x++)
            {
                for (int y = 0; y < chessBoardTexture2D.Height; y++)
                {
                    if (x % ChessPieceSize * ChessBoardSize == 0 || y % ChessPieceSize * ChessBoardSize == 0)
                    {
                        colorList.Add(Color.White);
                    }
                    else
                    {
                        colorList.Add(Color.Black);
                    }
                }
            }
            chessBoardTexture2D.SetData<Color>(colorList.ToArray());

            BoardSprite = new Sprite(new Engine.Textures.Texture(chessBoardTexture2D));
            ChessContainer.AddChild(BoardSprite);

            ChessContainer.Position.X = 100;
            ChessContainer.Position.Y = 100;

            engine.Stage.AddChild(ChessContainer);

            ResetBoard();
        }
        public ChessGame Engine;
        public PieceTeam CurrentTeam = PieceTeam.White;
        private void ResetBoard()
        {
            foreach (var p in Pieces)
            {
                if (p.PieceSprite.Parent != null)
                    p.PieceSprite.Parent.RemoveChild(p.PieceSprite);
            }
            Pieces.Clear();

            for (int t = 0; t < 2; t++)
            {
                for (int i = 0; i < 8; i++)
                    AddPiece(Piece.Pawn, (PieceTeam)t, new Vector2(i, t == 0 ? 1 : 6));
                var y = t == 0 ? 0 : 7;
                for (int i = 0; i < 3; i++)
                    AddPiece(Piece.Rook, (PieceTeam)t, new Vector2(i == 0 ? 0 : 7, y));
                for (int i = 0; i < 3; i++)
                    AddPiece(Piece.Knight, (PieceTeam)t, new Vector2(i == 0 ? 1 : 6, y));
                for (int i = 0; i < 3; i++)
                    AddPiece(Piece.Bishop, (PieceTeam)t, new Vector2(i == 0 ? 2 : 5, y));
                AddPiece(Piece.Queen, (PieceTeam)t, new Vector2(3, y));
                AddPiece(Piece.King, (PieceTeam)t, new Vector2(4, y));
            }
            ChessContainer.AddChild(BoardSprite);
        }
        public List<ChessPiece> Pieces = new List<ChessPiece>();
        public ChessPiece AddPiece(Piece type, PieceTeam team, Vector2 position)
        {
            var piece = new ChessPiece(Engine, this)
            {
                BoardPosition = position,
                PieceType = type,
                Team = team,
                Status = PieceStatus.Active
            };
            piece.UpdateSprite();
            Pieces.Add(piece);
            ChessContainer.AddChild(piece.PieceSprite);
            return piece;
        }

        #region Hover Detection
        internal Vector2 HoveredTile = new Vector2(0, 0);

        internal MouseState previousMouseState;
        internal KeyboardState previousKeyboardState;
        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            var mstate = Mouse.GetState();
            var size = ChessPieceSize * ChessBoardSize;
            var cglobal = ChessContainer.GlobalPosition();
            if (previousMouseState != null)
            {
                if (mstate.Position.X > cglobal.X && mstate.Position.X < cglobal.X + size)
                    if (mstate.Position.Y > cglobal.Y && mstate.Position.Y < cglobal.Y + size)
                    {
                        HoveredTile.X = (float)Math.Floor((mstate.Position.X - cglobal.X) / ChessPieceSize);
                        HoveredTile.Y = (float)Math.Floor((mstate.Position.Y - cglobal.Y) / ChessPieceSize);
                    }
            }
            if (previousKeyboardState != null)
            {

            }

            previousKeyboardState = kstate;
            previousMouseState = mstate;

            base.Update(gameTime);
        }
        public override void FixedUpdate(GameTime gameTime)
        {
            DebugText.SetText($"Selected\n({HoveredTile.X}, {HoveredTile.Y})");
            base.FixedUpdate(gameTime);
        }
        public Text DebugText;

        #endregion
    }
}
