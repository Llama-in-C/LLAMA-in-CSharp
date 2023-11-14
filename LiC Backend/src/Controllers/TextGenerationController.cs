using LiC_Backend.ModelLayer;
using LiC_Backend.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace LiC_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TextGenerationController : ControllerBase
{
    [HttpPost]
    [Route("Initialize")]
    public async Task<ActionResult<SocketClientModel.SocketResponse>> Initialize([FromBody] SocketClientModel.SocketPayload incomingPayload)
    {
        SocketClientModel.SocketResponse results = await SocketClientService.Initialize(incomingPayload);

        return results;
    }
    
    [HttpPost]
    [Route("GenerateText")]
    public async Task<ActionResult<SocketClientModel.SocketResponse>> GenerateText([FromBody] SocketClientModel.SocketPayload incomingPayload)
    {
        SocketClientModel.SocketResponse results = await SocketClientService.GenerateText(incomingPayload);

        return results;
    }
    
    [HttpPost]
    [Route("SwapModel")]
    public async Task<ActionResult<SocketClientModel.SocketResponse>> SwapModel([FromBody] SocketClientModel.SocketPayload incomingPayload)
    {
        SocketClientModel.SocketResponse results = await SocketClientService.SwapModel(incomingPayload);
    
        return results;
    }
}