using HtmlAgilityPack;

namespace WebApp.Extensions
{
    public static class EmbedElementExtensions
    {
		private static HtmlNode GetGalleryComponentHtmlNode(this string element)
		{
			ArgumentNullException.ThrowIfNull(element);

			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(element);
			doc.OptionFixNestedTags = true;
			doc.OptionAutoCloseOnEnd = true;

			return doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'gallery-component')]");
		}

        public static bool IsEmbedGalleryComponent(this string element)
        {
			try
			{
				return GetGalleryComponentHtmlNode(element) != null;
			}
			catch
			{
				return false;
			}
        }
    }
}
