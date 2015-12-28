using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Google.GData.Photos;
using Google.GData.Blogger;
using System.IO;


namespace Blogger365
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PicasaManager luke_pm;
        private PicasaManager owen_pm;
        private BloggerManager luke_blog;
        private BloggerManager owen_blog;
        private readonly string AppName = "Blogger365";

        private FileInfo luke_next_photo;
        private FileInfo owen_next_photo;

        public MainWindow()
        {
            //
            // TODO: Looks like I need to conver this project to use v3 of the API
            // TODO: with oAuth2. Old methods not supported. See here:
            // https://developers.google.com/api-client-library/dotnet/get_started
            //
            InitializeComponent();
            ServiceHandle sh_picasa = new ServiceHandle(new PicasaService(AppName));
            sh_picasa.Login("user@gmail.com", "--password--");

            ServiceHandle sh_blogger = new ServiceHandle(new BloggerService(AppName));
            sh_blogger.Login("user@gmail.com", "--password--");

            luke_blog = new BloggerManager() { Service = sh_blogger };
            owen_blog = new BloggerManager() { Service = sh_blogger };

            luke_pm = new PicasaManager() { Service = sh_picasa };
            owen_pm = new PicasaManager() { Service = sh_picasa };

            LoadNextImage();
            
            Owen.Source = new BitmapImage(new Uri(owen_next_photo.FullName));
            Luke.Source = new BitmapImage(new Uri(luke_next_photo.FullName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Add thing to get latest image from folder
            // Also need to generate preview
            UploadedPhoto luke_photo = luke_pm.UploadImageToAlbum(luke_next_photo.FullName, new System.Uri(Properties.Settings.Default.AlbumURILuke));
            UploadedPhoto owen_photo = owen_pm.UploadImageToAlbum(owen_next_photo.FullName, new System.Uri(Properties.Settings.Default.AlbumURIOwen));

            luke_blog.CreateBlogPost(Properties.Settings.Default.BlogIDLuke, luke_photo);
            owen_blog.CreateBlogPost(Properties.Settings.Default.BlogIDOwen, owen_photo);

            SaveLastImageInfo();
            MessageBox.Show("Done!");
        }

        private void LoadNextImage()
        {
            FileInfo owen_last_photo = new FileInfo(Properties.Settings.Default.LastImageOwen);
            FileInfo luke_last_photo = new FileInfo(Properties.Settings.Default.LastImageLuke);

            PhotoFinder finder = new PhotoFinder() { Folder = owen_last_photo.Directory.FullName, LastUpload = owen_last_photo };
            finder.LoadAllPhotos();
            owen_next_photo = finder.GetNextImage();

            finder.Folder = luke_last_photo.Directory.FullName;
            finder.LastUpload = luke_last_photo;
            finder.LoadAllPhotos();
            luke_next_photo = finder.GetNextImage();
        }

        private void SaveLastImageInfo()
        {
            Properties.Settings.Default.LastImageOwen = owen_next_photo.FullName;
            Properties.Settings.Default.LastImageLuke = luke_next_photo.FullName;
            Properties.Settings.Default.Save();
        }
    }
}
