using AutoresApi.DTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace AutoresApi.Services
{
    public class HashService
    {
        public ResultHash Hash(string planeText)
        {
            var sal = new byte[6];
            using(var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(sal);
            }

            return Hash(planeText, sal);
        }

        public ResultHash Hash(string planeText, byte[] sal)
        {
            var derivativeKey = KeyDerivation.Pbkdf2(password: planeText,
                salt: sal, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 32);

            var hash = Convert.ToBase64String(derivativeKey);

            return new ResultHash()
            {
                Hash = hash,
                Sal = sal
            };
        }
    }
}
