using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace SnakeGame
{
    // Contains all our  image components of the game
    public static class Images
    {
        //Static Variables for all our images
        
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource Body = LoadImage("Body.png");
        public readonly static ImageSource Head = LoadImage("Head.png");
        public readonly static ImageSource Food = LoadImage("Food.png");
        public readonly static ImageSource DeadBody = LoadImage("DeadBody.png");
        public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");


        /*The LoadImage method is a private helper method that takes a string fileName as input 
         and returns an ImageSource object. This method loads an image file from the 
          "GameComponents" folder using a relative path, and creates a new BitmapImage object 
          with the loaded image file.*/
        private static ImageSource LoadImage (string fileName)
        {
            return new BitmapImage(new Uri($"GameComponents/{fileName}", UriKind.Relative));

        }


    }
}
