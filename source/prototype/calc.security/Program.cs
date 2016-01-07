using System;
using System.Collections.Generic;
using System.Linq;

namespace calc.security
{
	class MainClass
	{
		public static int Main (string[] args)
		{
			Console.Error.WriteLine ("calc.security {0}...", args[0]);

			using (var p = new JsonConsoleServicePortal ()) {
				var input = p.Receive ();

				Console.Error.WriteLine ("User: {0}, password: {1}", input["Username"], input["Password"]) ;

				p.Send (new Dictionary<string, object>{
					{"Failed", true},
					{"Username", input["Username"]},
					{"Errormsg", "Unknown user name or invalid password!"}
				});
				return 1;

			};
		}
	}
}
