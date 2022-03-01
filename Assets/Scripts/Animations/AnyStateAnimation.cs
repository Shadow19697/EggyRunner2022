namespace Scripts.Animations
{
    public class AnyStateAnimation
    {
        public string Name { get; set; }
        public bool Active { get; set; }

        public AnyStateAnimation(string name)
        {
            this.Name = name;
        }
    } 
}
