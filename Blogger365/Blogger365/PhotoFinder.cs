using System;
using System.Collections.Generic;
using System.IO;
using ExifLib;

namespace Blogger365
{
    public class PhotoFinder
    {
        public string Folder { get; set; }
        public string Pattern { get; set; }

        private List<FileInfo> photos_fileinfo;

        public PhotoFinder()
        {
            Pattern = "*.jpg";
            photos_fileinfo = new List<FileInfo>();
        }

        public int LoadAllPhotos()
        {
            photos_fileinfo.Clear();

            var photos_in_dir = Directory.EnumerateFiles(Folder, Pattern);
            foreach(var photo in photos_in_dir)
                photos_fileinfo.Add(new FileInfo(photo));

            return photos_fileinfo.Count;
        }


        public FileInfo LastUpload { get; set; }

        public FileInfo GetNextImage()
        {
            // Next = return value
            // Last = last image uploaded
            // Current = search index

            FileInfo next_image_fileinfo = null;
            DateTime next_image_date;

            DateTime last_image_taken_date = GetDateTaken(LastUpload);

            next_image_date = last_image_taken_date;
            next_image_fileinfo = new FileInfo(LastUpload.FullName);

            foreach (var photo in photos_fileinfo)
            {
                DateTime current_image_date = GetDateTaken(photo);

                if ((current_image_date > last_image_taken_date)
                    && ((current_image_date < next_image_date) || (next_image_date == last_image_taken_date)))
                {
                    next_image_date = current_image_date;
                    next_image_fileinfo = photo;
                }
                else if ((current_image_date > last_image_taken_date)
                    && (current_image_date > next_image_date))
                {
                    continue;
                }
                else if (current_image_date < last_image_taken_date)
                {
                    continue;
                }
                    
            }
            return next_image_fileinfo;
        }

        private DateTime GetDateTaken(FileInfo photo)
        {
            DateTime exif_date;
            // Try to get the date taken via exif data, if that fails, use
            // creation time.
            try
            {
                using (var exifreader = new ExifReader(photo.FullName))
                {
                    exifreader.GetTagValue(ExifTags.DateTimeDigitized, out exif_date);
                }
            }
            catch (ExifLibException ex)
            {
                exif_date = photo.CreationTime;
            }

            return exif_date;
        }
    }
}
