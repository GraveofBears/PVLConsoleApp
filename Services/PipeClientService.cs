using System.IO.Pipes;
using System.Text.Json;
using System.Threading.Tasks;

namespace PVLConsoleApp.Services
{
    public class PipeClientService
    {
        private const string PipeName = "PVLPipe";

        public async Task<string> SendLoginRequestAsync(string username, string passwordHash)
        {
            var request = new
            {
                Username = username,
                PasswordHash = passwordHash
            };

            var json = JsonSerializer.Serialize(request);

            using var pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut);
            await pipeClient.ConnectAsync();

            using var writer = new StreamWriter(pipeClient) { AutoFlush = true };
            using var reader = new StreamReader(pipeClient);

            await writer.WriteLineAsync(json);
            return await reader.ReadLineAsync();
        }

        public async Task<string> SendFileTransferAsync(string filename, byte[] fileBytes, string sessionToken)
        {
            var request = new
            {
                Filename = filename,
                FileBytes = fileBytes,
                SessionToken = sessionToken
            };

            var json = JsonSerializer.Serialize(request);

            using var pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut);
            await pipeClient.ConnectAsync();

            using var writer = new StreamWriter(pipeClient) { AutoFlush = true };
            using var reader = new StreamReader(pipeClient);

            await writer.WriteLineAsync(json);
            return await reader.ReadLineAsync(); // Expect "OK" or "FAIL"
        }
    }
}
