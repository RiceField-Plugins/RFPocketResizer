namespace RFPocketResizer.Models
{
    public class PocketModel
    {
        public uint Width;
        public uint Height;

        public PocketModel() { Width = 5; Height = 3; }
        public PocketModel(uint width, uint height)
        {
            Width = width;
            Height = height;
        }
    }
}