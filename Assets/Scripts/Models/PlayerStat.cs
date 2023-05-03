namespace Models
{
    public class PlayerStat
    {
        public int Score = 0;
        public string Username = "Username";

        public PlayerStat()
        {
        }
        
        public PlayerStat(int score)
        {
            this.Score = score;
        }
        
    }
}