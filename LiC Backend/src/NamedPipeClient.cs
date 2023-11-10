using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LiC_Backend;

using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

public abstract class NamedPipeClient
{
    public static async Task<CodeSwapResult> PythonHandoff(string inputText)
    {
        CodeSwapResult results = new CodeSwapResult();
        
        await using var pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut, PipeOptions.Asynchronous);
        Console.WriteLine("Connecting to server...");
        await pipeClient.ConnectAsync();

        Console.WriteLine("Connected to server, sending message...");
        string messageToSend = inputText;
        byte[] buffer = Encoding.UTF8.GetBytes(messageToSend);
        await pipeClient.WriteAsync(buffer, 0, buffer.Length);

        // Read server response
        byte[] responseBuffer = new byte[1024];
        int bytesRead = await pipeClient.ReadAsync(responseBuffer, 0, responseBuffer.Length);
        string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
            
        Console.WriteLine($"Server response: {response}");
        results.Code = StatusCodes.Status200OK;
        results.Input = inputText;
        results.Output = response;
        
        return results;
    }

    public class CodeSwapResult
    {
        public string? Error { get; set; }
        public int Code { get; set; }
        public string? Input { get; set; }
        public string? Output { get; set; }
    }
}