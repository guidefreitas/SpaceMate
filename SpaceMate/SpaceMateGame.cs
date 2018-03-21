using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceMate
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SpaceMateGame : Game
    {
        int playerLives = 30;
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        ScoreInfo scoreInfo;
        private List<Player> Players;
        private List<Asteroid> Asteroids;
        private List<Powerup> Powerups;
        private SpriteFont font;

        private List<String> MensagensAEnviar;
        private NetworkClient networkClient;
        public String sessionUUID;
        private String clientUUID;
        Vector2 lastNetworkSendPlayerPosition;

        float networkDelay = 50f;
        float networkTimer;

        private bool isSessionHost = false;

        public SpaceMateGame(bool isHost, String sessionJoinUUID, String serverIpAddress, Int32 ServerPort)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferWidth = 800;  //Especifica a largura da tela
            graphics.PreferredBackBufferHeight = 600;   //Especifica a altura da tela
            graphics.ApplyChanges();
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            Players = new List<Player>();
            Asteroids = new List<Asteroid>();
            scoreInfo = new ScoreInfo();
            Powerups = new List<Powerup>();
            MensagensAEnviar = new List<string>();
            //Inicia a comunicação de rede com o servidor
            networkClient = new NetworkClient();
            clientUUID =  networkClient.Connect(serverIpAddress, ServerPort);
            isSessionHost = isHost;
            if (isHost)
            {
                sessionUUID = networkClient.CreateSession(sessionJoinUUID);
            }
            else
            {
                sessionUUID = networkClient.SubscribeSession(sessionJoinUUID);
            }
            
        }

        private void SpawnConnectedPlayers(String sessionUUID)
        {
            List<String> connectedClientsUUIDs = networkClient.GetSubscribers(sessionUUID);
            foreach(var cUUID in connectedClientsUUIDs)
            {
                if(cUUID != clientUUID)
                    SpawnPlayer(cUUID, cUUID, false);
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = false;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SpawnPlayer(clientUUID, "Player 1", true);
            SpawnConnectedPlayers(sessionUUID);
            font = Content.Load<SpriteFont>("MainFont");
            var asteroid = new Asteroid(this.Content, graphics);
            Asteroids.Add(asteroid);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        private void UpdateNetworkData(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            networkTimer += elapsed;
            if (networkTimer >= networkDelay)
            {

                var player = Players.Where(p => p.ClientUUID == clientUUID).FirstOrDefault();
                if (player != null)
                {
                    Vector2 playerPos = player.Position;

                    //Envia a posição do player apenas quando ele se mexer
                    if (lastNetworkSendPlayerPosition != null && 
                        (lastNetworkSendPlayerPosition.X != playerPos.X || 
                        lastNetworkSendPlayerPosition.Y != playerPos.Y))
                    {
                        networkClient.Send(String.Format($"send_data|{sessionUUID}|{clientUUID}|player_position|{playerPos.X}|{playerPos.Y}"));
                        lastNetworkSendPlayerPosition = playerPos;
                    }
                    

                }

                for(int i = 0; i < MensagensAEnviar.Count; i++)
                {
                    String msg = MensagensAEnviar[i];
                    networkClient.Send(msg);
                    MensagensAEnviar.RemoveAt(i);
                }

                networkTimer = 0;
            }


            String serverData = networkClient.Read();
            if (!String.IsNullOrEmpty(serverData))
            {
                /*
                if (serverData.StartsWith("send_data"))
                {
                    String[] data = serverData.Split('|');
                }
                */

                if (serverData.StartsWith("new_subscriber"))
                {
                    String[] data = serverData.Split('|');
                    String newSubscriberSessionUUID = data[1];
                    String newSubscriberClientUUID = data[2];
                    SpawnPlayer(newSubscriberClientUUID, "Player 2", false);
                }

                if(serverData.StartsWith("data_received"))
                {
                    String[] data = serverData.Split('|');
                    String receivedSessionUUID = data[1];
                    String senderClientUUID = data[2];
                    String command = data[3];

                    //Se os dados recebidos pela rede forem referentes a posição do player, atualiza a posição na tela.
                    if (command.Equals("player_position"))
                    {
                        float posX = Convert.ToSingle(data[4]);
                        float posY = Convert.ToSingle(data[5]);
                        var player2 = this.Players.Where(p => p.ClientUUID == senderClientUUID).FirstOrDefault();
                        if(player2 != null)
                        {
                            player2.Position = new Vector2(posX, posY);
                        }
                    }

                    if (command.Equals("created_asteroid"))
                    {
                        var asteroidUUID = data[4];
                        float asteroidX = Convert.ToSingle(data[5]);
                        int asteroidSpeed = Convert.ToInt32(data[6]);
                        var asteroid = new Asteroid(this.Content, this.graphics);
                        asteroid.UUID = asteroidUUID;
                        asteroid.Position =  new Vector2(asteroidX, 0);
                        asteroid.Speed = asteroidSpeed;
                        this.Asteroids.Add(asteroid);
                    }

                    if(command.Equals("asteroid_outofscreen") && !this.isSessionHost)
                    {
                        var asteroidUUID = data[4];
                        var asteroid = this.Asteroids.Where(m => m.UUID == asteroidUUID).FirstOrDefault();
                        if(asteroid != null)
                        {
                            this.Asteroids.Remove(asteroid);
                        }
                    }

                    if(command.Equals("asteroid_destroied_player") && !this.isSessionHost)
                    {
                        var asteroidUUID = data[4];
                        var playerUUID = data[5];
                        var asteroid = this.Asteroids.Where(m => m.UUID == asteroidUUID).FirstOrDefault();
                        if (asteroid != null)
                        {
                            this.Asteroids.Remove(asteroid);
                        }

                        var player = this.Players.Where(m => m.ClientUUID == playerUUID).FirstOrDefault();
                        if(player != null)
                        {
                            this.Players.Remove(player);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
           

            //Sai do jogo se for pressionado ESC
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            KeyboardState state = Keyboard.GetState();


            for(int pCount = 0;pCount < this.Players.Count; pCount++)
            {
                var player = this.Players[pCount];
                for(int aCount = 0;aCount < this.Asteroids.Count; aCount++)
                {
                    var asteroid = this.Asteroids[aCount];

                    //Verifica se tem algum asteroide batendo em algum Player

                    if (player.isLocal)
                    {
                        if (player.Rectangle.Intersects(asteroid.Rectangle))
                        {
                            //Se bateu, remove o player, o asteroide, atualiza o número de mortes e recria o jogador
                            /*
                            this.Players.RemoveAt(pCount);
                            this.Asteroids.RemoveAt(aCount);
                            scoreInfo.playersScoreInfo
                                     .Where(p => p.ClientUUID == player.ClientUUID)
                                     .First()
                                     .Deaths += 1;
                            SpawnPlayer(clientUUID, player.Name, true);

                            String msg = $"send_data|{sessionUUID}|{clientUUID}|asteroid_destroied_player|{asteroid.UUID}|{player.ClientUUID}";
                            MensagensAEnviar.Add(msg);
                            */
                        }
                    }
                    
                    
                }
            }


            //Chama a funcao de update de cada um dos players, bullets e asteroides.
            foreach(var player in Players)
            {
                player.Update(gameTime, state);
                for (int bCount = 0;bCount < player.Bullets.Count;bCount++)
                {
                    var bullet = player.Bullets[bCount];
                    bullet.Update(gameTime, state);
                    for(int aCount = 0; aCount < Asteroids.Count; aCount++)
                    {
                        var asteroid = Asteroids[aCount];

                        //Verifica se alguma bala está enconstando em algum asteroide
                        if (bullet.Rectangle.Intersects(asteroid.Rectangle))
                        {
                            //Se encostar, remove o asteroide, a bala e incrementa o número de asteroides destruídos
                            scoreInfo.playersScoreInfo.Where(p => p.ClientUUID == player.ClientUUID).First().AsteroidsDestroied += 1;
                            Asteroids.RemoveAt(aCount);
                            player.Bullets.RemoveAt(bCount);
                        }
                    }
                }
            }


            if (this.isSessionHost)
            {
                //Remove os asteroides que já sairam da tela da memória
                for (int i = 0; i < Asteroids.Count; i++)
                {
                    var asteroid = Asteroids[i];
                    if (asteroid.isOutOfScreen())
                    {
                        Asteroids.RemoveAt(i);
                        String msg = $"send_data|{sessionUUID}|{clientUUID}|asteroid_outofscreen|{asteroid.UUID}";
                        MensagensAEnviar.Add(msg);
                    }
                    else
                    {
                        Asteroids[i].Update(gameTime, state);
                    }
                }
            }
            else
            {
                foreach (var asteroid in Asteroids)
                {
                    asteroid.Update(gameTime, state);
                }
            }

            
            

            for (int pCount = 0; pCount < this.Players.Count; pCount++)
            {
                var player = this.Players[pCount];
                for (int aCount = 0; aCount < this.Powerups.Count; aCount++)
                {
                    var powerup = this.Powerups[aCount];

                    //Verifica se tem algum powerup batendo em algum Player
                    if (player.Rectangle.Intersects(powerup.Rectangle))
                    {
                        //Se bateu, remove o powerup
                        this.Powerups.RemoveAt(aCount);
                        player.FireRate = 10;
                    }
                }
            }

            //Update dos powerups
            for(int pCount = 0; pCount < Powerups.Count; pCount++)
            {
                var powerup = Powerups.ElementAt(pCount);
                if (powerup.isOutOfScreen())
                {
                    Powerups.RemoveAt(pCount);
                }
                else
                {
                    powerup.Update(gameTime, state);
                }
                
            }


            //Chama a rotina que verifica se deve criar um novo asteroide
            if (this.isSessionHost)
            {
                this.CreateAsteroid(gameTime);
            }

            //Cria o powerup
            this.CreatePowerup(gameTime);

            //Envia informações das posições para o servidor
            UpdateNetworkData(gameTime);

            base.Update(gameTime);
        }


        //Cria um novo player na tela
        private void SpawnPlayer(String ClientUUID, String playerName, bool isLocal)
        {
            //Busca para ver se já existe um registro de Score para esse player
            var playerScoreInfo = this.scoreInfo.playersScoreInfo.Where(p => p.ClientUUID == ClientUUID).FirstOrDefault();


            if(playerScoreInfo == null)
            {
                //Se não existir, cria um novo ScoreInfo e associa ao player criado
                playerScoreInfo = new PlayerScoreInfo();
                playerScoreInfo.ClientUUID = ClientUUID;
                this.scoreInfo.playersScoreInfo.Add(playerScoreInfo);
                var player = new Player(this.Content, graphics, ClientUUID, isLocal);
                player.ClientUUID = ClientUUID;
                player.Name = playerName;
                player.isLocal = isLocal;
                Players.Add(player);
            }
            else
            {
                //Se já existir, verifica se ainda tem vidas e cria o player.
                if(playerScoreInfo.Deaths < this.playerLives)
                {
                    var player = new Player(this.Content, graphics, ClientUUID, isLocal);
                    player.ClientUUID = ClientUUID;
                    player.Name = playerName;
                    Players.Add(player);
                }
            }

            
        }

        private void CreateAsteroid(GameTime gameTime)
        {
            
            //Cria um novo asteroide se tiverem menos que 3 asteroides na tela
            if (Asteroids.Count < 3)
            {
                var randomX = new Random().Next(0, graphics.GraphicsDevice.Viewport.Width);
                var asteroid = new Asteroid(this.Content, this.graphics);
                asteroid.Position = new Vector2(randomX, 0);
                this.Asteroids.Add(asteroid);

                //Mandar a atualização via rede
                MensagensAEnviar.Add($"send_data|{sessionUUID}|{clientUUID}|created_asteroid|{asteroid.UUID}|{asteroid.Position.X}|{asteroid.Speed}");
            }
            
            
        }

        private void CreatePowerup(GameTime gameTime)
        {
            //Cria um novo asteroide se tiverem menos que 3 asteroides na tela
            if (Powerups.Count < 1)
            {
                var randomX = new Random().Next(0, graphics.GraphicsDevice.Viewport.Width);
                var powerup = new Powerup(this.Content, this.graphics);
                powerup.Position = new Vector2(randomX, 0);
                this.Powerups.Add(powerup);
            }
        }

        //Escreve na tela o label contendo o score do jogo
        private void DrawScore()
        {
            String scoreMessage = "Score: " + this.scoreInfo.ScoreTotal();
            spriteBatch.Begin();

            spriteBatch.DrawString(font,scoreMessage, new Vector2((graphics.GraphicsDevice.Viewport.Width/2 - 20), 20), Color.White);

            spriteBatch.End();
        }

        /// <summary>
        /// É reponsável por desenhar os elementos na tela
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Limpa a tela com a cor preta
            GraphicsDevice.Clear(Color.Black);

            //Desenha o score
            DrawScore();

            //Desenha os players e as balas
            foreach (var player in Players)
            {
                player.Draw(spriteBatch);
                foreach (var bullet in player.Bullets)
                {
                    bullet.Draw(spriteBatch);
                }
            }


            //Desenha os asteroides
            foreach(var asteroid in Asteroids)
            {
                asteroid.Draw(spriteBatch);
            }

            //Desenha os powerups
            foreach (var powerup in Powerups)
            {
                powerup.Draw(spriteBatch);
            }

            base.Draw(gameTime);
        }
    }
}
