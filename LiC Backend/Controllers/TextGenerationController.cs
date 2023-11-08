using Microsoft.AspNetCore.Mvc;

namespace LiC_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TextGenerationController : ControllerBase
{
    [HttpPost]
    public IActionResult GenerateText([FromBody] string inputText)
    {
        // Placeholder logic for text generation
        var generatedText = $"Received: {inputText}";

        return Ok(new { Input = inputText, Output = generatedText });
    }
}