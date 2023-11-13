import json
import os
import win32file
import win32pipe
import traceback

from multiprocessing import Process
from LiCObjects import (PipePayload, PipeResponse, CallType)
from LiCActions import (text_inference)
from exllamav2 import (model, cache, tokenizer, generator)
from exllamav2.generator import (base, sampler)

try:
    import flash_attn
except ImportError:
    print('Yo, exllamaV2 is WAYYYY better with flash_attn. It can be a pain to figure out, but it\'s worth it, trust!')


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

                stringified_data = data.decode()
                payload = PipePayload.from_json(stringified_data)

                match payload.CallType:
                    case CallType.Initialize:
                        print("Initializing...")

                        pipe_response = PipeResponse(Error=None, Code=200, Output="Initialized!", TimeTotal=None,
                                                     MaxNewTokens=None)
                    case CallType.Generate:
                        print("Generating...")

                        pipe_response = text_inference(payload, settings, generator)

                        # Write the result back to the named pipe
                        win32file.WriteFile(pipe, str.encode(json.dumps(pipe_response.__dict__).__str__()))
                    case CallType.SwapModel:
                        print("Swapping model...")

                        pipe_response = PipeResponse(Error=None, Code=200, Output="Swapped model!",
                                                     TimeTotal=None, MaxNewTokens=None)

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
model_directory = os.path.join(adjacent_dir, 'Athena-v3-13b-5.25bpw-6h-exl2')

config.model_dir = model_directory
config.prepare()

model = model.ExLlamaV2(config)
print("Loading model: " + model_directory)

cache = cache.ExLlamaV2Cache(model, lazy=True)
model.load_autosplit(cache)

tokenizer = tokenizer.ExLlamaV2Tokenizer(config)

# Initialize generator
generator = base.ExLlamaV2BaseGenerator(model, cache, tokenizer)

settings = sampler.ExLlamaV2Sampler.Settings()
settings.disallow_tokens(tokenizer, [tokenizer.eos_token_id])
