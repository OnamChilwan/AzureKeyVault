namespace KeyVault.Client.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using KeyVault.Client.Models;
    using KeyVault.Client.Services;
    using Newtonsoft.Json.Linq;

    [RoutePrefix("api")]
    public class CardTokenController : ApiController
    {
        private readonly ITokeniserService tokeniserService;

        public CardTokenController(ITokeniserService tokeniserService)
        {
            this.tokeniserService = tokeniserService;
        }

        [Route("card/tokenise")]
        [HttpPut]
        public async Task<IHttpActionResult> Put([FromBody]JObject data)
        {
            var token = await this.tokeniserService.Tokenise(data.ToString());

            return this.Created(string.Empty, new Reference { Value = token });
        }

        [Route("card/detokenise")]
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