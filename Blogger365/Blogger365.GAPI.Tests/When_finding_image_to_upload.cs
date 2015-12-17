using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blogger365;
using System.IO;

namespace Blogger365.GAPI.Tests
{
    [TestClass]
    public class When_finding_image_to_upload
    {
        private PhotoFinder pf;
        [TestInitialize]
        public void When_finding_image_to_upload_init()
        {
            pf = new PhotoFinder() { Folder = @"Photos" };
        }

        [TestMethod]
        public void Then_can_find_all_images_in_folder()
        {
            int numfound = pf.LoadAllPhotos();

            Assert.IsTrue(numfound == System.IO.Directory.GetFiles(@"Photos", "*.jpg").Length);
        }

        [TestMethod]
        public void Then_can_find_next_exif_date_in_folder()
        {
            int numfound = pf.LoadAllPhotos();
            pf.LastUpload = new FileInfo(@"Photos\Santa Ornament.jpg");
            Assert.IsTrue(pf.GetNextImage().Name == "IMG_4097.jpg");
        }

    }
}
