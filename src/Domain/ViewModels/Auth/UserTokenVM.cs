using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels.Auth
{
    public class UserTokenVM
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string Email { get; set; }
    }
}
