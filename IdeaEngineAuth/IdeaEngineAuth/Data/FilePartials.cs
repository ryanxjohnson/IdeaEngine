using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Models;

namespace IdeaEngineAuth.Data
{
    [MetadataType(typeof(FileLink))]
    public partial class CurrentFileData : FileLink { }

    public partial class File : FileLink
    {
        public override long ID
        {
            get { return base.ID = File_ID; }
            set { base.ID = File_ID = value; }
        }

        public HttpPostedFileBase RawFile { get; set; }

        public override void saveFile()
        {
            if ( RawFile != null )
            {
                RawFile.SaveAs(buildPath());
            }
        }
    }
}