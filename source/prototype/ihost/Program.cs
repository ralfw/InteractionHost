using System;
using System.Collections.Generic;
using System.Linq;
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
			var interactionLines = System.IO.File.ReadLines ("interactions.txt");
			var interactions = interactionLines.Select (l => {
				var lcols = l.Split(';');
				var responses = lcols.Skip(2).Select(r => {
					var rcols = r.Split(':');
					return new {exitcode = rcols[0], filepath = rcols[1]};
				});
				return new {route = lcols[0], cmd = lcols[1], responses = responses};
			});


			foreach (var i in interactions) {
				Get [i.route] = routeparams => {
					Console.WriteLine ("route: {0}, cmd: {1}", i.route, i.cmd);

					var input = "";
					var d = (DynamicDictionary)routeparams;
					foreach (var k in d.Keys) {
						if (input != "")
							input += "\n";
						input += "route." + k + ":" + d [k];
					};
					d = (DynamicDictionary)Request.Query;
					foreach (var k in d.Keys) {
						if (input != "")
							input += "\n";
						input += "query." + k + ":" + d [k];
					};
					d = (DynamicDictionary)Request.Form;
					foreach (var k in d.Keys) {
						if (input != "")
							input += "\n";
						input += "form." + k + ":" + d [k];
					};

					Console.WriteLine(input);

					return Response.AsRedirect(i.responses.First().filepath);
				};
			}


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
