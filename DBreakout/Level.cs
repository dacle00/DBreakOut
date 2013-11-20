using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;

namespace DBreakout
{
    class Level
    {

        public ArrayList bricks;
        public int numBricks;
        public String BackgroundImage;
        public Rectangle playArea;


        //new default level
        public Level(Rectangle area)
        {
            playArea = area;
            bricks = new ArrayList();
            numBricks = 4;
            for( int i=0; i<numBricks; i++ )
            {
                Brick b = new Brick(4, Color.White);
                b.position = new Vector2(300, i*b.size.Height);
                bricks.Add(b);
            }
        }

    }
}
