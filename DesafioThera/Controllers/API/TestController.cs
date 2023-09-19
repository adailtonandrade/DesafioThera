using System.Web.Http;

namespace DesafioThera.Controllers.API
{
    public class TestController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok(new { Nome = "Teste", Situacao = "Ok" });
        }
    }
}