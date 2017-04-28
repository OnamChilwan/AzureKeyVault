namespace KeyVault.Client.Controllers
{
    using System;
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

        [Route("{token}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string token)
        {
            try
            {
                var result = await this.tokeniserService.Detokenise<CardHolderData>(token);

                if (result == null)
                {
                    return this.NotFound();
                }

                return this.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]CardHolderData card)
        {
            try
            {
                var token = await this.tokeniserService.Tokenise(card);
                var uri = $"{this.Request.RequestUri}{token}";

                return this.Created(uri, card);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}