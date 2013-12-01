using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace DBreakout
{
    class Paddle : Sprite
    {

        const string PADDLE_NAME = "Paddle";
        const int START_POS_X = 125;
        const int START_POS_Y = 200;
        const int BASE_SPEED = 200;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        Rectangle playArea;
        public Vector2 center;
        //public bool isSticky;
        //public bool holdingBall;

        public enum State
        {
            moving, slowed, paused, held, lengthened, shortened
        }
        public State currentState = State.moving;
        public State prePauseState;

        public Vector2 direction = Vector2.Zero;
        public Vector2 speed = Vector2.Zero;
        public ArrayList paddleInput;


        public Paddle()
        {
            paddleInput = new ArrayList();
            prePauseState = State.moving;
        }


        public void LoadContent(ContentManager theContentManager)
        {
            position = new Vector2(START_POS_X, START_POS_Y);
            center = new Vector2(position.X + (size.Width / 2), position.Y + (size.Height / 2));
            base.LoadContent(theContentManager, PADDLE_NAME);
        }


        public void Update(GameTime theGameTime)
        {
            bool paddleMoved = false;
            foreach (Action a in paddleInput)
            {
                if (a.command == Action.cmd.PaddleUp)
                {
                    paddleMoved = true;
                    doPaddleUp();
                }
                else if (a.command == Action.cmd.PaddleDown)
                {
                    paddleMoved = true;
                    doPaddleDown();
                }
                else if (a.command == Action.cmd.PaddleAction)
                    doPaddleAction();
            }
            if (!paddleMoved)
                speed = Vector2.Zero;

            base.Update(theGameTime, speed, direction);
            center.Y = position.Y + (size.Height / 2); // after paddle moved.
        }


        private void doPaddleUp()
        {
            if (currentState == State.moving)
                if (position.Y > playArea.Y)
                {
                    speed.Y = BASE_SPEED;
                    direction.Y = MOVE_UP;
                }
                else
                    position.Y = playArea.Y;
        }


        private void doPaddleDown()
        {
            if (currentState == State.moving)
                if (position.Y + size.Height < (playArea.Bottom))
                {
                    speed.Y = BASE_SPEED;
                    direction.Y = MOVE_DOWN;
                }
                else
                    position.Y = playArea.Height - size.Height;
        }


        private void doPaddleAction()
        {
            if (currentState == State.paused)
            {
                //TODO:  if some action is queued up, do it.
                //releaseBall, activate a powerup, etc
            }
                
        }


        public void definePlayArea(Rectangle area)
        {
            playArea = area;
        }


    }
}
