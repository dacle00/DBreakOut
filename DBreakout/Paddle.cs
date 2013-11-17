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

        enum State
        {
            moving, slowed, paused, held, lengthened, shortened
        }
        State currentState = State.moving;

        Vector2 direction = Vector2.Zero;
        Vector2 speed = Vector2.Zero;

        KeyboardState prevKeyboardState;


        public Paddle(Rectangle areaBounds)
        {
            playArea = areaBounds;
        }




        public void LoadContent(ContentManager theContentManager)
        {
            position = new Vector2(START_POS_X, START_POS_Y);
            base.LoadContent(theContentManager, PADDLE_NAME);
        }


        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState);

            prevKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, speed, direction);
        }


        public void UpdateMovement(KeyboardState kboard)
        {

            if (currentState == State.moving)
            {
                speed = Vector2.Zero;
                direction = Vector2.Zero;

                if (kboard.IsKeyDown(Keys.Up) || kboard.IsKeyDown(Keys.W))
                {
                    if (position.Y > playArea.Y)
                    {
                        speed.Y = BASE_SPEED;
                        direction.Y = MOVE_UP;
                    }
                    else
                    {
                        position.Y = playArea.Y;
                    }
                }
                else if (kboard.IsKeyDown(Keys.Down) || kboard.IsKeyDown(Keys.S))
                {
                    if (position.Y < (playArea.Height - size.Height))
                    {
                        speed.Y = BASE_SPEED;
                        direction.Y = MOVE_DOWN;
                    }
                    else
                    {
                        position.Y = playArea.Height - size.Height;
                    }
                }


            }
        }

    }
}
