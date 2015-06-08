using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace AppEngine.Models.DataObject
{
    public class TrainingDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TrainingDetailID { get; set; }
        public int TrainingID { get; set; }
        public TrainingResource ResourceType { get; set; }
        //[MaxLength(100)]
        public string Text { get; set; }
        public string ExternalResource { get; set; }
        public string InternalResource { get; set; }
        public int DisplayNo { get; set; }
        public decimal FileSize { get; set; }
        [NotMapped]
        public string Name
        {
            get
            {
                if (!String.IsNullOrEmpty(ExternalResource))
                {
                    return ExternalResource;
                }
                if (!String.IsNullOrEmpty(InternalResource))
                {
                    var path = InternalResource;
                    if (path.StartsWith("/"))
                    {
                        path = path.Substring(1, path.Length - 1);
                        path = path.Replace("/", "\\");
                    }
                    return Path.GetFileName(path);
                }
                return String.Empty;
            }
        }
    }

    public enum TrainingResource
    {
        Text = 0,
        Graphic = 1,
        Video = 2,
        Presentation = 3,
        File = 4
    }

}
