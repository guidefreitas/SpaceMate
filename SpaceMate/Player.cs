using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceMate
{

    public class Player : AssetBase2D
    {
        public String ClientUUID { get; set; }
        public String Name { get; set; }
        private int Speed { get; set; }
        
        private double lastFire = 0;

        public double FireRate { get; set; }

        public bool isLocal { get; set; }
        public List<Bullet> Bullets { get; set; }
        

        public Player(ContentManager contentManager, GraphicsDeviceManager graphics, String ClientUUID, bool isLocal) : base(contentManager, graphics)
        {
            this.Bullets = new List<Bullet>();
            this.Speed = 400;
            this.FireRate = 500;
            this.ClientUUID = ClientUUID;
            this.isLocal = isLocal;
            if (isLocal)
            {
                texture = this.Content.Load<Texture2D>("player1");
            }
            else {
                texture = this.Content.Load<Texture2D>("player2");
            }
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
            _position = new Vector2(graphics.GraphicsDevice.Viewport.Width/2, graphics.GraphicsDevice.Viewport.Height - Height);
        }

        public override void Update(GameTime gameTime, KeyboardState state)
        {
            if (isLocal)
            {
                if (state.IsKeyDown(Keys.D))
                    MoveRight(gameTime);
                if (state.IsKeyDown(Keys.A))
                    MoveLeft(gameTime);
                if (state.IsKeyDown(Keys.W))
                    MoveUp(gameTime);
                if (state.IsKeyDown(Keys.S))
                    MoveDown(gameTime);
                if (state.IsKeyDown(Keys.E))
                    FireLaser(gameTime);
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            this.Rectangle = new Rectangle((int)_position.X, (int)_position.Y, Width, Height);
            spriteBatch.Draw(texture, destinationRectangle: this.Rectangle, origin: origin);
            spriteBatch.End();
            
        }

        public void MoveRight(GameTime gameTime)
        {
            if((_position.X + this.Width/2) < this.Graphics.GraphicsDevice.Viewport.Width)
            {
                _position.X += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
        }

        public void MoveLeft(GameTime gameTime)
        {
            if ((_position.X - this.Width/2) > 0)
            {
                _position.X -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void MoveUp(GameTime gameTime)
        {
            if((_position.Y - this.Height/2) > 0)
            {
                _position.Y -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void MoveDown(GameTime gameTime)
        {
            if((_position.Y + this.Height/2) < this.Graphics.GraphicsDevice.Viewport.Height)
            {
                _position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
        }

        public Bullet FireLaser(GameTime gameTime)
        {
            double now = gameTime.TotalGameTime.TotalMilliseconds;

            if ((now - lastFire) > FireRate)
            {
                Bullet bullet = new Bullet(this.Content, this.Graphics, this);
                this.Bullets.Add(bullet);
                lastFire = now;
                return bullet;
            }


            return null;
        }

    }

    
}
