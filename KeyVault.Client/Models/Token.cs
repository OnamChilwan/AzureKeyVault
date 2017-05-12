namespace KeyVault.Client.Models
{
    using System;
    using System.IdentityModel.Tokens.Jwt;

    using Microsoft.IdentityModel.Tokens;

    //public class Token
    //{
    //    private readonly string token;

    //    public Token(string token)
    //    {
    //        this.token = token;
    //    }

    //    public static Token Create()
    //    {
    //        var securityTokenDescriptor = new SecurityTokenDescriptor();
    //        var token = new JwtSecurityTokenHandler().CreateEncodedJwt(securityTokenDescriptor);

    //        return new Token($"{Guid.NewGuid()}");
    //    }

    //    public override string ToString()
    //    {
    //        return this.token;
    //    }
    //}
}