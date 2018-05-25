using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace nicold.APICalculator.Controllers
{
    /// <summary>
    /// Samples
    ///     http://nicolapicalculator.azurewebsites.net/api/calc/sum?param1=5&param2=7
    ///     http://nicolapicalculator.azurewebsites.net/api/calc/subtract?param1=5&param2=7
    ///     http://nicolapicalculator.azurewebsites.net/api/calc/multiply?param1=5&param2=7
    ///     http://nicolapicalculator.azurewebsites.net/api/calc/split?param1=5&param2=7
    /// </summary>


    [Produces("application/json")]
    [Route("api/Calc")]
    public class CalcController : Controller
    {
        public const string SUM = "sum";
        public const string SUBTRACT= "subtract";
        public const string MULTIPLIES = "multiply";
        public const string SPLIT = "split";

        // GET api/values/
        [HttpGet("{op}")]
        public IActionResult Get(string op, [FromQuery] double param1, [FromQuery] double param2)
        {
            double result = 0;

            if (isAuthorized())
            {
                try
                {
                    switch (op)
                    {
                        case SUM:
                            result = param1 + param2;
                            break;
                        case SUBTRACT:
                            result = param1 - param2;
                            break;
                        case MULTIPLIES:
                            result = param1 * param2;
                            break;
                        case SPLIT:
                            result = param1 / param2;
                            break;
                        default:
                            throw new InvalidOperationException(op);
                    }

                }
                catch (InvalidOperationException)
                {
                    return NotFound($"Operation not found: {op}");
                }
            }
            else
            {
                return Unauthorized();
            }         

            return new ObjectResult(result);
        }

        private bool isAuthorized()
        {
            var user = HttpContext.User;
            var claims = HttpContext.User.Claims.ToArray();

            return user.Identity.IsAuthenticated;
        }
    }
}