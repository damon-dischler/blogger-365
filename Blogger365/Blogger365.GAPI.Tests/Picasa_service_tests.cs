using System;
using Google.GData.Photos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Google.GData.Blogger;


namespace Blogger365.GAPI.Tests
{
    [TestClass]
    public class Picasa_service_tests
    {
        private ServiceHandle sh_picasa;
        private ServiceHandle sh_picasa_loggedin;
        private ServiceHandle sh_blogger;
        private ServiceHandle sh_blogger_loggedin;
        private PicasaManager pm;
        private BloggerManager bm;
        private readonly string AppName = "Blogger365.Tests";

        [TestInitialize]
        public void Service_login_tests_init()
        {
            sh_picasa = new ServiceHandle(new PicasaService(AppName));
            sh_picasa_loggedin = new ServiceHandle(new PicasaService(AppName));
            sh_picasa_loggedin.Login("user@gmail.com", "--password--");

            sh_blogger = new ServiceHandle(new BloggerService(AppName));
            sh_blogger_loggedin = new ServiceHandle(new BloggerService(AppName));
            sh_blogger_loggedin.Login("user@gmail.com", "--password--");


            pm = new PicasaManager();
            bm = new BloggerManager();
        }

        [TestMethod]
        public void Valid_login_credentials_returns_authtoken()
        {
            string tok = sh_picasa.Login("user@gmail.com", "--password--");
            Assert.IsNotNull(tok);

            tok = sh_blogger.Login("user@gmail.com", "--password--");
            Assert.IsNotNull(tok);

        }

        [TestMethod]
        public void Invalid_login_credentials_returns_null()
        {
            string tok = sh_picasa.Login("user@gmail.com", "password");
            Assert.IsNull(tok);

            tok = sh_blogger.Login("user@gmail.com", "password");
            Assert.IsNull(tok);
        }


        [TestMethod]
        public void Get_album_list_returns_more_than_one()
        {
            pm.Service = sh_picasa_loggedin;
            pm.GetAlbumList(Mother.TestFileLocation);

            Assert.IsTrue(pm.AlbumList.Entries.Count > 0);
        }

        [TestMethod]
        public void Upload_image_to_album_succeeds()
        {
            pm.Service = sh_picasa_loggedin;

            UploadedPhoto photo = pm.UploadImageToAlbum(Mother.TestImageLocation, Mother.TestAlbumURI);
            Assert.IsNotNull(photo);

            StringAssert.StartsWith(photo.PicasaPhoto.PhotoUri.ToString(), "https://");
            StringAssert.EndsWith(photo.PicasaPhoto.PhotoUri.ToString(), System.IO.Path.GetExtension(Mother.TestImageLocation));
        }

        [TestMethod]
        public void Create_blank_blogpost_succeeds()
        {
            bm.Service = sh_blogger_loggedin;

            Assert.IsNotNull(bm.CreateBlogPost(Mother.DamonCantBlogId));
        }

        [TestMethod]
        public void Create_blogpost_withimage_succeeds()
        {
            bm.Service = sh_blogger_loggedin;
            pm.Service = sh_picasa_loggedin;

            UploadedPhoto photo = pm.UploadImageToAlbum(Mother.TestImageLocation, Mother.TestAlbumURI);
            Assert.IsNotNull(photo);

            StringAssert.StartsWith(photo.PicasaPhoto.PhotoUri.ToString(), "https://");
            StringAssert.EndsWith(photo.PicasaPhoto.PhotoUri.ToString(), System.IO.Path.GetExtension(Mother.TestImageLocation));


            Assert.IsNotNull(bm.CreateBlogPost(Mother.DamonCantBlogId, photo));
        }

        [TestMethod]
        public void Create_blogpost_withimage_and_title_succeeds()
        {
            bm.Service = sh_blogger_loggedin;
            pm.Service = sh_picasa_loggedin;

            UploadedPhoto photo = pm.UploadImageToAlbum(Mother.TestImageLocation, Mother.TestAlbumURI);
            Assert.IsNotNull(photo);

            StringAssert.StartsWith(photo.PicasaPhoto.PhotoUri.ToString(), "https://");
            StringAssert.EndsWith(photo.PicasaPhoto.PhotoUri.ToString(), System.IO.Path.GetExtension(Mother.TestImageLocation));


            Assert.IsNotNull(bm.CreateBlogPost(Mother.DamonCantBlogId, photo, title:"Awesome Title, DUDE"));
        }

        [TestMethod]
        public void Create_blogpost_withimage_and_caption_succeeds()
        {
            bm.Service = sh_blogger_loggedin;
            pm.Service = sh_picasa_loggedin;

            UploadedPhoto photo = pm.UploadImageToAlbum(Mother.TestImageLocation, Mother.TestAlbumURI);
            Assert.IsNotNull(photo);

            StringAssert.StartsWith(photo.PicasaPhoto.PhotoUri.ToString(), "https://");
            StringAssert.EndsWith(photo.PicasaPhoto.PhotoUri.ToString(), System.IO.Path.GetExtension(Mother.TestImageLocation));


            Assert.IsNotNull(bm.CreateBlogPost(Mother.DamonCantBlogId, photo, caption: DateTime.Now.ToString()));
        }


    }
}
