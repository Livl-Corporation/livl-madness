namespace Models
{
    public class PlayerStat
    {
        public readonly int Score = 0;
        public readonly string Username;

        public PlayerStat()
        {
            Username = "Livl Customer";
        }
        
        public PlayerStat(string username)
        {
            Username = username;
        }
        
        public PlayerStat(PlayerStat stat, int score)
        {
            Username = stat.Username;
            Score = score;
        }
        
    }
    
}