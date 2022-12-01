using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers;


[Route("[controller]")]
[ApiController]
public class BaseController<T> : ControllerBase where T : BaseController<T>
{
	protected readonly ILogger<T> _logger = null!;
	public BaseController() { }
	public BaseController(ILogger<T> logger) => _logger = logger;
}
