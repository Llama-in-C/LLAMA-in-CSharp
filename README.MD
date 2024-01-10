<a href="https://discord.gg/W2cf6R2FhP"><img src="LiC Frontend/wwwroot/img/LiC_README_Banner.png" alt="cybernetic llama"></a>
Share the Discord: https://discord.gg/W2cf6R2FhP

# Prerequisites:
<ol>
  <li>https://dotnet.microsoft.com/en-us/download/dotnet/8.0</li>
  <li>https://dotnet.microsoft.com/en-us/download/dotnet/7.0/runtime?cid=getdotnetcore&os=windows&arch=x64</li>
    <ol>
      <li>Hosting Bundle for Windows. Linux/MacOS have only one option.</li>
    </ol>
  <li style="color:red"><strong>It is STRONGLY recommended that you load a virtual environment:</strong></li>
    <ol>
      <li>Conda is a popular option: <a href=" https://docs.conda.io/en/latest/miniconda.html">https://docs.conda.io/en/latest/miniconda.html</a></li>
      <li>There is also Python's venv: <a href="https://docs.python.org/3/library/venv">https://docs.python.org/3/library/venv.html</a></li>
    </ol>
  <li>Run the terminal command after setting up your preferred virtual environment:</li>
    
```shell
  pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu121
  ```
  <li>Run pip install on the requirements.txt file in the ..\src\Python directory</li>
</ol>

# Start up:
Now that you have the prerequisites installed, you can start up the project. This is done by via terminal commands,
the plan is to evolve it into a .bat/.sh file and then ultimately a .exe file in a future update.

## Important!
When initializing the server for the time being you must run the following command in the terminal:
```shell
  python LiC.py --model [path to your model here]
  ```

This is necessary as the script for the socket server is not yet smart enough to find the model on its own. 
This will be fixed in a future update.

When the Socket Server is initialized and the model is loaded, you will need to run the "LiC Backend" project,
this can be done using the following command in the terminal:
```shell
  dotnet run '.\LiC Backend.csproj'
  ```

Once the backend is running, you are able to call to the api using the following url:
```shell
  https://localhost:7108/TextGeneration/[Your endpoint here]
  ```

# Endpoints:
The following endpoints are currently available and intake a json payload.

## GenerateText:
This endpoint generates text based on the input text. The input text is used as a prompt for the model to generate text from.
Payload example:
```json5
{
  "CallType": 1, // int between 0 and 2. Currently only 1 = GenerateText is a valid value.
  "InputText": "If you could relate to one fictional character, who would it be?", // string input text
  "Temperature": 1.0, // float between 0.0 and 2.0
  "TopK": 0, // int between 0 and 100
  "TopP": 0.69, // float between 0.0 and 1.0
  "RepetitionPenalty": 1.0, // float between 1.0 and 1.5
  "MaxNewTokens": 400 // Limit of number of tokens returned by the model higher numbers can result in longer wait times and undesirable output.
}
```