using ImageAbridged.Services.Core;
using System.Diagnostics;

namespace ImageAbridged.Services
{
	public class ProcessInfo : IProcessInfo
	{
		public void ExecuteNoWindowRedirectProcess(string filename, string arguments)
		{
			using (var process = new Process
			{
				StartInfo = { FileName = filename, Arguments = arguments,
							  CreateNoWindow = true, ErrorDialog = false,
							  UseShellExecute = false, RedirectStandardOutput = false,
							  RedirectStandardError = false },
				EnableRaisingEvents = true
			})
			{
				process.Start();
				process.WaitForExit();
			}
		}

		public void ExecuteWindowRedirectProcess(string filename, string arguments)
		{
			using (var process = new Process
			{
				StartInfo = { FileName = filename, Arguments = arguments,
							  CreateNoWindow = false, ErrorDialog = false,
							  UseShellExecute = false, RedirectStandardOutput = true,
							  RedirectStandardError = true,
							  WindowStyle = ProcessWindowStyle.Hidden },
				EnableRaisingEvents = true
			})
			{
				process.Start();
				process.WaitForExit();
			}
		}

		public void ExecuteNodeProcess(string filename, string arguments)
		{
			using (var process = new Process
			{
				StartInfo = { CreateNoWindow = true, RedirectStandardInput = true,
							  RedirectStandardOutput = true, UseShellExecute = false,
							  RedirectStandardError = true, FileName = "node.exe",
							  Arguments = "-i"}
			})
			{
				process.Start();
				process.WaitForExit();
			}
		}
	}
}
