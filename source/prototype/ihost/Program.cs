using System;
using Nancy;

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


	public class NancyPortal : NancyModule {
		public NancyPortal() {
			Get ["/{a}/{b}"] = p => {
				var d = (DynamicDictionary)p;
				Console.WriteLine("{0}", string.Join(",", d.Keys));

				d = (DynamicDictionary)Request.Query;
				Console.WriteLine("{0}", string.Join(",", d.Keys));

				Console.WriteLine("called at /{0}/{1}?id={2}", p.a, p.b, Request.Query["id"]);
				return "Hello: " + DateTime.Now.ToString ();
			};
		}
	}
}
