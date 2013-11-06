using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace musicapp.Models
{
    public class TabulatureTable
    {
        public int id { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<int> coverID { get; set; }
        public Nullable<int> intrumentID { get; set; }
        public string tabSitePath { get; set; }
        public string tabFolderPath { get; set; }
        public string tabName { get; set; }
        public string tabExtension { get; set; }
        public string tabDescription { get; set; }
        public string tabContent { get; set; }
        public Nullable<System.DateTime> tabUploadTime { get; set; }
        public string tabGuid { get; set; }

        public virtual Users Users { get; set; }
    }
}