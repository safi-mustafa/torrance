﻿
namespace ViewModels.Authentication
{
    public class TokenVM
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
