namespace Prog6212Poe.Models
{
    using System;
    using System.Collections.Generic;

    public partial class LoginInfo
    {
        public LoginInfo()
        {
          
        }
        public int Id { get; set; }
        public int? LoginId { get; set; }
        public virtual Login Login { get; set; }
      
    }
}


