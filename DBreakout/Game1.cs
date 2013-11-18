using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DBreakout
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Sprite background;
        Sprite brickAreaBackground;
        Sprite playAreaBackground;

        Rectangle playArea;
        const int BUFFER_TOP = 5;
        const int BUFFER_BOTTOM = 5;
        const int BUFFER_LEFT = 5;
        const int BUFFER_RIGHT = 5;

        Paddle paddle; 
        const int PADDLE_X = 20;

        Ball ball;

        Rectangle brickArea;
        const int BRICK_BUFFER_TOP = 40;
        const int BRICK_BUFFER_BOTTOM = 40;
        const int BRICK_BUFFER_LEFT = 200;
        const int BRICK_BUFFER_RIGHT = 40;
        //TODO:  in the LoadContent, or somewhere, have a function to fill the brickArea with bricks, random power,
        Brick[] bricks;
        Brick aBrick;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            playArea = new Rectangle(BUFFER_LEFT, BUFFER_TOP, graphics.PreferredBackBufferWidth - (BUFFER_LEFT + BUFFER_RIGHT), graphics.PreferredBackBufferHeight - (BUFFER_TOP + BUFFER_BOTTOM));
            brickArea = new Rectangle(playArea.X + BRICK_BUFFER_LEFT, playArea.Y + BRICK_BUFFER_TOP, playArea.Width - (BRICK_BUFFER_LEFT + BRICK_BUFFER_RIGHT), playArea.Height - (BRICK_BUFFER_TOP + BRICK_BUFFER_BOTTOM));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            background = new Sprite("MetalB");
            playAreaBackground = new Sprite("SolidTanBG");
            brickAreaBackground = new Sprite("SolidBrownBG");
            paddle = new Paddle(playArea);
            ball = new Ball(playArea);
            aBrick = new Brick(playArea, 3);

            base.Initialize();

            background.position = Vector2.Zero;
            playAreaBackground.position = new Vector2(playArea.X, playArea.Y);
            playAreaBackground.size = playArea; //TODO: not working
            brickAreaBackground.position = new Vector2(brickArea.X, brickArea.Y);
            brickAreaBackground.size = brickArea;  //TODO: not working


            paddle.position = new Vector2(playArea.X + PADDLE_X, (playArea.Y + playArea.Height / 2) - (paddle.size.Height / 2));
            ball.position = new Vector2(playArea.X + playArea.Width / 2, playArea.X + playArea.Height / 2);
            aBrick.position = new Vector2(playArea.X + playArea.Width / 4, playArea.X + playArea.Height / 4);
        }

        /// <summary>``````````````````````````
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playArea.Width = GraphicsDevice.Viewport.Width;
            playArea.Height = GraphicsDevice.Viewport.Height;

            background.LoadContent(this.Content, "SolidTanBG");
            playAreaBackground.LoadContent(this.Content, "MetalBG");
            brickAreaBackground.LoadContent(this.Content, "SolidBrownBG");
            paddle.LoadContent(this.Content, "Paddle");
            ball.LoadContent(this.Content, "Ball2");
            aBrick.LoadContent(this.Content, "Brick");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            paddle.Update(gameTime);
            ball.Update(gameTime);
            
            //CHECK BALL HIT PADDLE
            bool paddleHit = ball.CheckPaddleCollision(new Rectangle((int)paddle.position.X, (int)paddle.position.Y, paddle.size.Width, paddle.size.Height));
            if(paddleHit)
            {
                //play sound
                //sticky?
                //accel?
            }

            // Check BALL HIT BRICK
            /*
            bool brickHit = ball.CheckBrickCollision(aBrick);
            if (brickHit)
            {
                //play sound
                //sticky?
                //accel?
            }
            */

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            background.Draw(this.spriteBatch);
            playAreaBackground.Draw(this.spriteBatch, playArea);
            brickAreaBackground.Draw(this.spriteBatch, brickArea);
            paddle.Draw(this.spriteBatch);
            ball.Draw(this.spriteBatch);
            aBrick.Draw(this.spriteBatch);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
