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

        public Vector2 direction = Vector2.Zero;
        public Vector2 speed = Vector2.Zero;

        KeyboardState prevKeyboardState;


        public Ball()
        {
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
                speed.X = Math.Abs(speed.X); //bounce RIGHT!
            }

            if (position.X >= (playArea.Width - size.Width))
            {
                //collision with right side of play area
                position.X = playArea.Width - size.Width;
                speed.X = -Math.Abs(speed.X); //bounce LEFT!
            }

            if (position.Y <= playArea.Y)
            {
                //collision with top of play area
                position.Y = playArea.Y;
                speed.Y = Math.Abs(speed.Y); //bounce DOWN!
            }

            if (position.Y >= (playArea.Height - size.Height))
            {
                //collision with bottom of play area
                position.Y = playArea.Height - size.Height;
                speed.Y = -Math.Abs(speed.Y); //bounce UP!
            }
        }


        public bool CheckPaddleCollision(Rectangle paddleLocation)
        {
            bool isCollision = false;
            Rectangle ballLocation = new Rectangle((int)position.X, (int)position.Y, spriteTexture.Width, spriteTexture.Height);
            Rectangle ballLocation_prev = new Rectangle((int)position_prev.X, (int)position_prev.Y, spriteTexture.Width, spriteTexture.Height);
            if (paddleLocation.Intersects(ballLocation) && !paddleLocation.Intersects(ballLocation_prev))
            {
                isCollision = true;
                int speedbuffer = 6; // extra pixels of padding when doing collision

                //if ball is touching top of paddle
                // and if ball is moving downwards
                // and if ball was not more than half its width into paddle
                if (ballLocation.Y + ballLocation.Height + speedbuffer > paddleLocation.Y 
                         && direction.Y==MOVE_DOWN
                         && ballLocation.X < paddleLocation.X + paddleLocation.Width -ballLocation.Width/2)
                {
                    position.Y = paddleLocation.Y - size.Y;
                    speed.Y = -Math.Abs(speed.Y); //bounce UP!
                }

                    //TODO: dterming 45degree angle collsions updownleftright
                    // TODO: copy logic for the above, to the rest of the sides.

                //if ball is touching bottom of paddle, bounce ball down
                else if (ballLocation.Y > (paddleLocation.Y + paddleLocation.Height)
                         && direction.Y == MOVE_UP)
                {
                    position.Y = paddleLocation.Y + paddleLocation.Height;
                    speed.Y = Math.Abs(speed.Y); //bounce DOWN!
                }

                //if ball is touching right of paddle, bounce ball right
                else if (ballLocation.X - speedbuffer < (paddleLocation.X + paddleLocation.Width))
                {
                    position.X = paddleLocation.X + paddleLocation.Width;
                    speed.X = Math.Abs(speed.X); //bounce RIGHT!
                }

                //if ball is touching left of paddle, bounce ball left
                else
                {
                    position.X = paddleLocation.X - size.X;
                    speed.X = -Math.Abs(speed.X); // bounce LEFT  //how would this ever happen??
                }



                /*OLD COLLISION - 
                //calc new ball trajectory based upon dist-from-paddle-center
                float pMidY = paddleLocation.Y + (paddleLocation.Height / 2);
                float bMidY = ballLocation.Y + (ballLocation.Height / 2);
                float deltaY = bMidY - pMidY;
                float itFeltRightModifier = 4.5f;
                speed.Y = deltaY * itFeltRightModifier;
                speed.X *= -1;
                 * */
            }

            return isCollision;
        }

        public bool CheckBrickCollision(Vector2 brickPos, Rectangle brickSize)
        {
            Rectangle brickLocation = new Rectangle((int)brickPos.X, (int)brickPos.Y, brickSize.Width, brickSize.Height);
            Rectangle ballLocation = new Rectangle((int)position.X, (int)position.Y, spriteTexture.Width, spriteTexture.Height);

            return brickLocation.Intersects(ballLocation);
        }




        //if ball is more than 45* to side of object, bounce horizontally
        //else, ball is more 45* or more to top/bottom, so bounce vertically
        public Vector2 CollisionDetermineNewDirection(Vector2 brickPos, Rectangle brickSize)
        {
            //TODO: if ball is moving perfectly left/right or up/down
            Rectangle objLocation = new Rectangle((int)brickPos.X, (int)brickPos.Y, brickSize.Width, brickSize.Height);
            Rectangle ballLocation = new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height);
            Vector2 newDirection = Vector2.Zero;

            float distance_BallTop_objBottom = Math.Abs(objLocation.Bottom - ballLocation.Top);
            float distance_BallBottom_objTop = Math.Abs(ballLocation.Bottom - objLocation.Top);
            float distance_BallLeft_objRight = Math.Abs(objLocation.Right - ballLocation.Left);
            float distance_BallRight_objLeft = Math.Abs(ballLocation.Right - objLocation.Left);

            //Ball is moving UP, focus bottom edge and a side
            if (direction.Y == MOVE_UP)
            {
                // UP AND LEFT - focus bottom edge and right side
                if (direction.X == MOVE_LEFT)
                {
                    if (distance_BallLeft_objRight < distance_BallTop_objBottom)
                        newDirection.X = MOVE_RIGHT;  //bounce RIGHT
                    else
                        newDirection.Y = MOVE_DOWN;  //boune DOWN
                }

                // UP AND RIGHT - focus bottom edge and left side
                else
                {
                    if (distance_BallRight_objLeft < distance_BallTop_objBottom)
                        newDirection.X = MOVE_LEFT; //bounce LEFT
                    else
                        newDirection.Y = MOVE_DOWN;  //bounce DOWN
                }
            }

            // Ball is moving DOWN - focus top edge and a side
            else // if (direction.Y == MOVE_DOWN)
            {

                // DOWN AND LEFT - focus top edge and right side
                if (direction.X == MOVE_LEFT)
                {
                    if (distance_BallLeft_objRight < distance_BallBottom_objTop)
                        newDirection.X = MOVE_RIGHT;  //bounce RIGHT
                    else
                        newDirection.Y = MOVE_UP; //bouce UP
                }

                // DOWN AND RIGHT - focus top edge and left side
                else
                {
                    if (distance_BallRight_objLeft < distance_BallBottom_objTop)
                        newDirection.X = MOVE_LEFT; //bounce LEFT
                    else
                        newDirection.Y = MOVE_UP; //bounce UP
                }
            }

            //newDirection contains only one non-zero value. 
            //the non-zero value can be used to bounce the ball;
            return newDirection;
        }


        public void definePlayArea(Rectangle area)
        {
            playArea = area;
        }


    }
}
