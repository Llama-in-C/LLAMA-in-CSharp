using System.Text.Json;
using System.Text.Json.Serialization;

namespace LiC_Backend;

using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

public abstract class NamedPipeClient
{
    public static async Task<PipeResponse> GenerateText(PipePayload incomingPayload)
    {
        PipeResponse responseDeserialized = new PipeResponse();
        
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

            responseDeserialized = JsonSerializer.Deserialize<PipeResponse>(response) ??
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

    public class PipePayload
    {
        [JsonPropertyName("InputText")]
        public required string InputText { get; set; }
        
        [JsonPropertyName("CallType")]
        public required Enumerations.IncomingCallType CallType { get; set; }
        
        [JsonPropertyName("PathToModel")]
        public string? PathToModel { get; set; }
    }
    
    public class PipeResponse
    {
        public string? Error { get; set; }
        
        public int Code { get; set; }
        
        public string? Output { get; set; }
    }
}