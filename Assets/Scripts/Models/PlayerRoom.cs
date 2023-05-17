namespace Models
{
    public class PlayerRoom
    {
        
        public string Username;
        public uint NetId;
        public bool IsReady;
        
        public PlayerRoom(string username, uint netId)
        {
            Username = username;
            NetId = netId;
            IsReady = false;
        }
        
        public PlayerRoom()
        {
            Username = "";
            NetId = 0;
            IsReady = false;
        }
        
        public PlayerRoom(PlayerRoom player, bool isReady)
        {
            Username = player.Username;
            NetId = player.NetId;
            IsReady = isReady;
        }
        
    }
}