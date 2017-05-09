namespace KeyVault.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using KeyVault.Client.Commands;
    using KeyVault.Client.Models;
    using KeyVault.Client.Queries;
    using Microsoft.IdentityModel.Tokens;

    public interface ITokeniserService
    {
        Task<string> Tokenise(string data);

        Task<string> Detokenise(string token);
    }

    public class TokeniserService : ITokeniserService
    {
        private readonly IAddDataCommand addDataCommand;
        private readonly IGetDataQuery getDataQuery;
        private const string SecuirtyKey = "hello world this is a very secure secret sssssshhhhhh"; //TODO: inject this in

        public TokeniserService(
            IAddDataCommand addDataCommand,
            IGetDataQuery getDataQuery)
        {
            this.addDataCommand = addDataCommand;
            this.getDataQuery = getDataQuery;
        }

        public async Task<string> Tokenise(string data)
        {
            var reference = Guid.NewGuid().ToString("D");

            await this.addDataCommand.Execute(reference, Convert.ToBase64String(Encoding.UTF8.GetBytes(data)));

            return await Task.FromResult(CreateJwt(reference));
        }

        public async Task<string> Detokenise(string token)
        {
            var reference = GetReferenceFromToken(token);
            var data = await this.getDataQuery.Execute(reference);

            if (data != null)
            {
                var value = Encoding.UTF8.GetString(Convert.FromBase64String(data));
                return value;
            }

            throw new Exception("Invalid token");
        }

        private static string CreateJwt(string token)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.UserData, token) };
            var claimsIdentity = new ClaimsIdentity(claims);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = "Tokenizer",
                IssuedAt = DateTime.UtcNow,
                Subject = claimsIdentity,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecuirtyKey)), SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            var encodedJwt = handler.CreateToken(securityTokenDescriptor);

            return handler.WriteToken(encodedJwt);
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
    }
}