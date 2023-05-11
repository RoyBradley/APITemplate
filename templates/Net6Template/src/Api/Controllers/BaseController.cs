using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers;


[Route("[controller]")]
[ApiController]
public class BaseController<T> : ControllerBase where T : BaseController<T>
{
	private readonly ILogger<T> _logger;

	protected BaseController() { }
	protected BaseController(ILogger<T> logger) => _logger = logger;
}
