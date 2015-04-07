using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Anura
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Objects
        public Texture2D
            black,
            playerTexture,
            healthTexture,
            healthBlackTexture;
        public Texture2D[]
            tiles, projectiles, particles, objects;
        public Vector2 mousePos = new Vector2(0, 0);
        public Random random;
        public SpriteFont font;

        // Timers
        System.Timers.Timer shootTimer;

        // Game Classes
        public Dungeon dungeon;

        // Primative
        public float
            playerX,
            playerY,
            playerXV = 0,
            playerYV = 0,
            cameraDisplacementX = 0,
            cameraDisplacementY = 0,
            movementSpeed = 0.4F,
            health = 100.0F,
            healthVel = 0;
        public int
            currentLevel = 0,
            displacementX,
            displacementY,
            tileWidth = 64,
            tileHeight = 64,
            projectieWidth = 24,
            projectileHeight = 24,
            shootSpeed = 200;
        public Boolean
            canShoot = true;

        public Game()
        {
            random = new Random();

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            displacementX = graphics.PreferredBackBufferWidth / 2;
            displacementY = graphics.PreferredBackBufferHeight / 2;

            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            dungeon = new Dungeon();

            shootTimer = new System.Timers.Timer(shootSpeed);
            shootTimer.Elapsed += new ElapsedEventHandler(shootTimer_Elapsed);
        }

        void shootTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            canShoot = true;
            shootTimer.Stop();
        }
        
        protected override void LoadContent()
        {
            // Create SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // --- Load Content
            // Load tiles
            tiles = new Texture2D[2];
            for (int i = 0; i < tiles.Length; i++)
                tiles[i] = Content.Load<Texture2D>("Tiles\\" + i);
            
            // Load projectiles
            projectiles = new Texture2D[1];
            for (int i = 0; i < projectiles.Length; i++)
                projectiles[i] = Content.Load<Texture2D>("Projectiles\\" + i);

            // Load particles
            particles = new Texture2D[1];
            for (int i = 0; i < particles.Length; i++)
                particles[i] = Content.Load<Texture2D>("Particles\\" + i);

            // Load objects
            objects = new Texture2D[1];
            for (int i = 0; i < objects.Length; i++)
                objects[i] = Content.Load<Texture2D>("Objects\\" + i);

            // Load GUI
            black = Content.Load<Texture2D>("GUI\\black");
            playerTexture = Content.Load<Texture2D>("GUI\\cursor");
            healthTexture = Content.Load<Texture2D>("GUI\\health");
            healthBlackTexture = Content.Load<Texture2D>("GUI\\healthB");

            // Load font
            font = Content.Load<SpriteFont>("Fonts\\font");
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            // Allow the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            #region Update Logic
            // Update the mouse position
            mousePos.X = Mouse.GetState().X;
            mousePos.Y = Mouse.GetState().Y;

            // Increase/Decrease Health
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus)) healthVel--;
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus)) healthVel++;
            health += healthVel;
            if (healthVel < -0.5 || healthVel > 0.5) healthVel *= 0.8F; else healthVel = 0.0F;
            if (health < 0) health = 0;
            if (health > 100) health = 100;

            // Move the player
            Vector2 movement = new Vector2(0.0F, 0.0F);
            if (Keyboard.GetState().IsKeyDown(Keys.W)) movement.Y -= 1.0F;
            if (Keyboard.GetState().IsKeyDown(Keys.S)) movement.Y += 1.0F;
            if (Keyboard.GetState().IsKeyDown(Keys.A)) movement.X -= 1.0F;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) movement.X += 1.0F;
            if(movement.X != 0 || movement.Y != 0) movement.Normalize();
            movement *= movementSpeed;
            playerXV += movement.X;
            playerYV += movement.Y;
            cameraDisplacementX += playerXV * 2.5F;
            cameraDisplacementY += playerYV * 2.5F;
            playerX += playerXV;
            playerY += playerYV;
            if (playerXV < 0.5F || playerXV > 0.5F) playerXV *= 0.9F; else playerXV = 0;
            if (playerYV < 0.5F || playerYV > 0.5F) playerYV *= 0.9F; else playerYV = 0;
            fixPlayerPosition();

            // Make the player shoot
            if(canShoot)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    dungeon.getLevel(currentLevel).getProjectiles().Add(new Projectile(0, playerX,
                        playerY - (tileHeight / 2), playerXV, playerYV - 12.0F));
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    dungeon.getLevel(currentLevel).getProjectiles().Add(new Projectile(0, playerX,
                        playerY + (tileHeight / 2), playerXV, playerYV + 12.0F));
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    dungeon.getLevel(currentLevel).getProjectiles().Add(new Projectile(0, playerX - (tileWidth / 2),
                        playerY, playerXV - 12.0F, playerYV));
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    dungeon.getLevel(currentLevel).getProjectiles().Add(new Projectile(0, playerX + (tileWidth / 2),
                        playerY, playerXV + 12.0F, playerYV));

                if (Keyboard.GetState().IsKeyDown(Keys.Up)
                    || Keyboard.GetState().IsKeyDown(Keys.Down)
                    || Keyboard.GetState().IsKeyDown(Keys.Left)
                    || Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    canShoot = false;
                    shootTimer.Start();
                }
            }

            // Update the projectiles
            for (int i = 0; i < dungeon.getLevel(currentLevel).getProjectiles().Count; i++)
            {
                // Make a projectile object
                Projectile p = dungeon.getLevel(currentLevel).getProjectiles()[i];

                // Update the projectile
                p.update();

                // Remove the projectile if outside of the map
                if (p.X < 0 || p.X > dungeon.getLevel(currentLevel).width * tileWidth
                    || p.Y < 0 || p.Y > dungeon.getLevel(currentLevel).height * tileHeight)
                {
                    // Remove
                    dungeon.getLevel(currentLevel).getProjectiles().RemoveAt(i);

                    // Make the camera shake
                    // cameraDisplacementX += (random.Next(1, 20) * 1.0F) - (random.Next(1, 20) * 1.0F);
                    // cameraDisplacementY += (random.Next(1, 15) * 1.0F) - (random.Next(1, 15) * 1.0F);
                }
            }

            // Update the camera (Linear Interpolation)
            displacementX = (graphics.PreferredBackBufferWidth / 2) + (int) cameraDisplacementX;
            displacementY = (graphics.PreferredBackBufferHeight / 2) + (int)cameraDisplacementY;
            cameraDisplacementX *= 0.7F;
            cameraDisplacementY *= 0.7F;
            #endregion

            base.Update(gameTime);
        }

        public void fixPlayerPosition()
        {
            // Above player
            while (playerY < 0
                || (dungeon.getLevel(currentLevel).collides((int) playerX / tileWidth, (int) playerY / tileHeight)
                || dungeon.getLevel(currentLevel).collides((int) playerX / tileWidth, (int) (playerY + (tileHeight - 1)) / tileHeight)))
            {
                playerY++;
                playerYV = 0;
            }

            // Below player
            while (playerY > (dungeon.getLevel(currentLevel).height * tileHeight))
            {
                playerY--;
                playerYV = 0;
            }

            // Left of player
            while (playerX < 0)
            {
                playerX++;
                playerXV = 0;
            }

            // Right of player
            while (playerX > (dungeon.getLevel(currentLevel).width * tileWidth))
            {
                playerX--;
                playerXV = 0;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            // Background Colour
            GraphicsDevice.Clear(Color.Black);

            #region Drawing Logic
            // Begin the SpriteBatch
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise);

            // Draw the level
            Level l = dungeon.getLevel(currentLevel);
            for(int x = 0; x < l.width; x++)
                for (int y = 0; y < l.height; y++)
                {
                    spriteBatch.Draw(tiles[l.tiles[x, y]],
                        new Rectangle(displacementX - (int) playerX + (x * tileWidth),
                        displacementY - (int) playerY + (y * tileHeight), tileWidth, tileHeight),
                        Color.White);
                }

            /*
            // Draw Objects
            for(int i = 0; i < l.getObjects().Count; i++)
                spriteBatch.Draw(objects[l.getObjects()[i].ID],
                    new Rectangle(displacementX - (int) playerX + (int) l.getObjects()[i].X,
                        displacementY - (int) playerY + (int) l.getObjects()[i].Y,
                        ),
                        Color.White);
            */

            // Draw the player
            spriteBatch.Draw(playerTexture, new Rectangle(displacementX - (tileWidth / 2),
                displacementY - (tileHeight / 2), tileWidth, tileHeight),
                Color.White);

            /*
            // Draw Particles
            for (int i = 0; i < l.getParticles().Count; i++)
                spriteBatch.Draw(particles[l.getParticles()[i].ID],
                    new Rectangle(l.getParticles));
            */

            // Draw Projecties
            for (int i = 0; i < l.getProjectiles().Count; i++)
                spriteBatch.Draw(projectiles[l.getProjectiles()[i].ID],
                    new Rectangle(displacementX - (int) playerX + (int) l.getProjectiles()[i].X - (projectieWidth / 2),
                        displacementY - (int) playerY + (int) l.getProjectiles()[i].Y - (projectileHeight / 2),
                        projectieWidth, projectileHeight),
                        Color.White);

            // Draw the Health Bar
            spriteBatch.DrawString(font, "Health", new Vector2(50 + (int) cameraDisplacementX, 5 + (int) cameraDisplacementY), Color.White);
            spriteBatch.Draw(black, new Rectangle(45 + (int) cameraDisplacementX, 45 + (int) cameraDisplacementY, 210, 42), Color.White);
            for (int i = 0; i < 100; i++)
            {
                if(i < health)
                    spriteBatch.Draw(healthTexture, new Rectangle(50 + (i * 2) + (int) cameraDisplacementX,
                        50 + (int) cameraDisplacementY, 2, 32),
                        new Color(i * 4, i * 4, i * 4));
                else
                    spriteBatch.Draw(healthBlackTexture, new Rectangle(50 + (i * 2) + (int) cameraDisplacementX,
                        50 + (int) cameraDisplacementY, 2, 32), Color.White);
            }

            // End the SpriteBatch
            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}
