namespace LiC_Backend.ModelLayer;

public abstract class SocketClientModel
{
    public class SocketPayload
    {
        public required IncomingCallType CallType { get; set; }
        
        public string? PathToModel { get; set; }
        
        public string? InputText { get; set; }

        public float? Temperature { get; set; }
        
        public float? TopK { get; set; }
        
        public float? TopP { get; set; }
        
        public float? RepetitionPenalty { get; set; }
        
        public int? MaxNewTokens { get; set; }
    }
    
    public class SocketResponse
    {
        public string? Error { get; set; }
        
        public int Code { get; set; }
        
        public string? Output { get; set; }

        public float? TimeTotal { get; set; }

        public int? MaxNewTokens { get; set; }
    }
    
    public enum IncomingCallType
    {
        Initialize = 0,
        Generate = 1,
        SwapModel = 2
    }
}