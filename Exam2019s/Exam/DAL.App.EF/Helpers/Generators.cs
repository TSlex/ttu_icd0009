using System;
using Domain.Identity;

namespace DAL.App.EF.Helpers
{
    public static class Generators
    {
        public static string[] FirstNames = new[]
        {
            "Renate",
            "Raivo",
            "Ahti",
            "Evelin",
            "Aino",
            "Artur",
            "Neeme",
            "Liisi",
            "Sirje",
            "Andres",
            "Alo",
            "Urmet",
        };

        public static string[] LastNames = new[]
        {
            "Leok",
            "Ross",
            "Jaik",
            "Kivikas",
            "Aavik",
            "Molder",
            "Kuusk",
            "Kirss",
            "Anton",
            "Parn",
            "Pihlak",
        };

        public static string GetRandomElement(string[] arr)
        {
            return arr[new Random().Next(0, arr.Length - 1)];
        }

        public static AppUser GenerateUsers()
        {
            var firstName = GetRandomElement(FirstNames);
            var lastName = GetRandomElement(LastNames);
            
            var fLenght = firstName.Length;
            var fNameLeft = firstName.Substring(0, fLenght / 2);
            
            var lLenght = lastName.Length;
            var lNameRight = lastName.Substring(lLenght / 2);
            
            var name = fNameLeft + lNameRight + new Random().Next(1000, 9999);
        }
    }
}