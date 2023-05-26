namespace Models
{
    public class Gallery : Entity
    {
        public string Name { get; set; }
        public List<ImageObject> ImageObjects { get; set; }
    }
}
