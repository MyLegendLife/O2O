using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace O2O.Web.Models
{
    public class LoginPost
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Pwd { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string VarCode { get; set; }
    }
}