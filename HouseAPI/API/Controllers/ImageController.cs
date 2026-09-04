namespace House.API.Controllers
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using HLL.Images.Interfaces;

    [Route("image")]
    public class ImageController : BaseController
    {
        private readonly IImageProvider __image;
        public ImageController(IImageProvider image)
        {
            __image = image;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public Task<IActionResult> RandomImage()
        {
            return ExecuteAndMapToActionResult(() => __image.GetRandomImageSource());
        }
    }
}