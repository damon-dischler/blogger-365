using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.GData.Photos;
using Google.Picasa;

namespace Blogger365
{
    public class PicasaManager
    {
        public ServiceHandle Service { get; set; }

        private PicasaFeed pFeed;

        public PicasaFeed AlbumList
        {
            get { return pFeed; }
            private set { pFeed = value; }
        }
        
        public void GetAlbumList(string outFileLoc = null)
        {
            AlbumQuery query = new AlbumQuery();
            AlbumList = null;

            query.Uri = new Uri(PicasaQuery.CreatePicasaUri(Service.UserName));
            pFeed = Service.GoogleService.Query(query) as PicasaFeed;

            if (outFileLoc != null)
            {
                using (System.IO.StreamWriter fs = new System.IO.StreamWriter(outFileLoc, false))
                {
                    if (pFeed != null && pFeed.Entries.Count > 0)
                    {
                        foreach (PicasaEntry entry in pFeed.Entries)
                        {
                            fs.WriteLine(entry.Title.Text +
                                            " (" + entry.GetPhotoExtensionValue(GPhotoNameTable.NumPhotos) + " ) " +
                                            " (" + entry.FeedUri + ")");
                        }
                    }
                }
            }
        }

        public UploadedPhoto UploadImageToAlbum(string fileName, Uri targetAlbumUri)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            System.IO.FileStream fileStream = fileInfo.OpenRead();

            PicasaEntry entry = new PhotoEntry();

            entry.MediaSource = new Google.GData.Client.MediaFileSource(fileStream, fileName, "image/jpeg");

            // note there is also an async version of this function
            PicasaEntry insertedEntry = Service.GoogleService.Insert(targetAlbumUri, entry);
            Photo returnPhoto = new Photo();
            returnPhoto.AtomEntry = insertedEntry;

            return new UploadedPhoto { Entry = insertedEntry, PicasaPhoto = returnPhoto };
        }
    }


}
