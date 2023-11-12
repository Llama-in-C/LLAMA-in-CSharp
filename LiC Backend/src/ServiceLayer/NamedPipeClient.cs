using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using LiC_Backend.ModelLayer;

namespace LiC_Backend.ServiceLayer;

public abstract class NamedPipeClient
{
    public static async Task<PipeModel.PipeResponse> GenerateText(PipeModel.PipePayload incomingPayload)
    {
        PipeModel.PipeResponse responseDeserialized = new PipeModel.PipeResponse();
        
        try
        {
            await using var pipeClient =
                new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut, PipeOptions.Asynchronous);
            Console.WriteLine("Connecting to server...");
            await pipeClient.ConnectAsync();

            Console.WriteLine("Connected to server, sending message...");
            
            string serializedPayload = JsonSerializer.Serialize(incomingPayload);
            byte[] buffer = Encoding.UTF8.GetBytes(serializedPayload);
            await pipeClient.WriteAsync(buffer, 0, buffer.Length);

            // Read server response
            byte[] responseBuffer = new byte[1024];
            int bytesRead = await pipeClient.ReadAsync(responseBuffer, 0, responseBuffer.Length);
            string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);

            responseDeserialized = JsonSerializer.Deserialize<PipeModel.PipeResponse>(response) ??
                                   throw new SystemException("Server response was null!");

            Console.WriteLine($"Server response: {responseDeserialized.Output ?? "No output / or error getting output, call it."}");

        }
        catch (Exception ex)
        {
            responseDeserialized.Code = StatusCodes.Status500InternalServerError;
            responseDeserialized.Error = ex.Message;
        }
        
        return responseDeserialized;
    }

 

    public static async Task<PipeModel.PipeResponse> Initialize(PipeModel.PipePayload incomingPayload)
    {
        throw new NotImplementedException();
    }

    public static async Task<PipeModel.PipeResponse> SwapModel(PipeModel.PipePayload incomingPayload)
    {
        throw new NotImplementedException();
    }
}