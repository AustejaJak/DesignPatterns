namespace BloonsProject
{
    public class Player
    {
        public Player()
        {
            Lives = 30;
            Money = 2000;
            Round = 1;
        }

        public int Round { get; set; }

        public double Money { get; set; }

        public int Lives { get; set; }
    }
}