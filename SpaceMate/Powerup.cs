using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SpaceMate
{
    public class Powerup : AssetBase2D
    {
        private float Speed = 30;
        private float angle = 0.01f;
        public Powerup(ContentManager contentManager, GraphicsDeviceManager graphics):base(contentManager, graphics)
        {
            this.Speed = 30;
            texture = this.Content.Load<Texture2D>("powerup");
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
            _position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            this.Rectangle = new Rectangle((int)_position.X, (int)_position.Y, Width, Height);
            spriteBatch.Draw(texture, position: new Vector2(_position.X, _position.Y), origin: origin, rotation: angle);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime, KeyboardState state)
        {
            angle += 0.02f;
            _position.Y += this.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
