﻿using FRAMEDRAG.Engine;
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
        }
        public ChessGame Engine;
        public PieceTeam CurrentTeam = PieceTeam.White;
        private void ResetBoard()
        {
            
        }
    }
}
