using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBreakout
{
    class Action
    {

        public enum cmd
        {
            PaddleUp, PaddleDown, PauseGame, ToggleDebug, PaddleAction, RestartGame, RestartLevel
        }
        public cmd command;
        public bool isNew;

        public Action(cmd c, bool n)
        {
            command = c;
            isNew = n;
        }
    }
}
