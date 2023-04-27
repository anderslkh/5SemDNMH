using System.Text.Json.Serialization;

namespace Models
{
    [JsonDerivedType(typeof(ImageMetadataQueryParameters), "ImageMetadata")]
    [JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
    public class BaseQueryParameters
    {
        public Guid? Id { get; set; }
    }
}
