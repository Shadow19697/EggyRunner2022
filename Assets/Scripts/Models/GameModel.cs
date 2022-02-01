namespace Scripts.Models
{
    public class GameModel
    {
        public string name;
        public int score;
        public int level;

        public GameModel(string _name, int _score, int _level)
        {
            name = _name;
            score = _score;
            level = _level;
        }
    } 
}
