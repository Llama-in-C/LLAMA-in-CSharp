namespace LiC_Backend.ModelLayer;

public abstract class NamedPipeClientModel
{
    public class PipePayload
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
    
    public class PipeResponse
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