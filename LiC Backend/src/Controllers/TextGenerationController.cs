using LiC_Backend.ModelLayer;
using LiC_Backend.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using static LiC_Backend.ServiceLayer.NamedPipeClient;

namespace LiC_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TextGenerationController : ControllerBase
{
    [HttpPost]
    [Route("Initialize")]
    public async Task<ActionResult<PipeModel.PipeResponse>> Initialize([FromBody] PipeModel.PipePayload incomingPayload)
    {
        PipeModel.PipeResponse results = await NamedPipeClient.Initialize(incomingPayload);

        return results;
    }
    
    [HttpPost]
    [Route("GenerateText")]
    public async Task<ActionResult<PipeModel.PipeResponse>> GenerateText([FromBody] PipeModel.PipePayload incomingPayload)
    {
        PipeModel.PipeResponse results = await NamedPipeClient.GenerateText(incomingPayload);

        return results;
    }
    
    [HttpPost]
    [Route("SwapModel")]
    public async Task<ActionResult<PipeModel.PipeResponse>> SwapModel([FromBody] PipeModel.PipePayload incomingPayload)
    {
        PipeModel.PipeResponse results = await NamedPipeClient.SwapModel(incomingPayload);
    
        return results;
    }
}