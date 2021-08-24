using LoggerService;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Ecommerce.Tests
{
	public class XunitLogger : ILoggerManager, IDisposable
	{
		private ITestOutputHelper _output;

		public XunitLogger(ITestOutputHelper output)
		{
			_output = output;
		}

		public void Dispose()
		{
		}

		public void LogInfo(string message)
		{
			_output.WriteLine(message);
		}

		public void LogWarn(string message)
		{
			_output.WriteLine(message);
		}

		public void LogDebug(string message)
		{
			_output.WriteLine(message);
		}

		public void LogError(string message)
		{
			_output.WriteLine(message);
		}
	}
}
