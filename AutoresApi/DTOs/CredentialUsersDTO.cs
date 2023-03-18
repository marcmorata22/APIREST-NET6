﻿using System.ComponentModel.DataAnnotations;

namespace AutoresApi.DTOs
{
    public class CredentialUsersDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } 
    }
}
