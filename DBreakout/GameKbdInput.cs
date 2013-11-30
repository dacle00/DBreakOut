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
    class GameKbdInput
    {

        KeyboardState kbdState;
        KeyboardState prevKeyboardState;


        public String Update(GameTime gameTime)
        {
            kbdState = Keyboard.GetState();

            String msg = "";
            
            
            if (!kbdState.Equals(prevKeyboardState))
                msg = CheckGameInput(kbdState);


            prevKeyboardState = kbdState;
            return msg;

        }


        public String CheckGameInput(KeyboardState kboard)
        {
            String msg = "";

            if (kboard.IsKeyDown(Keys.F3))
            {
                msg = "toggle debug";
            }
            else if (kboard.IsKeyDown(Keys.R))
            {
                msg = "restart game";
            }
            else if (kboard.IsKeyDown(Keys.R) && (kboard.IsKeyDown(Keys.LeftShift) || kboard.IsKeyDown(Keys.RightShift)))
            {
                msg = "restart level";
            }
            return msg;
        }
    }
}
