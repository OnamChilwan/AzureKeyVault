namespace KeyVault.Client.Models
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    public class Token
    {
        private readonly string token;

        public Token(string token)
        {
            this.token = token;
            this.Reference = GetReferenceFromToken(token);
        }

        private Token(string token, string reference)
        {
            this.Reference = reference;
            this.token = token;
        }

        public static Token Create(string data, string secret, int numberOfDays)
        {
            var reference = Guid.NewGuid().ToString("D");
            var claims = new List<Claim> { new Claim(ClaimTypes.UserData, reference) };
            var claimsIdentity = new ClaimsIdentity(claims);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(numberOfDays).Date,
                Issuer = "Tokenizer",
                IssuedAt = DateTime.UtcNow.Date,
                Subject = claimsIdentity,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            var encodedJwt = handler.CreateToken(securityTokenDescriptor);

            return new Token(handler.WriteToken(encodedJwt), reference);
        }

        public override string ToString()
        {
            return this.token;
        }

        private static string GetReferenceFromToken(string data)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(data);
            object value;

            if (token.Payload.TryGetValue(ClaimTypes.UserData, out value))
            {
                return value.ToString();
            }

            return string.Empty;
        }

        public string Reference { get; }
    }
}