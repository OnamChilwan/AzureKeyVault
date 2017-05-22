namespace KeyVault.Client.Services
{
    using System.Text;
    using System.Threading.Tasks;
    using KeyVault.Client.Models;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;
    using Microsoft.Azure.KeyVault.WebKey;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    //public interface IKeyVaultService
    //{
    //    Task<EncryptedCard> Encrypt(CardHolderData cardHolderData);

    //    Task<CardHolderData> Decrypt(EncryptedCard encryptedCard);
    //}

    //public class KeyVaultService : IKeyVaultService
    //{
    //    private const string EncryptionKeyName = "foobar";
    //    private readonly string uri;
    //    private readonly string clientId;
    //    private readonly string clientSecret;
    //    private readonly KeyVaultClient vault;

    //    public KeyVaultService(string uri, string clientId, string clientSecret)
    //    {
    //        this.uri = uri;
    //        this.clientId = clientId;
    //        this.clientSecret = clientSecret;
    //        this.vault = new KeyVaultClient(new KeyVaultCredential(this.GetToken));
    //    }

    //    public async Task<EncryptedCard> Encrypt(CardHolderData cardHolderData)
    //    {
    //        var encryptionKey = await this.GetSecret(EncryptionKeyName);
    //        var cardNumber = await this.Encrypt(encryptionKey.Value, cardHolderData.CardNumber);
    //        var endDate = await this.Encrypt(encryptionKey.Value, cardHolderData.EndDate);
    //        var nameOnCard = await this.Encrypt(encryptionKey.Value, cardHolderData.NameOnCard);

    //        return new EncryptedCard { CardNumber = cardNumber, NameOnCard = nameOnCard, EndDate = endDate };
    //    }

    //    public async Task<CardHolderData> Decrypt(EncryptedCard card)
    //    {
    //        var encryptionKey = await this.GetSecret(EncryptionKeyName);
    //        var cardNumber = await this.Decrypt(encryptionKey.Value, card.CardNumber);
    //        var endDate = await this.Decrypt(encryptionKey.Value, card.EndDate);
    //        var nameOnCard = await this.Decrypt(encryptionKey.Value, card.NameOnCard);

    //        return new CardHolderData { CardNumber = cardNumber, NameOnCard = nameOnCard, EndDate = endDate };
    //    }

    //    private async Task<SecretBundle> GetSecret(string name)
    //    {
    //        return await this.vault.GetSecretAsync(this.uri, name);
    //    }

    //    private async Task<string> GetToken(string authority, string resource, string scope)
    //    {
    //        var context = new AuthenticationContext(authority);
    //        var credentials = new ClientCredential(this.clientId, this.clientSecret);
    //        var result = await context.AcquireTokenAsync(resource, credentials);

    //        return result.AccessToken;
    //    }

    //    private async Task<byte[]> Encrypt(string key, string value)
    //    {
    //        var data = Encoding.ASCII.GetBytes(value);
    //        var result = await this.vault.EncryptAsync(this.uri, key, "2797837aeb47405b8ca333bd0769a6c5", JsonWebKeyEncryptionAlgorithm.RSAOAEP, data);

    //        return result.Result;
    //    }

    //    private async Task<string> Decrypt(string key, byte[] value)
    //    {
    //        var result = await this.vault.DecryptAsync(this.uri, key, "2797837aeb47405b8ca333bd0769a6c5", JsonWebKeyEncryptionAlgorithm.RSAOAEP, value);
    //        var foo = Encoding.ASCII.GetString(result.Result);

    //        return foo;
    //    }
    //}
}