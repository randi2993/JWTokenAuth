using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels.Auth
{
    public class JWTokenVM
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; } // Not yet, coming soon
    }
}
