namespace LLAMA_in_CSharp;

public enum LlamaVocabType 
{
    LlamaVocabTypeSpm = 0, // SentencePiece
    LlamaVocabTypeBpe = 1, // Byte Pair Encoding
}

// available llama models
internal enum EModel {
    ModelUnknown,
    Model1B,
    Model3B,
    Model7B,
    Model8B,
    Model13B,
    Model15B,
    Model30B,
    Model34B,
    Model40B,
    Model65B,
    Model70B,
}

internal enum LlmArch {
    LlmArchLlama,
    LlmArchFalcon,
    LlmArchBaichuan,
    LlmArchGpt2,
    LlmArchGptj,
    LlmArchGptneox,
    LlmArchMpt,
    LlmArchStarcoder,
    LlmArchPersimmon,
    LlmArchRefact,
    LlmArchBloom,
    LlmArchUnknown,
}

// model file types
internal enum LlamaFtype
{
    LlamaFtypeAllF32 = 0,
    LlamaFtypeMostlyF16 = 1,  // except 1d tensors
    LlamaFtypeMostlyQ40 = 2,  // except 1d tensors
    LlamaFtypeMostlyQ41 = 3,  // except 1d tensors
    LlamaFtypeMostlyQ41SomeF16 = 4,  // tok_embeddings.weight and output.weight are F16
    LlamaFtypeMostlyQ80 = 7,  // except 1d tensors
    LlamaFtypeMostlyQ50 = 8,  // except 1d tensors
    LlamaFtypeMostlyQ51 = 9,  // except 1d tensors
    LlamaFtypeMostlyQ2K = 10, // except 1d tensors
    LlamaFtypeMostlyQ3Ks = 11, // except 1d tensors
    LlamaFtypeMostlyQ3Km = 12, // except 1d tensors
    LlamaFtypeMostlyQ3Kl = 13, // except 1d tensors
    LlamaFtypeMostlyQ4Ks = 14, // except 1d tensors
    LlamaFtypeMostlyQ4Km = 15, // except 1d tensors
    LlamaFtypeMostlyQ5Ks = 16, // except 1d tensors
    LlamaFtypeMostlyQ5Km = 17, // except 1d tensors
    LlamaFtypeMostlyQ6K = 18, // except 1d tensors

    LlamaFtypeGuessed = 1024, // not specified in the model file
}