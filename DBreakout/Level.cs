using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace DBreakout
{
    class Level
    {

        //public String BackgroundImage;
        //public Rectangle playArea;
        public Brick[] bricks;
        public int numBricks;
        Color[] colors = { Color.Wheat, Color.Chartreuse, Color.Beige, Color.DarkSalmon, Color.Gainsboro };
        public Rectangle brickArea;


        public Level()
        {
            brickArea = new Rectangle(100,100,400,400);
            numBricks = 5;
            bricks = new Brick[numBricks];

            for (int i = 0; i < numBricks; i++)
            {
                Random rnd = new Random(DateTime.Now.Second);
                int r = rnd.Next(1, 3);  //brick power
                bricks[i] = new Brick(r, colors[i]);
                bricks[i].position = new Vector2(brickArea.X + brickArea.Width - (numBricks * bricks[i].size.Width) + (i * bricks[i].size.Width), brickArea.Y + (i * bricks[i].size.Height));
            }


        }

        //new default level
        public Level(Rectangle area, int levelNumber)
        {
            brickArea = area;
            switch (levelNumber)
            {
                case 1:
                    buildLevel1();
                    break;
                case 2:
                    buildLevel2();
                    break;
                default:
                    buildLevel0();
                    break;
            }

        }


        // one brick in the ceter.
        private void buildLevel0()
        {
            numBricks = 1;
            bricks = new Brick[numBricks];
            for (int i = 0; i < numBricks; i++)
            {
                Random rnd = new Random();
                int r = rnd.Next(1, 3);
                bricks[i] = new Brick(r, colors[i]);
                bricks[i].position = new Vector2(brickArea.Left + (brickArea.Width / 2), brickArea.Top + (brickArea.Height / 2));
            }
        }


        // a diagonal line of bricks, top left to bottom right
        private void buildLevel1()
        {
            numBricks = 5;
            bricks = new Brick[numBricks];
            for (int i = 0; i < numBricks; i++)
            {
                Random rnd = new Random();
                int r = rnd.Next(1, 3);
                bricks[i] = new Brick(r, colors[i]);
                bricks[i].position = new Vector2(brickArea.X + brickArea.Width - (numBricks * bricks[i].size.Width) + (i * bricks[i].size.Width), brickArea.Y + (i * bricks[i].size.Height));
            }
        }

        // a diagonal block of bricks, top left to bottom right
        //TODO: why is brickArea still ZEROs for this level, but not for other levels?
        private void buildLevel2()
        {
            int rows = 5;
            int cols = 5;
            numBricks = rows * cols;
            int c = 0;
            bricks = new Brick[numBricks];
            for (int j = 0; j < cols; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    Random rnd = new Random();
                    int r = rnd.Next(1, 3);
                    bricks[c] = new Brick(r, colors[i]);
                    bricks[c++].position = new Vector2(brickArea.X + brickArea.Width - (i * bricks[i].size.Width) + (i * bricks[i].size.Width), brickArea.Y + (j * bricks[i].size.Height));
                }
            }
        }



        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < numBricks; i++)
            {
                bricks[i].Update(gameTime);
            }
        }


        public void defineBrickArea(Rectangle area)
        {
            brickArea = area;

            for (int i = 0; i < numBricks; i++)
            {
                bricks[i].position = new Vector2(brickArea.X + brickArea.Width - (numBricks * bricks[i].size.Width) + (i * bricks[i].size.Width), brickArea.Y + (i * bricks[i].size.Height));
            }


        }


    }
}
