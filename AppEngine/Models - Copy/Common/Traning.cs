using System;

namespace AppEngine.Models.Common
{
    public class Traning
    {

        public int TraninigID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int TraningTypeID { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
    }
}
