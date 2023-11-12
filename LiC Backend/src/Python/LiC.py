import json
import os
import time
import win32file
import win32pipe
import traceback

from enum import Enum
from multiprocessing import Process
from dataclasses import dataclass
from typing import Optional

try:
    import flash_attn
except ImportError:
    print('Yo, exllamaV2 is WAYYYY better with flash_attn. It can be a pain to figure out, but it\'s worth it, trust!')

from exllamav2 import (
    model, cache, tokenizer, generator
)

from exllamav2.generator import (
    base, sampler
)


class CallType(Enum):
    Initialize = 0
    Generate = 1
    SwapModel = 2

    @classmethod
    def from_value(cls, value):
        # Find the enum member with the matching value
        for member in cls:
            if member.value == value:
                return member
        # If no matching value is found, raise an error
        raise ValueError(f"No matching enum for value: {value}")


@dataclass
class PipePayload:
    CallType: CallType
    InputText: Optional[str]
    PathToModel: Optional[str]

    @staticmethod
    def from_json(json_data):
        parsed_json = json.loads(json_data)
        parsed_json['CallType'] = CallType.from_value(parsed_json['CallType'])
        return PipePayload(**parsed_json)


@dataclass
class PipeResponse:
    Error: Optional[str]
    Code: int
    Output: Optional[str]


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

                stringified_data = data.decode()

                payload = PipePayload.from_json(stringified_data)

                output = generator.generate_simple(payload.InputText, settings, max_new_tokens)

                time_end = time.time()
                time_total = time_end - time_begin

                print(f"Generated text:\n{output}\n")
                print(f"Response generated in {time_total:.2f} seconds, {max_new_tokens} tokens, "
                      f"{max_new_tokens / time_total:.2f} tokens/second")

                pipe_response = PipeResponse(Error=None, Code=200, Output=output)

                # Write the result back to the named pipe
                win32file.WriteFile(pipe, str.encode(json.dumps(pipe_response.__dict__).__str__()))

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

# Generate some text

settings = sampler.ExLlamaV2Sampler.Settings()
settings.temperature = 1.25
settings.top_k = 0
settings.top_p = 0.8
settings.token_repetition_penalty = 1.15
settings.disallow_tokens(tokenizer, [tokenizer.eos_token_id])

max_new_tokens = 100