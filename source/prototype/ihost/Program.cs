using System;
using Nancy;

namespace ihost
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			using (var host = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:8080")))
			{
				host.Start();

				Console.WriteLine ("Press ENTER to stop...");
				Console.ReadLine();
			}
		}
	}


	public class NancyPortal : NancyModule {
		public NancyPortal() {
			Get ["/"] = _ => {
				Console.WriteLine("called at /");
				return "Hello: " + DateTime.Now.ToString ();
			};
		}
	}
}
