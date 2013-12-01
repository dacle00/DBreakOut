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
        const int BASE_SPEED = 100;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        Rectangle playArea;
        public bool isColliding;
        public object collidingWith;
        public float rotationVal;
        protected const int maxHeldTime = 100; //just over a second?
        protected int heldTime;

        public enum State
        {
            moving, paused, held
        }
        public State currentState = State.held;
        public State prePauseState;

        public Vector2 direction = Vector2.Zero;
        public Vector2 speed = Vector2.Zero;


        public Ball()
        {
            speed = new Vector2(300, 200);
            direction = new Vector2(MOVE_RIGHT, MOVE_DOWN);
            rotationVal = 0f;
            isColliding = false;
            collidingWith = null;
            heldTime = 0;
            prePauseState = State.moving;
        }


        public void LoadContent(ContentManager theContentManager)
        {
            position = new Vector2(START_POS_X, START_POS_Y);

            base.LoadContent(theContentManager, BALL_NAME);

        }


        public bool Update(GameTime theGameTime)
        {
            bool wallHit = false;
            if (currentState==State.moving)
            {
                UpdateMovement();
                wallHit = doWallCollision();
                base.Update(theGameTime, speed, direction);
            }
            else if (currentState==State.held)
            {
                if (heldTime == 0)
                    UpdateMovement();
                if (heldTime++ >= maxHeldTime)
                {
                    heldTime = 0;
                    isColliding = false;
                    collidingWith = null;
                    currentState = State.moving;
                }
            }
            return wallHit;
        }

        
        public void UpdateWhileHeld(GameTime theGameTime, Vector2 pdlSpeed, Vector2 pdlDirection, float pdlRightSide)
        {
            position.X = pdlRightSide;  // +1f
            base.Update(theGameTime, pdlSpeed, pdlDirection);
        }

        public void UpdateMovement()
        {
            //if moving
            if (currentState == State.moving)
            {
                if (isColliding && collidingWith.GetType() == new Brick(0, Color.White).GetType())
                    doBrickCollision();

                if (isColliding && collidingWith.GetType() == new Paddle().GetType())
                    doPaddleCollision();
            }
            else if (currentState == State.held)
            {
                //TODO:  get new dierction, if aaplicable, but do not repeat this every single frame of being held
                doPaddleCollision();
            }
            else if (currentState == State.paused)
            {
                //nothing?
            }

        }


        //used to rotate ball to face paddle center, typically
        public void RotateBallToFaceAPoint(Vector2 aPoint)
        {
            //float aCircle = MathHelper.Pi * 2;
            float rise = (position.Y + (size.Height / 2)) - aPoint.Y;
            float run = (position.X + (size.Width / 2)) - aPoint.X;
            float slope = rise / Math.Abs(run);
            //if (slope>-45 && slope<=45)
                rotationVal = slope / MathHelper.Pi * 2;

        }


        protected bool doWallCollision()
        {
            bool hit = false;
            //use playArea rectangle. stay within it.

            if (position.X <= playArea.X)
            {
                //collision with left side of play area
                position.X = playArea.X;
                direction.X = MOVE_RIGHT;
                hit = true;
            }

            if (position.X >= (playArea.Width - size.Width))
            {
                //collision with right side of play area
                position.X = playArea.Width - size.Width;
                direction.X = MOVE_LEFT;
                hit = true;
            }

            if (position.Y <= playArea.Y)
            {
                //collision with top of play area
                position.Y = playArea.Y;
                direction.Y = MOVE_DOWN;
                hit = true;
            }

            if (position.Y >= (playArea.Height - size.Height))
            {
                //collision with bottom of play area
                position.Y = playArea.Height - size.Height;
                direction.Y = MOVE_UP;
                hit = true;
            }

            return hit;
        }


        protected void doBrickCollision()
        {
            Vector2 ballNewDir = Vector2.Zero;
            Brick tmpBrk = (Brick)collidingWith;
            ballNewDir = CollisionDetermineNewDirection(tmpBrk.position, tmpBrk.size);  //sets one non-zero field in ballNewDir
            if (ballNewDir.X != 0)
            {
                direction.X = ballNewDir.X;
                if (ballNewDir.X < 0)
                    position.X = tmpBrk.position.X - size.Width;
                else if (ballNewDir.X > 0)
                    position.X = tmpBrk.position.X + tmpBrk.size.Width;
                else
                    speed = Vector2.Zero; //this should never happen!
            }
            else if (ballNewDir.Y != 0)
            {
                direction.Y = ballNewDir.Y;
                if (ballNewDir.Y < 0)
                    position.Y = tmpBrk.position.Y - size.Width;
                else if (ballNewDir.Y > 0)
                    position.Y = tmpBrk.position.Y + tmpBrk.size.Height;
                else
                    speed = Vector2.Zero; //this should never happen!
            }

        }


        public void doPaddleCollision()
        {
            Vector2 ballNewDir = Vector2.Zero;
            if (collidingWith != null)
            {
                Paddle tmpPdl = (Paddle)collidingWith;

                //detremine new ball direction and dock ball to collided object
                ballNewDir = CollisionDetermineNewDirection(tmpPdl.position, tmpPdl.size);  //sets one non-zero field in ballNewDir
                if (ballNewDir.X != 0)
                {
                    direction.X = ballNewDir.X;
                    if (ballNewDir.X < 0)
                        position.X = tmpPdl.position.X - size.Width;
                    else if (ballNewDir.X > 0)
                        position.X = tmpPdl.position.X + tmpPdl.size.Width;
                    float pMidY = tmpPdl.position.Y + (tmpPdl.size.Height / 2);
                    float bMidY = position.Y + (size.Height / 2);
                    float deltaY = bMidY - pMidY;
                    float itFeltRightModifier = 5.25f;
                    speed.Y = Math.Abs(deltaY * itFeltRightModifier);
                    if (deltaY < 0)
                        direction.Y = MOVE_UP;
                    else
                        direction.Y = MOVE_DOWN;
                }
                else if (ballNewDir.Y != 0)
                {
                    direction.Y = ballNewDir.Y;
                    if (ballNewDir.Y < 0)
                        position.Y = tmpPdl.position.Y - size.Height;
                    else if (ballNewDir.Y > 0)
                        position.Y = tmpPdl.position.Y + tmpPdl.size.Height;
                }
            }
        }



        public bool CheckPaddleCollision(Rectangle paddleLocation)
        {
            Rectangle ballLocation = new Rectangle((int)position.X, (int)position.Y, spriteTexture.Width, spriteTexture.Height);
            Rectangle ballLocation_prev = new Rectangle((int)position_prev.X, (int)position_prev.Y, spriteTexture.Width, spriteTexture.Height);

            return paddleLocation.Intersects(ballLocation);
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
