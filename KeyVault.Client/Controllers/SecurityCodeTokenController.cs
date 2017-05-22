namespace KeyVault.Client.Controllers
{
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;

    using KeyVault.Client.Commands;
    using KeyVault.Client.Models;
    using KeyVault.Client.Queries;
    using KeyVault.Client.Services;

    using Newtonsoft.Json.Linq;

    [RoutePrefix("api")]
    public class SecurityCodeTokenController : ApiController
    {
        private readonly ITokeniserService tokeniserService;

        public SecurityCodeTokenController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            this.tokeniserService = new TokeniserService(
                new SqlAddSecurityCodeCommand(connectionString),
                new SqlGetSecurityCodeQuery(connectionString),
                "hello world this is a very secure secret sssssshhhhhh");
        }

        [Route("securitycode/tokenise")]
        [HttpPut]
        public async Task<IHttpActionResult> Put([FromBody]JObject data)
        {
            var token = await this.tokeniserService.Tokenise(data.ToString());

            return this.Created(string.Empty, new Reference { Value = token });
        }

        [Route("securitycode/detokenise")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]Reference reference)
        {
            var result = await this.tokeniserService.Detokenise(reference.Value);

            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(JObject.Parse(result));
        }
    }
}