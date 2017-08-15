using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceMate
{
    public class Bullet : AssetBase2D
    {
        
        private Player Player { get; set; }
        public int Speed { get; set; }
        private float angle = 0.0f;

        public Bullet(ContentManager contentManager, GraphicsDeviceManager graphics, Player player) : base(contentManager, graphics)
        {
            this.Player = player;
            this.Speed = 200;
            this.Width = 5;
            this.Height = 20;
            texture = this.Content.Load<Texture2D>("laser");
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
            _position = new Vector2(player.Position.X, player.Position.Y);
            angle = 1.55f;
        }

        public override void Update(GameTime gameTime, KeyboardState state)
        {
            _position.Y -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            this.Rectangle = new Rectangle((int)_position.X, (int)_position.Y, Height, Width);
            spriteBatch.Draw(texture, destinationRectangle: this.Rectangle, effects: SpriteEffects.FlipHorizontally, origin: origin, rotation: angle);
            spriteBatch.End();
        }

    }
}
