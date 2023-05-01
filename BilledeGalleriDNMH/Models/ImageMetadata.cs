namespace Models
{
    public class ImageMetadata : Entity
    {
        public byte[] Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public string Location { get; set; }
        public string CameraInformation { get; set; }
        public string CopyrightInformation { get; set; }
        public string[] Keywords { get; set; }
    }

    public class ImageMetadataQueryParameters : BaseQueryParameters
    {
        public byte[]? Image { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Location { get; set; }
        public string? CameraInformation { get; set; }
        public string? CopyrightInformation { get; set; }
        public string[]? Keywords { get; set; }
    } 
}
