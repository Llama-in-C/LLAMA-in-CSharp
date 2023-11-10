using Microsoft.AspNetCore.Mvc;
using static LiC_Backend.NamedPipeClient;

namespace LiC_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TextGenerationController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CodeSwapResult>> GenerateText([FromBody] string inputText)
    {
        CodeSwapResult results = await NamedPipeClient.PythonHandoff(inputText);

        return results; //Ok(new { Input = inputText, Output = ActionR });
    }
}