import sys
import os
import time
import win32file
import win32pipe
import traceback
from multiprocessing import Process
from pathlib import Path


try:
    import flash_attn
except ImportError:
    print('Yo, exllamaV2 is WAYYYY better with flash_attn. I can be a pain to figure out, but it\'s worth it, trust!')

sys.path.insert(0, 'Z:\LLAMA in CSharp\LiC Backend\src\Python\External\exllamav2')

from External.exllamav2.exllamav2 import (
    model, cache, tokenizer, generator
)

from External.exllamav2.exllamav2.generator import (
    base, sampler
)


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
                print(f"Response generated in {time_total:.2f} seconds, {max_new_tokens} tokens, "
                      f"{max_new_tokens / time_total:.2f} tokens/second")

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

config = model.ExLlamaV2Config()
# Get the current directory (where your script is)
current_dir = os.path.dirname(os.path.realpath(__file__))

# The path to the adjacent directory
adjacent_dir = os.path.join(current_dir, '../Models')

# The path to the file in the adjacent directory
model_directory = os.path.join(adjacent_dir, 'Dawn-v2-70B-2.55bpw-h6-exl2')

config.model_dir = model_directory
config.prepare()

model = model.ExLlamaV2(config)
print("Loading model: " + model_directory)

cache = cache.ExLlamaV2Cache(model, lazy=True)
model.load_autosplit(cache)

tokenizer = tokenizer.ExLlamaV2Tokenizer(config)

# Initialize generator

generator = base.ExLlamaV2BaseGenerator(model, cache, tokenizer)

# Generate some text

settings = sampler.ExLlamaV2Sampler.Settings()
settings.temperature = 1.25
settings.top_k = 0
settings.top_p = 0.8
settings.token_repetition_penalty = 1.15
settings.disallow_tokens(tokenizer, [tokenizer.eos_token_id])

max_new_tokens = 100
