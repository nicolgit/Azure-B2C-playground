using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace nicold.APIScientificCalculator.Controllers
{

    [Authorize]
    [Produces("application/json")]
    [Route("api/ScientificCalc")]
    public class ScientificCalcController : ControllerBase
    {
        private static readonly HttpClient _client = new HttpClient();
        private string CALL_MULTIPLY = "http://nicolapicalculator.azurewebsites.net/api/calc/multiply?param1={0}&param2={1}";
        private string CALL_SPLIT = "http://nicolapicalculator.azurewebsites.net/api/calc/split?param1={0}&param2={1}";

        public const string POWER = "power";
        public const string PERCENTAGE = "percentage";
        public const string FACTORIAL = "factorial";

        // GET api/values/
        [HttpGet("{op}")]
        public async Task<IActionResult> Get(string op, [FromQuery] double param1, [FromQuery] double param2)
        {
            double result = 0;

            try
            {
                switch (op)
                {
                    case POWER:
                        result = 1;
                        for (int i = 0; i< param2; i++)
                        {
                            result = await _multiply(result, param1);
                        }
                        break;
                    case PERCENTAGE:
                        result = await _multiply( param1, param2);
                        result = await _split(result, 100.0);
                        break;
                    case FACTORIAL:
                        result = 1;
                        for (int i=1; i<= param1; i++)
                        {
                            result = await _multiply(result, i);
                        }
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

        private async Task<double> _multiply(double param1, double param2)
        {
            var header_bearer = HttpContext.Request.Headers["Authorization"];
            string bearer = header_bearer.FirstOrDefault().Split(" ")[1];

            double result = 0;

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var httpResponse = await _client.GetAsync(string.Format(CALL_MULTIPLY, param1, param2));

            if (httpResponse.IsSuccessStatusCode)
            {
                result = double.Parse(await httpResponse.Content.ReadAsStringAsync(), CultureInfo.InvariantCulture);
            }
            return result;
        }

        private async Task<double> _split(double param1, double param2)
        {
            var header_bearer = HttpContext.Request.Headers["Authorization"];
            string bearer = header_bearer.FirstOrDefault().Split(" ")[1];

            double result = 0;

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var httpResponse = await _client.GetAsync(string.Format(CALL_SPLIT, param1, param2));

            if (httpResponse.IsSuccessStatusCode)
            {
                result = double.Parse(await httpResponse.Content.ReadAsStringAsync(), CultureInfo.InvariantCulture);
            }
            return result;
        }
    }
}