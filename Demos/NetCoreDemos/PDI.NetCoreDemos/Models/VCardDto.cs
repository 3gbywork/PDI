using System;
using System.ComponentModel.DataAnnotations;

namespace PDI.NetCoreDemos.Models
{
    public class VCardDto
    {
        public int ID { get; set; }

        #region General
        [Display(Name = "Ver")]
        public string Version { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Last Rev")]
        public DateTime LastRevision { get; set; }
        #endregion

        #region Name
        [Display(Name = "Name")]
        public string SortableName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
        public string Suffix { get; set; }
        public string SortString { get; set; }
        public string FormattedName { get; set; }
        public string Nickname { get; set; }
        #endregion

        #region Work
        public string Organization { get; set; }
        //[Display(Name = "Title")]
        public string JobTitle { get; set; }
        public string Role { get; set; }
        public string Units { get; set; }
        public string Categories { get; set; }
        #endregion
    }
}
