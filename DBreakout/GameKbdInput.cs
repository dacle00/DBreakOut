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
    class GameKbdInput
    {

        KeyboardState kbdState;
        KeyboardState prevKeyboardState;
        public ArrayList input;


        public GameKbdInput()
        {
            input = new ArrayList();
        }

        public void Update()
        {
            kbdState = Keyboard.GetState();

            if (!kbdState.Equals(prevKeyboardState))
                CheckGameInput(kbdState);

            prevKeyboardState = kbdState;
        }


        public void CheckGameInput(KeyboardState kboard)
        {
            ArrayList newActions = new ArrayList();

            if (kboard.IsKeyDown(Keys.F3) && prevKeyboardState.IsKeyUp(Keys.F3))
                newActions.Add(new Action(Action.cmd.ToggleDebug, true));

            if (kboard.IsKeyDown(Keys.R) && kbdState.IsKeyDown(Keys.LeftShift))
                newActions.Add(new Action(Action.cmd.RestartGame, prevKeyboardState.IsKeyUp(Keys.R)));

            if (kboard.IsKeyDown(Keys.R) && kbdState.IsKeyUp(Keys.LeftShift))
                newActions.Add(new Action(Action.cmd.RestartLevel, prevKeyboardState.IsKeyUp(Keys.R)));

            if (kboard.IsKeyDown(Keys.P))
                newActions.Add(new Action(Action.cmd.PauseGame, prevKeyboardState.IsKeyUp(Keys.P)));

            if (kboard.IsKeyDown(Keys.W))
                newActions.Add(new Action(Action.cmd.PaddleUp, prevKeyboardState.IsKeyUp(Keys.W)));

            if (kboard.IsKeyDown(Keys.S))
                newActions.Add(new Action(Action.cmd.PaddleDown, prevKeyboardState.IsKeyUp(Keys.S)));

            if (kboard.IsKeyDown(Keys.Space))
                newActions.Add(new Action(Action.cmd.PaddleAction, prevKeyboardState.IsKeyUp(Keys.Space)));

            input = newActions;        
        }


    }
}
