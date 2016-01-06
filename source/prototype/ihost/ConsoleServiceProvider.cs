using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Nancy;

namespace ihost
{

	public class ConsoleServiceProvider : IDisposable
	{
		private readonly string serviceFilepath;
		private readonly string commandlineParams;

		public ConsoleServiceProvider(string serviceFilepath) : this(serviceFilepath, "") { }
		public ConsoleServiceProvider(string serviceFilepath, string commandlineParams)
		{
			this.commandlineParams = commandlineParams;
			this.serviceFilepath = serviceFilepath;
		}

		public void Process(string input, out int exitCode, out string output) { Process(this.commandlineParams, input, out exitCode, out output); }
		public void Process(string commandlineParams, string input, out int exitCode, out string output)
		{
			var p = new Process
			{
				StartInfo = new ProcessStartInfo(this.serviceFilepath, commandlineParams)
				{
					UseShellExecute = false,
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};
			p.Start();

			p.StandardInput.Write(input);
			p.StandardInput.Close();

			var outputLines = new List<string>();
			p.OutputDataReceived += (sender, e) => outputLines.Add(e.Data);
			p.BeginOutputReadLine();

			p.WaitForExit ();

			exitCode = p.ExitCode;
			output = string.Join("\n", outputLines);
		}

		#region IDisposable implementation
		public void Dispose() { }
		#endregion
	}
}
