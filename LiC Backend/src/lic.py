import sys
import os
import time

import win32file
import win32pipe

import traceback
from multiprocessing import Process

try:
    import flash_attn
except ImportError:
    print("Yo, exllamaV2 is WAYYYY better with flash_attn. I can be a pain to figure out, but it's worth it, trust!")

from exllamav2 import (
    ExLlamaV2,
    ExLlamaV2Config,
    ExLlamaV2Cache,
    ExLlamaV2Tokenizer,
)

from exllamav2.generator import (
    ExLlamaV2BaseGenerator,
    ExLlamaV2Sampler
)


sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))


def pipe_server():
    print("Starting pipe server")

    pipe_name = r'\\.\pipe\testpipe'

    while True:
        pipe = win32pipe.CreateNamedPipe(
            pipe_name,
            win32pipe.PIPE_ACCESS_DUPLEX,
            win32pipe.PIPE_TYPE_MESSAGE | win32pipe.PIPE_READMODE_MESSAGE | win32pipe.PIPE_WAIT,
            1, 65536, 65536,
            0,
            None)

        try:
            print("Waiting for client")
            win32pipe.ConnectNamedPipe(pipe, None)
            print("Got client")
            output = ""
            while True:
                # Read the message from the named pipe
                result, data = win32file.ReadFile(pipe, 64 * 1024)
                if result != 0:
                    break

                # Do some processing here
                generator.warmup()
                time_begin = time.time()

                output = generator.generate_simple(data.decode(), settings, max_new_tokens)

                time_end = time.time()
                time_total = time_end - time_begin

                print(f"Generated text:\n{output}\n")
                print(f"Response generated in {time_total:.2f} seconds, {max_new_tokens} tokens, {max_new_tokens / time_total:.2f} tokens/second")

                # Write the result back to the named pipe
                win32file.WriteFile(pipe, str.encode(output))

        except Exception as e:
            if e.args[0] != 109:
                print(f"Exception: {e}")
                traceback.print_exc()

        finally:
            win32file.CloseHandle(pipe)


if __name__ == '__main__':
    server_process = Process(target=pipe_server)
    server_process.start()
    server_process.join()


config = ExLlamaV2Config()
#config.model_dir = model_directory
config.prepare()

model = ExLlamaV2(config)
#print("Loading model: " + model_directory)

cache = ExLlamaV2Cache(model, lazy=True)
model.load_autosplit(cache)

tokenizer = ExLlamaV2Tokenizer(config)

# Initialize generator

generator = ExLlamaV2BaseGenerator(model, cache, tokenizer)

# Generate some text

settings = ExLlamaV2Sampler.Settings()
settings.temperature = 1.25
settings.top_k = 0
settings.top_p = 0.8
settings.token_repetition_penalty = 1.15
settings.disallow_tokens(tokenizer, [tokenizer.eos_token_id])

max_new_tokens = 100
