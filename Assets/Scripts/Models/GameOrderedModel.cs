namespace Scripts.Models
{
    public class GameOrderedModel
    {
        public int position;
        public GameModel game;

        public GameOrderedModel(int _position, GameModel _game)
        {
            this.position = _position;
            this.game = _game;
        }
    } 
}
