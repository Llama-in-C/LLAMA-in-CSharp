using System.Text.Json.Serialization;

namespace LiC_Backend.ModelLayer;

public class NamedPipeClientModel
{
    public class PipePayload
    {
        [JsonPropertyName("InputText")]
        public required string InputText { get; set; }
        
        [JsonPropertyName("CallType")]
        public required NamedPipeClientModel.IncomingCallType CallType { get; set; }
        
        [JsonPropertyName("PathToModel")]
        public string? PathToModel { get; set; }
    }
    
    public class PipeResponse
    {
        public string? Error { get; set; }
        
        public int Code { get; set; }
        
        public string? Output { get; set; }
    }
    
    public enum IncomingCallType
    {
        Initialize = 0,
        Generate = 1,
        SwapModel = 2
    }
}