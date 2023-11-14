import json
import os
import socket
import traceback
import argparse

from multiprocessing import Process
from LiCObjects import (SocketPayload, SocketResponse, CallType)
from LiCActions import (text_inference, swap_model)
from exllamav2 import (model, cache, tokenizer)
from exllamav2.generator import (base, sampler)

try:
    import flash_attn
except ImportError:
    print('Yo, exllamaV2 is WAYYYY better with flash_attn. It can be a pain to figure out, but it\'s worth it, trust!')


parser = argparse.ArgumentParser(formatter_class=lambda prog: argparse.HelpFormatter(prog, max_help_position=54))
parser.add_argument('--model', type=str, help='Needs the full path for the model, just to the folder for exl2'
                                              'or the model itself for gguf.')
args = parser.parse_args()

# Get the current directory (where your script is)
current_dir = os.path.dirname(os.path.realpath(__file__))

# The path to the adjacent directory
adjacent_dir = os.path.join(current_dir, '..', 'Models')

config = model.ExLlamaV2Config()

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


def start_server(host='localhost', port=5002):
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.bind((host, port))
        s.listen()

        print(f"Listening on {host}:{port}")
        while True:
            # Accept connections
            conn, addr = s.accept()
            print(f"Connected by {addr}")

            with conn:
                while True:
                    data = conn.recv(65536)
                    if not data:
                        break
                    print(f"Received data: {data}")

                    # Process the data
                    processed_data = process_data(data)

                    response = str.encode(json.dumps(processed_data.__dict__).__str__())

                    # Send response back to client
                    conn.sendall(response)

                    break


def process_data(data):
    socket_response = None

    stringified_data = data.decode()
    payload = SocketPayload.from_json(stringified_data)

    match payload.CallType:
        case CallType.Initialize:
            print("Initializing...")

            # I need to think about if this is even necessary or not...
            socket_response = SocketResponse(Code=200, Output="Initialized!")
        case CallType.Generate:
            print("Generating...")

            # Generate text
            socket_response = text_inference(payload, generator, settings)

        case CallType.SwapModel:
            print("Swapping model...")

            # Swap the model
            socket_response = swap_model(payload.PathToModel)

    return socket_response


if __name__ == "__main__":
    start_server()
