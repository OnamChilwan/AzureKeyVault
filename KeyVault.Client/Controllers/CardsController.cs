namespace KeyVault.Client.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using KeyVault.Client.Models;
    using KeyVault.Client.Services;

    [RoutePrefix("api/cards")]
    public class CardsController : ApiController
    {
        private readonly ITokeniserService tokeniserService;

        public CardsController(ITokeniserService tokeniserService)
        {
            this.tokeniserService = tokeniserService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string id)
        {
            var result = await this.tokeniserService.Detokenise(id);

            return this.Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]CardHolderData card)
        {
            var token = await this.tokeniserService.Tokenise(card);
            var uri = $"{this.Request.RequestUri}/{token}";

            return this.Created(uri, token);
        }
    }
}