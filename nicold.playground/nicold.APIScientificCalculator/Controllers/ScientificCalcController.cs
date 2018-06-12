using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace nicold.APIScientificCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScientificCalcController : ControllerBase
    {
        public const string POWER = "power";
        public const string PERCENTAGE = "percentage";
        public const string FACTORIAL = "factorial";

        // GET api/values/
        [HttpGet("{op}")]
        public IActionResult Get(string op, [FromQuery] double param1, [FromQuery] double param2)
        {
            double result = 0;

            try
            {
                switch (op)
                {
                    case POWER:
                        result = param1 + param2;
                        break;
                    case PERCENTAGE:
                        result = param1 - param2;
                        break;
                    case FACTORIAL:
                        result = param1 * param2;
                        break;
                    default:
                        throw new InvalidOperationException(op);
                }
            }
            catch (InvalidOperationException)
            {
                return NotFound($"Operation not found: {op}");
            }

            return new ObjectResult(result);
        }
    }
}