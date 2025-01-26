using System;
using HelseId.Core.BFF.Sample.Models.Model;
using Microsoft.AspNetCore.Mvc;

namespace HelseId.Core.BFF.Sample.Api.Controllers
{
    [ApiController]
    [Route("sample")]
    public class SampleController
    {
        [HttpGet]
        public ActionResult<SampleModel> Get()
        {
            var random = new Random();

            var someBytes = new byte[16];
            random.NextBytes(someBytes);

            return new SampleModel
            {
                SomeText = Convert.ToBase64String(someBytes),
                SomeNumber = random.Next(),
            };
        }
    }
}