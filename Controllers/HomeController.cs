using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace WebSaves.Controllers;

[Route("/")]
[OpenApiIgnore]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Redirect("swagger");
    }
}