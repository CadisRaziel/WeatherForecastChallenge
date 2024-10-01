using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WeatherForecastChallenge.API.Controllers
{    
    [ApiController]
    public class MainController : ControllerBase
    {      
        protected ICollection<string> Errors = new List<string>();
        protected ActionResult CustomResponse(object result = null)  
        {           
            if (InvalidOperation())
                return Ok(result);
                        
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                {"Message", Errors.ToArray() } 
            }));
        }
        
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in errors)
            {
                AddProcessingError(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected bool InvalidOperation()
        {
            return !Errors.Any(); 
        }

        protected void AddProcessingError(string erro)
        {
            Errors.Add(erro);
        }

        protected void ClearProcessingErrors()
        {
            Errors.Clear();
        }   
    }
}
