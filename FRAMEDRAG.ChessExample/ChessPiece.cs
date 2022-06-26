using FRAMEDRAG.Engine;
using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.ChessExample
{
    public class ChessPiece : Component
    {
        public PieceTeam Team = PieceTeam.Unknown;
        public Piece PieceType = Piece.Unknown;
        public Vector2 BoardPosition = Vector2.Zero;
        public Container PieceContainer;
        public Sprite PieceSprite;
        public PieceStatus Status = PieceStatus.Unknown;
        public ChessPiece(ChessGame engine, ChessBoard board) : base(engine)
        {
            Engine = engine;
            Board = board;
        }
        public ChessGame Engine;
        public ChessBoard Board;
        public void UpdateSprite()
        {
            var imageName = $@"{Team.ToString().ToLower()}-{PieceType.ToString().ToLower()}";
            var img = Board.ChessPieceTextures[imageName];
            var tex = new Engine.Textures.Texture(img);
            PieceSprite = new Sprite(tex)
            {
                Position = LocalPosition()
            };
            float ws = Board.ChessPieceSize / (float)tex.Width;
            float hs = Board.ChessPieceSize / (float)tex.Height;
            if (tex.Width < tex.Height)
            {
                ws = ws * (tex.Width / (float)tex.Height);
                PieceSprite.Position.X += (tex.Height - tex.Width) / 8f;
            }
            if (tex.Height < tex.Width)
            {
                hs = hs * (tex.Height / (float)tex.Width);
            }
            PieceSprite.Scale = new Vector2(ws, hs);
            PieceSprite.ZIndex = 10;
        }
        public Vector2 GlobalPosition()
        {
            var relativeSpriteLocation = LocalPosition();
            return Board.ChessContainer.Position + relativeSpriteLocation;
        }
        public Vector2 LocalPosition()
        {
            return new Vector2(
                Board.ChessPieceSize * (BoardPosition.X),
                Board.ChessPieceSize * (BoardPosition.Y));
        }
        public override void FixedUpdate(GameTime gameTime)
        {
            PieceSprite.Position = LocalPosition();

            base.FixedUpdate(gameTime);
        }
    }
}
