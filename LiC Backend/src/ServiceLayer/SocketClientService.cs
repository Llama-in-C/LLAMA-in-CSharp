using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using LiC_Backend.ModelLayer;

namespace LiC_Backend.ServiceLayer;

public abstract class SocketClientService
{
    public static async Task<SocketClientModel.SocketResponse> GenerateText(SocketClientModel.SocketPayload incomingPayload)
    {
        SocketClientModel.SocketResponse responseDeserialized = new SocketClientModel.SocketResponse();
        
        byte[] bytes = new byte[65536];
        
        try
        {            
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 5002);

            // Create a TCP/IP socket
            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint
            Console.WriteLine("Connecting to server...");
            await sender.ConnectAsync(localEndPoint);

            // Sending data
            Console.WriteLine("Connected to server, sending message...");
            string serializedPayload = JsonSerializer.Serialize(incomingPayload);
            byte[] message = Encoding.ASCII.GetBytes(serializedPayload);
            await sender.SendAsync(message);

            // Receiving data
            int bytesRec = sender.Receive(bytes);
            string response = Encoding.UTF8.GetString(bytes, 0, bytesRec);

            // Release the socket
            sender.Shutdown(SocketShutdown.Send);
            sender.Close();

            responseDeserialized = JsonSerializer.Deserialize<SocketClientModel.SocketResponse>(response) ??
                                   throw new SystemException("Server response was null!");

            Console.WriteLine($"Server response: {responseDeserialized.Output ?? "No output or error getting output, call it."}");
            
            if (responseDeserialized.MaxNewTokens != null && responseDeserialized.TimeTotal != null)
            {
                Console.WriteLine($"Response generated in: {responseDeserialized.TimeTotal:F2} seconds," +
                                  $" with {responseDeserialized.MaxNewTokens} tokens, " +
                                  $"{responseDeserialized.MaxNewTokens / responseDeserialized.TimeTotal:F3} tokens per sec.");
            }
        }
        catch (Exception ex)
        {
            responseDeserialized.Code = StatusCodes.Status500InternalServerError;
            responseDeserialized.Error = ex.Message;
        }
        
        return responseDeserialized;
    }

    public static async Task<SocketClientModel.SocketResponse> Initialize(SocketClientModel.SocketPayload incomingPayload)
    {
        throw new NotImplementedException();
    }

    public static async Task<SocketClientModel.SocketResponse> SwapModel(SocketClientModel.SocketPayload incomingPayload)
    {
        throw new NotImplementedException();
    }
}