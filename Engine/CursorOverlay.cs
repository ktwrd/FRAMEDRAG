﻿using FRAMEDRAG.Engine.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine
{
    public enum CursorType
    {
        Unknown=-1,
        Pointer,
        PointerLeft=0,
        PointerRight,
        Hand,
        HandGrab,
        Cell,
        Pencil,
        TextBeam,
        Hourglass,
        Pluss,
        Cross,

        ZoomIn,
        ZoomOut,


        None = int.MaxValue
    }
    public interface IEngineCursor
    {
        public Sprite Sprite { get; set; }
        public Vector2 Anchor { get; set; }
        public string ResourceFullPath { get; set; }
        public string ResourceName { get; set; }
        public CursorType Type { get; set; }
    }
    public class EngineCursor
    {
        public EngineCursor()
        {
            Anchor = new Vector2(0, 0);
            Type = (CursorType)(-1);
        }
        public Sprite Sprite { get; set; }
        public Vector2 Anchor { get; set; }
        public string ResourceFullPath { get; set; }
        public string ResourceName { get; set; }
        public CursorType Type { get; set; }

        public static EngineCursor Generate(CursorType type, EngineGame engine)
        {
            var instance = new EngineCursor();

            if (type == CursorType.Pointer)
            {
                instance.ResourceName = @"left_ptr";
            }
            instance.ResourceFullPath = $@"FRAMEDRAG.Engine.BuiltinAssets.cursor.{instance.ResourceName}.png";
            var res = Assembly.GetAssembly(typeof(EngineCursor)).GetManifestResourceStream(instance.ResourceFullPath);
            
            var texture = new Textures.Texture(Texture2D.FromStream(engine.GraphicsDevice, res));
            instance.Sprite = new Sprite(texture);

            instance.Sprite.ZIndex = int.MaxValue;
            instance.Sprite.Anchor = instance.Anchor;
            return instance;
        }
    }
    public class CursorOverlay : Component
    {
        public CursorOverlay(EngineGame engine) : base(engine)
        {
            SetCursor(EngineCursor.Generate(CursorType.Pointer, engine));
            engine.Stage.AddChild(container);
        }

        public EngineCursor? Current { get; private set; }

        private readonly Container container = new();

        public void SetCursor(EngineCursor cursor)
        {
            if (Current != null && Current.Sprite != null)
                container.RemoveChild(Current.Sprite);
            container.AddChild(cursor.Sprite);
            Current = cursor;
        }
        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            Current.Sprite.Position.X = mouseState.X;
            Current.Sprite.Position.Y = mouseState.Y;
            base.Update(gameTime);
        }
    }
}
