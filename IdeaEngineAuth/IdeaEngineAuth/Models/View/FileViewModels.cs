using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdeaEngineAuth.Models
{
    public class FileLink : HasID
    {
        [DisplayName("File")]
        public override long ID { get; set; }

        [DisplayName("File Name")]
        public virtual string FileName { get; set; }

        [DisplayName("File Path")]
        public virtual string FilePath { get; set; }

        public override MvcHtmlString getName()
        {
            return MvcHtmlString.Create("<a href='/Download/File/" + ID + "'>" + FileName + "</a>");
        }

        public String buildPath()
        {
            return Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/Uploads"), FilePath);
        }

        public virtual void saveFile() { }

        [DisplayName("Current User")]
        public CurrentUser CurrentUser { get; set; }

        public override CurrentUser getCurrentUser()
        {
            return CurrentUser;
        }
    }

    public class FileTuple : M_N_Tuple
    {
        public FileTuple(IEnumerable<FileLink> things, IEnumerable<FileLink> otherThings)
            : base(things, otherThings) { }

        public FileTuple(IEnumerable<FileLink> things) : base(things, new List<HasID>()) { }

        public override string getLable()
        {
            return "Files";
        }

        public override string getTitle()
        {
            return "File Name";
        }

        public override string getSetName()
        {
            return "UploadedFiles";
        }
    }
}