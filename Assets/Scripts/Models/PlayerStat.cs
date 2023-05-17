namespace Models
{
    public class PlayerStat
    {
        public readonly int Score = 0;
        public readonly string Username;
        public readonly uint NetId;

        public PlayerStat()
        {
            Username = "Livl Customer";
        }
        
        public PlayerStat(string username, uint netId)
        {
            Username = username;
            NetId = netId;
        }
        
        public PlayerStat(PlayerStat stat, int score)
        {
            Username = stat.Username;
            Score = score;
            NetId = stat.NetId;
        }
        
    }
    
}