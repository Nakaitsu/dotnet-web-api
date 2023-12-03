using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BCrypt.Net;
using static BCrypt.Net.BCrypt;
using System;
using System.Security.Cryptography;

namespace alura_webapi.Models
{
  public class User
  {
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Username { get; set; }

    [StringLength(100)]
    public string Email { get; set; }

    public string Password { get; set; }

    [NotMapped]
    private const int _saltSize = 16;

    [NotMapped]
    private const int _hashSize = 20;

    public static string HashPassword(string password, int iterations = 1000)
    {
      byte[] salt = new byte[_saltSize]; 
      RandomNumberGenerator.Create().GetBytes(salt);

      var key = new Rfc2898DeriveBytes(password, salt, iterations);
      byte[] hash = key.GetBytes(_hashSize);
      byte[] hashBytes = new byte[_saltSize + _hashSize];

      Array.Copy(salt, 0, hashBytes, 0, _saltSize);
      Array.Copy(hash, 0, hashBytes, _saltSize, _hashSize); 

      return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string password, int iterations = 1000)
    {
      byte[] storedHashed = Convert.FromBase64String(Password);
      byte[] salt = new byte[_saltSize];

      Array.Copy(storedHashed, 0, salt, 0, _saltSize);

      var key = new Rfc2898DeriveBytes(password, salt, iterations);
      byte[] hash = key.GetBytes(_hashSize);

      for(int i = 0; i < _hashSize; i++)
      {
        if(storedHashed[i + _saltSize] != hash[i])
          return false;
      }

      return true;
    }
  }
}