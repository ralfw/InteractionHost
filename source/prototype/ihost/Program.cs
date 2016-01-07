using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Nancy;
using Newtonsoft.Json;

namespace ihost
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var homeUri = new Uri ("http://localhost:8080");

			using (var host = new Nancy.Hosting.Self.NancyHost(homeUri))
			{
				host.Start();

				Console.WriteLine ($"Interaction Host listening on {homeUri}");
				Console.WriteLine ("Press ENTER to stop...");
				Console.ReadLine();
			}
		}
	}
}