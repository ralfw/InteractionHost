using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace foointeractions
{
	class MainClass
	{
		public static int Main (string[] args)
		{
			Console.Error.WriteLine ("foointeractions...");

			using (var p = new JsonConsoleServicePortal ()) {
				var input = p.Receive ();

				if (input["id"].ToString().IndexOf ("9") >= 0) {
					p.Send (new Dictionary<string, object>{{"errormsg", "Bummer!"}});
					return 1;
				} else {
					p.Send (new Dictionary<string, object>{{"time", DateTime.Now.ToString()}});
					return 0;
				}
			}
		}
	}


}
