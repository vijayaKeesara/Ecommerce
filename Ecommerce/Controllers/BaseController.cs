using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
	public class BaseController : ControllerBase
	{
	}
}
