using System.Runtime.InteropServices;

namespace Plugins
{
    public static class iOSCall
    {
        [DllImport("__Internal")]
        public static extern void openPhotoLibrary();
        
        // [DllImport("__Internal")]
        // public static extern void didSelectImage();
    }
}