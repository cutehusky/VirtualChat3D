using System;
using UnityEngine;

namespace Core.UserAccountModule.Model
{
    public class UserAccountData
    {
        public Texture2D Avatar;
        public string Description;
        public string Username;
        public DateTime DateOfBirth;
        public string UserId = "0"; // for test
        public bool IsOnline;
    }
}