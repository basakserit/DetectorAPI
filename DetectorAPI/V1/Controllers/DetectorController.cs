using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DetectorAPI.Service;
using DetectorAPI.V1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DetectorAPI.V1.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DetectorController : ControllerBase
    {
        private readonly IDetectorService _detectorService;

        public DetectorController(IDetectorService detectorService)
        {
            _detectorService = detectorService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ProviderDetectionResponse> Get(
            [Required]
            [FromQuery]
            [UrlArrayValidator(ErrorMessage = "Domain names should be valid URLs")]
            string[] domains)
        {
            return await _detectorService.DetectProvider(domains);
        }

        //[HttpGet]
        //[Route("Ips")]
        //[Produces("application/json")]
        //[ProducesResponseType(typeof(IpDetectionResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        //public async Task<IpDetectionResponse> GetIps(
        //    [Required]
        //    [FromQuery]
        //    [UrlArrayValidator(ErrorMessage = "Domain names should be valid URLs")]
        //    string[] domains)
        //{
        //    return await _detectorService.DetectIps(domains);
        //}
    }
}