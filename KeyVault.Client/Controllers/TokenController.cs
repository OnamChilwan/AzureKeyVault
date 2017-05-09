namespace KeyVault.Client.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using KeyVault.Client.Models;
    using KeyVault.Client.Services;
    using Newtonsoft.Json.Linq;

    [RoutePrefix("api")]
    public class TokenController : ApiController
    {
        private readonly ITokeniserService tokeniserService;

        public TokenController(ITokeniserService tokeniserService)
        {
            this.tokeniserService = tokeniserService;
        }

        [Route("tokenise")]
        [HttpPut]
        public async Task<IHttpActionResult> Put([FromBody]JObject data)
        {
            var token = await this.tokeniserService.Tokenise(data.ToString());
            var uri = $"{this.Request.RequestUri}/detokenise";

            return this.Created(uri, new Reference { Value = token });
        }

        [Route("detokenise")]
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