using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Scorpio.ProcessRunner
{
    public class GenericProcessRunner : IGenericProcessRunner
    {
        private readonly ILogger<GenericProcessRunner> _logger;

        public GenericProcessRunner(ILogger<GenericProcessRunner> logger)
        {
            _logger = logger;
        }

        public string RunCommand(string command)
        {
            string stdOutput = string.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var escapeCommand = command.Replace("\"", "\\\"");

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{escapeCommand}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true
                    },
                };

                stdOutput = TryToRunProcess(process);
            }
            else
            {
                _logger.LogWarning("You cannot start this process on windows machine");
            }

            return stdOutput;
        }

        private string TryToRunProcess(Process process)
        {
            string stdOutput = string.Empty;

            try
            {
                process.Start();
                stdOutput = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return stdOutput;
        }
    }
}
