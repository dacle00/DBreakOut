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
    class Brick : Sprite
    {

        const string BRICK_NAME = "Brick";
        const int START_POS_X = 125;
        const int START_POS_Y = 200;
        int maxDamage;
        int damage;
        Rectangle playArea;

        
        enum State
        {
            breakable, invincible
        }
        State currentState = State.breakable;


        public Brick(Rectangle areaBounds)
        {
            playArea = areaBounds;
        }


        public Brick(Rectangle areaBounds, int power)
        {
            playArea = areaBounds;
            if (power == -1)
                currentState = State.invincible;
            else
            {
                currentState = State.breakable;
                maxDamage = power;
            }
            damage = 0;
        }


        public void LoadContent(ContentManager theContentManager)
        {
            position = new Vector2(START_POS_X, START_POS_Y);
            base.LoadContent(theContentManager, BRICK_NAME);
        }


        public void Update(GameTime theGameTime)
        {
            //TODO: is this needed? we don't need to update every brick, every time, do we?
            //TODO: how do i call base.Update without a speed?
            base.Update(theGameTime, Vector2.Zero, Vector2.Zero);
        }

    }
}
