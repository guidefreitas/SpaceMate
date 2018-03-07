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
    public abstract class AssetBase2D
    {

        protected int Height = 100;
        protected int Width = 80;
        protected Vector2 origin;

        public String UUID { get; set; }
        public ContentManager Content { get; set; }
        public GraphicsDeviceManager Graphics { get; set; }
        public Rectangle Rectangle { get; set; }
        protected Texture2D texture;

        public AssetBase2D(ContentManager contentManager, GraphicsDeviceManager graphics)
        {
            UUID = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
            this.Content = contentManager;
            this.Graphics = graphics;
        }

        //Posicao do elemento na tela
        protected Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        //Guarda a posicao do objeto no servidor
        protected Vector2 _serverPosition;
        public Vector2 ServerPosition
        {
            get { return _serverPosition; }
            set { _serverPosition = value; }
        }

        //Classe abstrata de Update (a classe filha precisa implementar)
        public abstract void Update(GameTime gameTime, KeyboardState state);

        //Classe abstrada de desenvo (a classe filha precisa implementar)
        public abstract void Draw(SpriteBatch spriteBatch);

        //Verifica se o asset está fora dos limites da tela
        public virtual bool isOutOfScreen()
        {
            if ((this.Position.Y + this.texture.Height/2) < 0)
                return true;

            if ((this.Position.X + this.texture.Width/2) < 0)
                return true;

            if ((this.Position.Y - this.texture.Height / 2) > Graphics.GraphicsDevice.Viewport.Height)
                return true;

            if ((this.Position.X - this.texture.Width / 2) > Graphics.GraphicsDevice.Viewport.Width)
                return true;

            return false; 
        }
    }
}
