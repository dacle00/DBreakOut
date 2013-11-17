using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DBreakout
{
    class Ball : Sprite
    {

        const string BALL_NAME = "Ball";
        const int START_POS_X = 125;
        const int START_POS_Y = 200;
        const int BASE_SPEED = 900;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        Rectangle playArea;

        enum State
        {
            moving, paused, held
        }
        State currentState = State.moving;

        Vector2 direction = Vector2.Zero;
        Vector2 speed = Vector2.Zero;

        KeyboardState prevKeyboardState;


        public Ball(Rectangle areaBounds)
        {
            playArea = areaBounds;
            speed = new Vector2(300, 100);
            direction = new Vector2(MOVE_RIGHT, MOVE_DOWN);
        }


        public void LoadContent(ContentManager theContentManager)
        {
            position = new Vector2(START_POS_X, START_POS_Y);
            base.LoadContent(theContentManager, BALL_NAME);
        }


        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement();
            CheckWallCollision();

            prevKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, speed, direction);
        }
        

        public void UpdateMovement()
        {

            //if puased  (State.paused)
            //speed = Vector2.Zero;
            //direction = Vector2.Zero;


            //if held by sticky paddle (State.held)
            //speed = Vector2.Zero;
            //direction = Vector2.Zero;


            //if moving
            if (currentState == State.moving)
            {

            }
        }


        public void CheckWallCollision()
        {
            //use playArea rectangle. stay within it.

            if (position.X <= playArea.X)
            {
                //collision with left side of play area
                position.X = playArea.X;
                speed.X *=-1;
            }

            if (position.X >= (playArea.Width - base.spriteTexture.Width))
            {
                //collision with right side of play area
                position.X = playArea.Width-base.spriteTexture.Width;
                speed.X *=-1;
            }

            if (position.Y <= playArea.Y)
            {
                //collision with top of play area
                position.Y = playArea.Y;
                speed.Y *=-1;
            }

            if (position.Y  >= (playArea.Height-base.spriteTexture.Height))
            {
                //collision with bottom of play area
                position.Y = playArea.Height - base.spriteTexture.Height;
                speed.Y *=-1;
            }
        }


        public bool CheckPaddleCollision(Rectangle paddleLocation)
        {
            bool isCollision = false;
            Rectangle ballLocation = new Rectangle((int)position.X, (int)position.Y, spriteTexture.Width, spriteTexture.Height);

            if (paddleLocation.Intersects(ballLocation))
            {
                isCollision = true;
                //calc new ball trajectory based upon dist-from-paddle-center
                float pMidY = paddleLocation.Y + (paddleLocation.Height / 2);
                float bMidY = ballLocation.Y + (ballLocation.Height / 2);
                float deltaY = bMidY - pMidY;
                float itFeltRightModifier = 4.5f;
                speed.Y = deltaY * itFeltRightModifier;
                speed.X *= -1;
            }

            return isCollision;
        }



    }
}
