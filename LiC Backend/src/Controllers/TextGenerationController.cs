using LiC_Backend.ModelLayer;
using LiC_Backend.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using static LiC_Backend.ServiceLayer.NamedPipeClientService;

namespace LiC_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TextGenerationController : ControllerBase
{
    [HttpPost]
    [Route("Initialize")]
    public async Task<ActionResult<NamedPipeClientModel.PipeResponse>> Initialize([FromBody] NamedPipeClientModel.PipePayload incomingPayload)
    {
        NamedPipeClientModel.PipeResponse results = await NamedPipeClientService.Initialize(incomingPayload);

        return results;
    }
    
    [HttpPost]
    [Route("GenerateText")]
    public async Task<ActionResult<NamedPipeClientModel.PipeResponse>> GenerateText([FromBody] NamedPipeClientModel.PipePayload incomingPayload)
    {
        NamedPipeClientModel.PipeResponse results = await NamedPipeClientService.GenerateText(incomingPayload);

        return results;
    }
    
    [HttpPost]
    [Route("SwapModel")]
    public async Task<ActionResult<NamedPipeClientModel.PipeResponse>> SwapModel([FromBody] NamedPipeClientModel.PipePayload incomingPayload)
    {
        NamedPipeClientModel.PipeResponse results = await NamedPipeClientService.SwapModel(incomingPayload);
    
        return results;
    }
}