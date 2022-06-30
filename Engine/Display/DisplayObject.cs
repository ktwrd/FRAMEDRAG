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
        public Vector2 Pivot = new Vector2(0, 0);
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
        public int ZIndex = 0;
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
                return Parent.GlobalPosition() + Position;
            return Position;
        }

        public Vector2 GlobalToLocal(Vector2 pos)
        {
            return pos - GlobalPosition();
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

            var localTransform = LocalTransform;
            var parentTransform = Parent.WorldTransform;
            var worldTransform = WorldTransform;

            localTransform[0] = _cr * Scale.X;
            localTransform[1] = -_sr * Scale.Y;
            localTransform[3] = _sr * Scale.X;
            localTransform[4] = _cr * Scale.Y;

            var px = Pivot.X;
            var py = Pivot.Y;

            var a00 = localTransform[0];
            var a01 = localTransform[1];
            var a02 = Position.X - localTransform[0] * px - py * localTransform[1];
            var a10 = localTransform[3];
            var a11 = localTransform[4];
            var a12 = Position.Y - localTransform[4] * py - px * localTransform[3];

            var b00 = parentTransform[0];
            var b01 = parentTransform[1];
            var b02 = parentTransform[2];
            var b10 = parentTransform[3];
            var b11 = parentTransform[4];
            var b12 = parentTransform[5];

            localTransform[2] = a02;
            localTransform[5] = a12;

            worldTransform[0] = b00 * a00 + b01 * a10;
            worldTransform[1] = b00 * a01 + b01 * a11;
            worldTransform[2] = b00 * a02 + b01 * a12 + b02;

            worldTransform[3] = b10 * a00 + b11 * a10;
            worldTransform[4] = b10 * a01 + b11 * a11;
            worldTransform[5] = b10 * a02 + b11 * a12 + b12;

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
