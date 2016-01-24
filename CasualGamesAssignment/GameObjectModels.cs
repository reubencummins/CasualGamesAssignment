using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualGamesAssignment
{
    public class PlayerProfile
        {
        public string ID;
        public string Email;
        public string UserName;
        }


    public class GameScoreObject
    {
        public string PlayerID { get; set; }

        public int Score { get; set; }

    }


}
