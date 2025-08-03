﻿using TestingApi1.Models;

namespace TestingApi1.SaveData
{
    public class Register
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
    
}
