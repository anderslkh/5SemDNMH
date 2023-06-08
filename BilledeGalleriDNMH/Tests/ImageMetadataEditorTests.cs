using BLL;
using Models;

namespace Tests
{
    public class ImageMetadataEditorTests
    {
        [Fact]
        public void UpdateExifDataExpectSuccess()
        {
            //Arrange
            // Specify the path to the image file in your solution folder
            string imagePath = Path.Combine(System.AppContext.BaseDirectory, "burger.jpg");

            // Read the image file into a byte array
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            //Act
            byte[] imageUpdated = ImageMetadataEditor.UpdateExifMetadata(imageBytes, "burgerwithcheese",
                "here is a burger with cheese seen", "copyright info for image of burger with cheese", new string[] {"burger", "cheese", "cheeseburger"});

            //Assert
            Assert.NotEqual(imageBytes, imageUpdated);
        }

        [Fact]
        public void UpdateExifWithNullImageExpectFailure()
        {
            //Arrange

            //Act
            void act() => ImageMetadataEditor.UpdateExifMetadata(null, "burgerwithcheese",
                "here is a burger with cheese seen", "copyright info for image of burger with cheese", new string[] { "burger", "cheese", "cheeseburger" });

            //Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void UpdateExifWithEmptyImageExpectFailure()
        {
            //Arrange

            //Act
            void act() => ImageMetadataEditor.UpdateExifMetadata(new byte[] {}, "burgerwithcheese",
                "here is a burger with cheese seen", "copyright info for image of burger with cheese", new string[] { "burger", "cheese", "cheeseburger" });

            //Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void UpdateExifWithTextFileExpectFailure()
        {
            //Arrange
            // Specify the path to the image file in your solution folder
            string textPath = Path.Combine(System.AppContext.BaseDirectory, "teksttest.txt");

            // Read the image file into a byte array
            byte[] textBytes = File.ReadAllBytes(textPath);

            //Act
            void act() => ImageMetadataEditor.UpdateExifMetadata(textBytes, "burgerwithcheese",
                "here is a burger with cheese seen", "copyright info for image of burger with cheese", new string[] { "burger", "cheese", "cheeseburger" });

            //Assert
            Assert.Throws<ArgumentException>(act);
        }
    }
}