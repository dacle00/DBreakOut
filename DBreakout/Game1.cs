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
using System.Collections;

namespace DBreakout
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameKbdInput GameInput;

        Sprite background;
        Sprite brickAreaBackground;
        Sprite playAreaBackground;

        SoundEffect sndBrickPop;
        SoundEffect sndBallHitBrick;
        SoundEffect sndBallHitPaddle;
        SoundEffect sndBallHitSide;

        Rectangle playArea;
        const int BUFFER_TOP = 0;
        const int BUFFER_BOTTOM = 0;
        const int BUFFER_LEFT = 0;
        const int BUFFER_RIGHT = 0;

        Paddle paddle; 
        const int PADDLE_X = 20;

        Ball ball;

        Rectangle brickArea;
        const int BRICK_BUFFER_TOP = 40;
        const int BRICK_BUFFER_BOTTOM = 40;
        const int BRICK_BUFFER_LEFT = 200;
        const int BRICK_BUFFER_RIGHT = 40;
        Level currentLevel;
        static int levelNum=-1;
        bool showDebug;
        bool gameIsPaused;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            background = new Sprite("image/MetalB");
            playAreaBackground = new Sprite("image/SolidTanBG");
            brickAreaBackground = new Sprite("image/SolidBrownBG");
            paddle = new Paddle();
            ball = new Ball();
            currentLevel = new Level();
            levelNum++;
            if (levelNum<2)
                levelNum=0;
            showDebug = false;
            gameIsPaused = false;
            GameInput = new GameKbdInput();

            base.Initialize();

            paddle.definePlayArea(playArea);
            ball.definePlayArea(playArea);
            //currentLevel.defineBrickArea(brickArea);

            background.position = Vector2.Zero;
            playAreaBackground.position = new Vector2(playArea.X, playArea.Y);
            playAreaBackground.size = playArea; //TODO: not working
            brickAreaBackground.position = new Vector2(brickArea.X, brickArea.Y);
            brickAreaBackground.size = brickArea;  //TODO: not working

            paddle.position = new Vector2(playArea.X + PADDLE_X, (playArea.Y + playArea.Height / 2) - (paddle.size.Height / 2));
            ball.position = new Vector2(paddle.position.X + ball.size.Width +paddle.size.Width, paddle.position.Y + (paddle.size.Height / 2));


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
            playArea = new Rectangle(BUFFER_LEFT, BUFFER_TOP, graphics.PreferredBackBufferWidth - (BUFFER_LEFT + BUFFER_RIGHT), graphics.PreferredBackBufferHeight - (BUFFER_TOP + BUFFER_BOTTOM));
            brickArea = new Rectangle(playArea.X + BRICK_BUFFER_LEFT, playArea.Y + BRICK_BUFFER_TOP, playArea.Width - (BRICK_BUFFER_LEFT + BRICK_BUFFER_RIGHT), playArea.Height - (BRICK_BUFFER_TOP + BRICK_BUFFER_BOTTOM));

            background.LoadContent(this.Content, "image/SolidTanBG");
            playAreaBackground.LoadContent(this.Content, "image/MetalBG");
            brickAreaBackground.LoadContent(this.Content, "image/SolidBrownBG");
            paddle.LoadContent(this.Content, "image/Paddle");
            ball.LoadContent(this.Content, "image/Ball3");

            currentLevel = new Level(brickArea, levelNum); // level 1, 2, or default to level 0
            for (int i = 0; i < currentLevel.numBricks; i++)
            {
                currentLevel.bricks[i].LoadContent(this.Content, "image/Brick2");
            }

            sndBrickPop = Content.Load<SoundEffect>("sound/pop");
            sndBallHitBrick = Content.Load<SoundEffect>("sound/flop");
            sndBallHitPaddle = Content.Load<SoundEffect>("sound/pop2");
            sndBallHitSide= Content.Load<SoundEffect>("sound/tooo");

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

            //sort through all current inputs
            GameInput.Update();
            paddle.paddleInput = new ArrayList();
            foreach (Action a in GameInput.input)
            {
                if (a.command.ToString().Contains("Paddle"))
                    paddle.paddleInput.Add(a);
                else if (a.command == Action.cmd.ToggleDebug && a.isNew)
                    showDebug = !showDebug;
                else if (a.command == Action.cmd.PauseGame && a.isNew)
                {
                    gameIsPaused = !gameIsPaused;
                    if (gameIsPaused)
                    {
                        paddle.currentState = Paddle.State.paused;
                        ball.currentState = Ball.State.paused;
                    }
                    else
                    {
                        paddle.currentState = paddle.prePauseState;
                        ball.currentState = ball.prePauseState;
                    }
                    //TODO: handle all-game inputs here, like pause, restart, toggleDebug, etc.
                }
            }


            paddle.Update(gameTime);
            if (ball.Update(gameTime))
                sndBallHitSide.Play();

            if (ball.currentState == Ball.State.held)
                ball.UpdateWhileHeld(gameTime, paddle.speed, paddle.direction, paddle.position.X + paddle.size.Width);
            ball.RotateBallToFaceAPoint(paddle.center);
            currentLevel.Update(gameTime);

            int hitBrick = -1;
            int brokenBricks = 0;
            ball.isColliding = false;
            ball.collidingWith = null;
            ///////////////////////////////
            // Collision: BALL HIT BRICK //
            for (int i = 0; i < currentLevel.numBricks; i++)
            {
                //ignore broken bricks
                if (currentLevel.bricks[i].currentState != Brick.State.broken)
                {
                    //currentLevel.bricks[i].Update(gameTime); // TODO: is this needed? bricks normally don't change 
                    if (ball.CheckBrickCollision(currentLevel.bricks[i].position, currentLevel.bricks[i].size))
                    {
                        hitBrick = i;
                        ball.isColliding = true;
                        ball.collidingWith = (Brick)currentLevel.bricks[i];

                        //damage the collided brick
                        if (currentLevel.bricks[i].currentState != Brick.State.invincible)
                            if (currentLevel.bricks[i].damage++ >= currentLevel.bricks[i].maxDamage)
                            {
                                currentLevel.bricks[i].currentState = Brick.State.broken;
                                sndBrickPop.Play(); //play pop sound
                            }
                            else
                                sndBallHitBrick.Play();
                    }
                }
                else
                    brokenBricks++;
                if (brokenBricks >= currentLevel.bricks.Length)
                {
                    //winning conditions
                    ball.currentState = Ball.State.paused;
                    paddle.currentState = Paddle.State.paused;

                    Initialize();
                }
            }



            ////////////////////////////////
            // Collision: BALL HIT PADDLE //
            if (ball.CheckPaddleCollision(new Rectangle((int)paddle.position.X, (int)paddle.position.Y, paddle.size.Width, paddle.size.Height)))
            {
                ball.isColliding = true;
                ball.collidingWith = (Paddle)paddle;
                //ball.setState(Ball.State.held);
                sndBallHitPaddle.Play();

                //sticky powerup?
                //accel ball?

            }

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
            {
                background.Draw(this.spriteBatch);
                playAreaBackground.Draw(this.spriteBatch, playArea);
                //brickAreaBackground.Draw(this.spriteBatch, currentLevel.brickArea);
                if (showDebug)
                {
                    paddle.Draw(this.spriteBatch, 0f, "paddle");
                    ball.Draw(this.spriteBatch, ball.rotationVal, "ball");
                }
                else
                {
                    ball.Draw(this.spriteBatch, ball.rotationVal);
                    paddle.Draw(this.spriteBatch);
                }

                for (int i = 0; i < currentLevel.numBricks; i++)
                {
                    if (currentLevel.bricks[i].currentState != Brick.State.broken)
                        if (showDebug)
                            currentLevel.bricks[i].Draw(this.spriteBatch, currentLevel.bricks[i].color, (currentLevel.bricks[i].maxDamage - currentLevel.bricks[i].damage + 1).ToString());
                        else
                            currentLevel.bricks[i].Draw(this.spriteBatch, currentLevel.bricks[i].color);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
