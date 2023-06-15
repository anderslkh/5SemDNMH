namespace Models
{
    public class Gallery : Entity
    {
        public string GalleryName { get; set; }
        public List<string> ImageIds { get; set; }
    }
}
