using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRAMEDRAG.Engine.Display
{
    public class DisplayObject
    {
        public DisplayObject()
        {
            EntityID = GlobalEngine.EntityCount;
            GlobalEngine.EntityCount++;
        }

        protected ulong EntityID = 0;
        public DisplayObject Last;
        public DisplayObject First;

        public Vector2 Position = new Vector2();
        public Vector2 Scale = new Vector2(1, 1);
        public Vector2 AbsoluteScale()
        {
            return Scale * (Parent?.AbsoluteScale() ?? Vector2.One);
        }
        public Vector2 Pivot = new Vector2(0, 0);
        public Vector2 Origin = Vector2.Zero;
        public float Rotation = 0;
        public float RotationCache = 0;
        public float Alpha = 1;
        public bool Visible = true;
        // TODO Type can be Rectangle/Circle/Ellipse/Polygon
        public object? HitArea;
        public bool ButtonMode = false;
        public bool Renderable = false;
        public Container Parent;
        // TODO Stage Type
        public Stage Stage;
        public float WorldAlpha = 1;
        public bool _interactive = false;
        private int? _zIndex = null;
        public int ZIndex
        {
            get
            {
                if (Parent != null && _zIndex == null)
                    return Parent.ZIndex ;
                return _zIndex ?? 0;
            }
            set
            {
                _zIndex = value;
            }
        }
        public const int ZIndex_Max = 1048576;
        public float ZIndex_Calculated => Math.Max(0f, Math.Min(1f, ZIndex / ZIndex_Max));
        public bool Interactive
        {
            get
            {
                return this._interactive;
            }
            set
            {
                this._interactive = value;
                this.Stage.Dirty = true;
            }
        }
        public Vector2 GlobalPosition()
        {
            if (Parent != null)
                return (Parent.GlobalPosition() + Position) * AbsoluteScale();
            return Position * AbsoluteScale();
        }

        public Vector2 GlobalToLocal(Vector2 pos)
        {
            return (pos - GlobalPosition()) / AbsoluteScale();
        }
        public Vector2 ToGlobal(Vector2 pos)
        {
            var newPosition = new Vector2();

            newPosition.X = (WorldTransform[0] * pos.X) + (WorldTransform[2] * pos.Y) + WorldTransform[4];
            newPosition.Y = (WorldTransform[1] * pos.X) + (WorldTransform[3] * pos.Y) + WorldTransform[5];

            return newPosition;
        }
        /*private Graphics mask;
        public Graphics Mask
        {
            get
            {
                return this.mask;
            }
            set
            {
                mask = value;
                if (value)
                    this.AddFilter(value);
                else
                    this.RemoveFilter();
            }
        }*/
        public Matrix WorldTransform;
        public Matrix LocalTransform;
        private Color Color;
        private bool Dynamic = true;

        private float _sr = 0;
        private float _cr = 1;
        private int visibleCount = 0;
        public void UpdateTransform()
        {
            if (Rotation != RotationCache)
            {
                RotationCache = Rotation;
                _sr = (float)Math.Sin(Rotation);
                _cr = (float)Math.Cos(Rotation);
            }

            var parentTransform = Parent.WorldTransform;

            LocalTransform[0] = _cr * Scale.X; // a
            LocalTransform[1] = -_sr * Scale.Y;// b
            LocalTransform[3] = _sr * Scale.X; // d
            LocalTransform[4] = _cr * Scale.Y; // tx

            var px = Pivot.X;
            var py = Pivot.Y;

            var a00 = LocalTransform[0];
            var a01 = LocalTransform[1];
            var a02 = Position.X - LocalTransform[0] * px - py * LocalTransform[1];
            var a10 = LocalTransform[3];
            var a11 = LocalTransform[4];
            var a12 = Position.Y - LocalTransform[4] * py - px * LocalTransform[3];

            var b00 = parentTransform[0];
            var b01 = parentTransform[1];
            var b02 = parentTransform[2];
            var b10 = parentTransform[3];
            var b11 = parentTransform[4];
            var b12 = parentTransform[5];

            LocalTransform[2] = a02; // c
            LocalTransform[5] = a12; // ty

            WorldTransform[0] = b00 * a00 + b01 * a10;
            WorldTransform[1] = b00 * a01 + b01 * a11;
            WorldTransform[2] = b00 * a02 + b01 * a12 + b02;

            WorldTransform[3] = b10 * a00 + b11 * a10;
            WorldTransform[4] = b10 * a01 + b11 * a11;
            WorldTransform[5] = b10 * a02 + b11 * a12 + b12;

            WorldAlpha = Alpha * Parent.WorldAlpha;

            visibleCount = 0;
        }

        internal DisplayObject _iNext;
        internal DisplayObject _iPrev;

        public virtual void Draw(SpriteBatch spriteBatch, EngineGame engine)
        { }
        public virtual void Update(GameTime gameTime, EngineGame engine)
        { }
    }
}
