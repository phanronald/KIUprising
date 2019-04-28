namespace ImageAbridged.Services.Core
{
	public interface IProcessInfo
	{
		void ExecuteWindowRedirectProcess(string filename, string arguments);
		void ExecuteNoWindowRedirectProcess(string filename, string arguments);
		void ExecuteNodeProcess(string filename, string arguments);
	}
}
