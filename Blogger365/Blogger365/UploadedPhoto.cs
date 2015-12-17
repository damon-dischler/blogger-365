using Google.GData.Photos;
using Google.GData.Extensions.Exif;
using Google.Picasa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogger365
{
    public class UploadedPhoto
    {
        public Photo PicasaPhoto { get; set; }
        public PicasaEntry Entry { get; set; }

        public string GetTitleFromExifDate()
        {
            ExifTags exif_data = Entry.Exif;
            DateTime exif_datetime = ExifTimeStampToDateTime(exif_data.Time.UnsignedLongValue);
            string exif_date = String.Format("{0} {1} {2}, {3}",
                exif_datetime.DayOfWeek,
                exif_datetime.ToString("MMMM"),
                exif_datetime.Day,
                exif_datetime.Year);

            return exif_date;
        }

        public static DateTime ExifTimeStampToDateTime(double timeStamp)
        {
            timeStamp = timeStamp / 1000;
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
