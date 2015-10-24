using System.Drawing;

namespace CheapDeal.Core.Extensions
{
    public static class ImageExtenions
    {
        public static Image Resize(this Image image, Size size)
        {
            return new Bitmap(image, size);
        }

    }
}
