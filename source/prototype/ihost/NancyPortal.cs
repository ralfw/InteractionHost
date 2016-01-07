using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Nancy;
using Newtonsoft.Json;

namespace ihost
{
	public class NancyPortal : NancyModule {
		public NancyPortal() {
			var interactionLines = System.IO.File.ReadLines ("interactions.txt");
			var interactions = interactionLines
				.Where(l => !string.IsNullOrWhiteSpace(l))
				.Select (l => {
					var lcols = l.Split(';');

					var routecols = lcols[0].Split(':');
					var route = routecols.Length > 1 ? routecols[1] : routecols[0];
					var method = routecols.Length > 1 ? routecols[0] : "GET";

					var responses = lcols.Skip(2).Select(r => {
						var respcols = r.Split(':');
						return new {exitcode = int.Parse(respcols[0]), viewpath = respcols[1]};
					});
					return new {method = method.ToUpper(), route = route, cmd = lcols[1], responses = responses};
			});

			foreach (var i in interactions) {
				if (i.method == "GET")
					Get [i.route] = routeparams => Handle (routeparams, i.route, i.cmd, i.responses);
				else if (i.method == "POST")
					Post [i.route] = routeparams => Handle (routeparams, i.route, i.cmd, i.responses);
				else
					throw new InvalidOperationException (string.Format("Invalid HTTP method {0} for route {1}! Only GET and POST allowed.", 
														 			   i.method, i.route));
			}
		}


		dynamic Handle(DynamicDictionary routeparams, string route, string cmd, IEnumerable<dynamic> responses) {
			Console.WriteLine ("route: {0}, cmd: {1}", route, cmd);

			var d = (DynamicDictionary)routeparams;
			var jd = new Dictionary<string,string>();
			foreach (var k in d.Keys) jd[k] = d[k];
			d = (DynamicDictionary)Request.Query;
			foreach (var k in d.Keys) jd[k] = d[k];
			d = (DynamicDictionary)Request.Form;
			foreach (var k in d.Keys) jd[k] = d[k];

			var input = Newtonsoft.Json.JsonConvert.SerializeObject(jd, Formatting.Indented);
			Console.WriteLine(input);

			int exitCode = 0;
			var output = "";

			if (cmd != "") {
				var cmdParts = cmd.Split(new[]{' '}, 2);
				Console.WriteLine("run {0} with: {1}", cmdParts[0], cmdParts[1]);

				using(var cli = new ConsoleServiceProvider(cmdParts[0], cmdParts[1])) {
					cli.Process(input, out exitCode, out output);

					Console.WriteLine("exited with: {0}", exitCode);

					if (string.IsNullOrWhiteSpace (output))	output = "{}";
					Console.WriteLine(output);
				}
			}
			else
				output = input;

			var response = responses.FirstOrDefault(r => r.exitcode == exitCode);
			if (response != null) {
				Console.WriteLine ("viewpath: {0}", response.viewpath);
				var viewmodel = JsonConvert.DeserializeObject<Dictionary<string, object>>(output);
				return View[response.viewpath, viewmodel];
			}
			else
				return output;
		}
	}
}
