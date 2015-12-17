using Google.GData.Blogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogger365
{
    public class BloggerManager
    {
        public const string UriStringFormat = "https://www.blogger.com/feeds/{0}/posts/default";

        public ServiceHandle Service { get; set; }
        private long blog_id;

        public BloggerEntry CreateBlogPost(long blogId)
        {
            BloggerEntry entry = new BloggerEntry();
            blog_id = blogId;

            entry.Content.Type = "html";
            entry.Content.Content = "";
            entry.Title.Text = "Test Post";

            return Service.GoogleService.Insert(CreateBloggerUriById(), entry);
        }

        private Uri CreateBloggerUriById()
        {
            return new Uri(string.Format(UriStringFormat, blog_id));
        }

        public object CreateBlogPost(long blogId, UploadedPhoto photo, string title=null, string caption=null)
        {
            BloggerEntry entry = new BloggerEntry();
            blog_id = blogId;

            entry.Content.Type = "html";

            if (title == null)
                title = photo.GetTitleFromExifDate();

            entry.Title.Text = title;
            entry.Content.Content = String.Format("<img src=\"{0}\" />", photo.PicasaPhoto.PhotoUri.ToString());
            if (string.IsNullOrWhiteSpace(caption) == false)
            {
                entry.Content.Content += String.Format("<p>{0}</p>", caption);
            }

            return Service.GoogleService.Insert(CreateBloggerUriById(), entry);
        }

    }
}
