namespace PocketResizer.Models
{
    public class Pocket
    {
        public uint Width;
        public uint Height;

        public Pocket() { Width = 5; Height = 3; }
        public Pocket(uint width, uint height)
        {
            Width = width;
            Height = height;
        }
    }
}