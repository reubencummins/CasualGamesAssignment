using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Data.Entity.Core.Objects.DataClasses;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using CasualGamesAssignment;

namespace Server
{
    public enum GAMESTATE { STARTING, WAITING, PLAYING, ENDING }


    public static class GameData
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
        public static Dictionary<string, ShipInfo> Players = new Dictionary<string, ShipInfo>();
        public static Random r = new Random();
        public static GAMESTATE gameState = GAMESTATE.STARTING;
        

        //hardcoded to 1440*900 screen corners, TODO: fix
        public static Vector2[] StartPositions = 
        {
            new Vector2(100, 100),
            new Vector2(100, 800),
            new Vector2(1340, 100),
            new Vector2(1340, 800)
        };

        public static string[] PlayerShips =
        {
            "ship0",
            "ship1",
            "ship2",
            "ship3"
        };
    }


    public class GameHub : Hub
    {

        public override Task OnConnected()
        {
            GameData.ConnectedIds.Add(Context.ConnectionId);
            Clients.Caller.join();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // unreliable due to inconsistant timeout
            //if (GameData.ConnectedIds.Count > 0)
            //{
            //    GameData.ConnectedIds.Remove(Context.ConnectionId);
            //    PlayerDataObject found = GameData.Players[Context.ConnectionId];
            //    GameData.playerCharacters.Push(found.textureName);
            //    GameData.Players.Remove(Context.ConnectionId);
            //    Clients.All.removeOpponent(Context.ConnectionId);
            //    GameData.gameState = GAMESTATE.STARTING;
            //}
            return base.OnDisconnected(stopCalled);
        }

        public void removeMe()
        {
            GameData.ConnectedIds.Remove(Context.ConnectionId);
            ShipInfo found = GameData.Players[Context.ConnectionId];
            GameData.Players.Remove(Context.ConnectionId);
            Clients.All.removeOpponent(Context.ConnectionId);
            GameData.gameState = GAMESTATE.STARTING;
        }

        public void updateShip(CasualGamesAssignment.ShipUpdate update)
        {
            Clients.Others.updateOpponent(update);
        }
        

        //public string getCharacterName()
        //{
        //    if (GameData.playerCharacters.Count < 1)
        //        return string.Empty;
        //    else return (GameData.playerCharacters.Pop());
        //}

        //public Vector2 getWorldBounds()
        //{
        //    return GameData.WorldBound;
        ////}

        public int getPlayerCount() { return GameData.Players.Count(); }

        public GAMESTATE getGameState() { return GameData.gameState; }

        public List<ShipInfo> getOpponents()
        {
            List<ShipInfo> opponents = new List<ShipInfo>();
            foreach (var player in GameData.Players)
            {
                if (player.Key!=Context.ConnectionId)
                {
                    opponents.Add(player.Value);
                }
            }
            return opponents;
        }

        public void join()
        {
            ShipInfo nextPlayer = Join();
            Clients.Caller.confirmJoin(nextPlayer);
        }

        public ShipInfo Join()
        {
            int playerNumber = GameData.Players.Count;
            if (GameData.gameState == GAMESTATE.PLAYING)
                return null;
            // Create a new player Data Object
            ShipInfo player = new ShipInfo(Context.ConnectionId, GameData.PlayerShips[playerNumber],GameData.StartPositions[playerNumber])
            {
                MaxHealth = 3
                //add params based on user
            };
            GameData.Players.Add(Context.ConnectionId, player);
            if (GameData.Players.Count > 1 && GameData.gameState == GAMESTATE.STARTING)
            {
                GameData.gameState = GAMESTATE.PLAYING;
                Clients.All.play();
                Clients.All.registerOpponents(GameData.Players); // Other player(s)
            }
            return player;

        }
    }
}