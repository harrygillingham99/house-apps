namespace House.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Serilog;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    [ApiController]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public abstract class BaseController : ControllerBase
    {
        protected async Task<IActionResult> ExecuteAndMapToActionResult<T>(Func<Task<T>> request)
        {
            try
            {
                var response = await request.Invoke();
                return response switch
                {
                    Exception errorResponse => throw errorResponse,

                    _ => Ok(response)
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return Problem(ex.Message, statusCode: 500, title: ex.GetType().Name);
            }
        }

        protected async Task<IActionResult> ExecuteAndMapToActionResult<T>(Func<Task> request)
        {
            try
            {
                await request.Invoke();

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return Problem(ex.Message, statusCode: 500, title: ex.GetType().Name);
            }
        }

        protected IActionResult ExecuteAndMapToActionResultSync<T>(Func<T> request)
        {
            try
            {
                var response = request.Invoke();
                return response switch
                {
                    Exception errorResponse => throw errorResponse,

                    _ => Ok(response)
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return Problem(ex.Message, statusCode: 500, title: ex.GetType().Name);
            }
        }


        protected async Task<T> ExecuteAndReturn<T>(Func<Task<T>> request)
        {
            try
            {
                return await request.Invoke();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        protected IActionResult ExecuteAction(Action action, string errorMessage = null)
        {
            try
            {
                action.Invoke();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return Problem(errorMessage ?? ex.Message, statusCode: 500, title: ex.GetType().Name);
            }
        }
    }
}
