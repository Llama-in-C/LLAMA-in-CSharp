from time import time
from LiCObjects import PipeResponse


def text_inference(payload, settings, generator):
    generator.warmup()

    settings.temperature = payload.Temperature
    settings.top_k = payload.TopK
    settings.top_p = payload.TopP
    settings.token_repetition_penalty = payload.RepetitionPenalty

    time_begin = time()
    output = generator.generate_simple(payload.InputText, settings, payload.MaxNewTokens)

    time_end = time()
    time_total = time_end - time_begin

    return PipeResponse(Code=200, Output=output, TimeTotal=time_total, MaxNewTokens=payload.MaxNewTokens)
