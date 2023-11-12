using Microsoft.AspNetCore.Mvc;
using static LiC_Backend.NamedPipeClient;

namespace LiC_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TextGenerationController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PipeResponse>> GenerateText([FromBody] PipePayload incomingPayload)
    {
        PipeResponse results = await NamedPipeClient.GenerateText(incomingPayload);

        return results;
    }
    
    // [HttpPost]
    // public async Task<ActionResult<PipeResponse>> SwapModel([FromBody] PipePayload incomingPayload)
    // {
    //     PipeResponse results = await NamedPipeClient.GenerateText(incomingPayload);
    //
    //     return results;
    // }
}