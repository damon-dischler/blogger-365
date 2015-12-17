using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogger365.GAPI.Tests
{
    public static class Mother
    {
        public static string TestImageLocation = @"photo.JPG";
        public static string TestFileLocation = @"pfeed.txt";

        // "Damon Can't Blog" Album URI
        public static Uri TestAlbumURI = new Uri("https://picasaweb.google.com/data/feed/api/user/116954792991031607913/albumid/5773757018086211889");

        // "Damon Can't Blog" Blog ID
        public static long DamonCantBlogId = 8751800378518104799;
    }
}
