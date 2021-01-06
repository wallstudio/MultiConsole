using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramWrapper
{
    public class Program
    {
        public event Action<string> OnOutput;
        public event Action OnExit;
        public string FileName => process.StartInfo.FileName;
        public string Argments => process.StartInfo.Arguments;
        public IReadOnlyList<string> Outputs => outputs;

        readonly Process process;
        readonly List<string> outputs = new List<string>();


        public Program(string command)
        {
            string fileName, argments;
            if (command[0] == '\"')
            {
                fileName = new string(command.Skip(1).TakeWhile(c => c != '\"').ToArray());
                argments = new string(command.Skip(1).SkipWhile(c => c != '\"').Skip(1).ToArray());
            }
            else
            {
                fileName = command.Split(' ').First();
                argments = string.Join(" ", command.Split(' ').Skip(1));
            }

            process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = fileName,
                    Arguments = argments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                },
                EnableRaisingEvents = true,
            };
            void OnOutputHandle(object sender, DataReceivedEventArgs e)
            {
                lock(outputs)
                {
                    outputs.Add(e.Data);
                    OnOutput?.Invoke(e.Data);
                }
            }
            process.OutputDataReceived += OnOutputHandle;
            process.ErrorDataReceived += OnOutputHandle;
            process.Exited += (_, e) => OnExit?.Invoke();
        }

        public async Task Execute()
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await Task.Run(() => process.WaitForExit());
        }
    }
}
