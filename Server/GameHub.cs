using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Data.Entity.Core.Objects.DataClasses;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace Server
{
    public enum GAMESTATE { STARTING, WAITING, PLAYING, ENDING }


    public static class GameData
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
        public static Dictionary<string, PlayerDataObject> Players = new Dictionary<string, PlayerDataObject>();
        public static List<ShipInfo> ShipInfo = new List<ShipInfo>();
        public static List<ShipUpdate> ShipUpdates = new List<ShipUpdate>();

        public static Stack<string> playerCharacters = new Stack<string>(new[] { "body", "body2" });
        public static Random r = new Random();
        public static Vector2 WorldBound = new Vector2(640, 480);
        public static GAMESTATE gameState = GAMESTATE.STARTING;


    }


    public class multiGameHub : Hub
    {
        // see http://www.asp.net/signalr/overview/guide-to-the-api/hubs-api-guide-server
        // searh for static usage

        public override Task OnConnected()
        {
            GameData.ConnectedIds.Add(Context.ConnectionId);
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
            PlayerDataObject found = GameData.Players[Context.ConnectionId];
            GameData.playerCharacters.Push(found.textureName);
            GameData.Players.Remove(Context.ConnectionId);
            Clients.All.removeOpponent(Context.ConnectionId);
            GameData.gameState = GAMESTATE.STARTING;
        }

        public void playerMoved(Vector2 newPos)
        {
            Clients.Others.opponentMoved(newPos);
        }

        public void moveMe(Vector2 delta)
        {
            Clients.All.move(delta, Context.ConnectionId);

        }

        public string getCharacterName()
        {
            if (GameData.playerCharacters.Count < 1)
                return string.Empty;
            else return (GameData.playerCharacters.Pop());
        }

        public Vector2 getWorldBounds()
        {
            return GameData.WorldBound;
        }

        public int getPlayerCount() { return GameData.Players.Count(); }

        public GAMESTATE getGameState() { return GameData.gameState; }

        public PlayerDataObject getOpponent()
        {
            KeyValuePair<string, PlayerDataObject> opponentEntry = GameData.Players.Where(p => p.Key != Context.ConnectionId).FirstOrDefault();
            return opponentEntry.Value;
        }


        public PlayerDataObject Join()
        {
            // Only 2 players allowed (could change this accordingly)
            if (GameData.gameState == GAMESTATE.PLAYING)
                return null;
            // Create a new player Data Object
            PlayerDataObject player = new PlayerDataObject
            {
                ConnectionClientID = Context.ConnectionId,
                lives = 3,
                textureName = GameData.playerCharacters.Pop(),
                position = new
                        Vector2(GameData.r.Next((int)GameData.WorldBound.X),
                                GameData.r.Next((int)GameData.WorldBound.Y)),
                health = 100,
                score = 0
            };
            // Just looking at two player game to begin
            GameData.Players.Add(Context.ConnectionId, player);
            // First tme we get two players
            if (GameData.Players.Count > 1 && GameData.gameState == GAMESTATE.STARTING)
            {
                GameData.gameState = GAMESTATE.PLAYING;
                //Clients.All.play();
                //Clients.All.registerOpponents(Players); // Other player(s)
            }
            return player;

        }
    }
}