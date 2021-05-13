using System.Xml.Serialization;

namespace PocketResizer.Models
{
    public class Pocket
    {
        [XmlElement("Width")]
        public uint Width;
        [XmlElement("Height")]
        public uint Height;

        public Pocket() { Width = 5; Height = 3; }
        public Pocket(uint width, uint height)
        {
            Width = width;
            Height = height;
        }
    }
}