using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DBreakout
{
    class Sprite
    {


        //Details
        protected string name;
        public Rectangle size;
        public Vector2 position, position_prev;
        public Vector2 fontPosition;
        protected Texture2D spriteTexture;
        protected float scale;
        Color color;
        SpriteFont debugFont;
        Color debugColor = Color.Orange;

        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                size = new Rectangle(0, 0, (int)(spriteTexture.Width * Scale), (int)(spriteTexture.Height * Scale));
            }
        }



        public Sprite()
        {
            position = new Vector2(0, 0);
            position_prev = new Vector2(0, 0);
            fontPosition = new Vector2(0, -30);
            name = "newSprite";
            scale = 1f;
        }

        public Sprite(string assetName)
        {
            position = new Vector2(0, 0);
            position_prev = new Vector2(0, 0);
            fontPosition = new Vector2(0, -30);
            name = assetName;
            scale = 1f;
        }

        public Sprite(string assetName, Color c)
        {
            position = new Vector2(0, 0);
            position_prev = new Vector2(0, 0);
            fontPosition = new Vector2(0, -30);
            name = assetName;
            scale = 1f;
            color = c;
        }



        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            spriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            size = new Rectangle(0, 0, (int)(spriteTexture.Width * scale), (int)(spriteTexture.Height * scale));
            debugFont = theContentManager.Load<SpriteFont>("fnt");

        }


        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            position_prev = position;
            position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            fontPosition = new Vector2(position.X, position.Y - 30);
        }

        
        //Draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch, float slope = 0f, String strDebug = "")
        {

            Vector2 origin = Vector2.Zero;
            // optional slope is used by rotating ball
            if (slope != 0)
            {
                Rectangle objLoc = new Rectangle(0, 0, size.Width, size.Height);
                //origin = new Vector2(objLoc.Center.X, objLoc.Center.Y);
                origin = new Vector2(size.Width / 2, size.Height / 2);
            }
            Rectangle tmpRec = new Rectangle(0, 0, spriteTexture.Width, spriteTexture.Height);
            theSpriteBatch.Draw(spriteTexture, position+origin, tmpRec, Color.White, slope, origin, scale, SpriteEffects.None, 0);

            if (strDebug != null)
            {
                theSpriteBatch.DrawString(debugFont, strDebug, position + fontPosition, debugColor);
            }

        }


        // overloaded the draw method, to scale a BG image to be the size of specified rectangle
        public void Draw(SpriteBatch theSpriteBatch, Rectangle restrict, String strDebug = "")
        {
            Rectangle tmpRec = new Rectangle(0, 0, restrict.Width, restrict.Height);
            theSpriteBatch.Draw(spriteTexture, position, tmpRec, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);

            if (strDebug != null)
            {
                theSpriteBatch.DrawString(debugFont, strDebug, position + fontPosition, debugColor);
            }
        }

        // overloaded the draw method, to colorize sprites
        public void Draw(SpriteBatch theSpriteBatch, Color c, String strDebug = "")
        {
            Rectangle tmpRec = new Rectangle(0, 0, spriteTexture.Width, spriteTexture.Height);
            theSpriteBatch.Draw(spriteTexture, position, tmpRec, c, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);

            if (strDebug != null)
            {
                theSpriteBatch.DrawString(debugFont, strDebug, position + fontPosition, debugColor);
            }
        }

    }
}
