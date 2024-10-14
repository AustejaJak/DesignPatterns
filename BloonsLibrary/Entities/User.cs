using System.Collections.Generic;

namespace BloonsProject

{
    public class User : Player
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // Each user will have their own list of towers
        public List<Tower> Towers { get; set; } = new List<Tower>();
    }
}