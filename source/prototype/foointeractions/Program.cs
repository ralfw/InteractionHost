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
			using (var p = new ConsoleServicePortal ()) {
				var input = p.Receive ();

				var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(input);

				if (dict["id"].IndexOf ("9") >= 0) {
					p.Send ("{'errormsg': 'Bummer!'}");
					return 1;
				} else {
					p.Send (string.Format("{'time': '{0}'}", DateTime.Now.ToString ()));
					return 0;
				}
			}
		}
	}


	public class ConsoleServicePortal : IDisposable {
		public string Receive() {
			var message = "";
			var l = "";
			while ((l = Console.ReadLine()) != null) {
				if (message != "") message += "\n";
				message += l;
			}
			return message;
		}

		public void Send(string message) {
			Console.Write(message);
		}

		#region IDisposable implementation
		public void Dispose () {}
		#endregion
	}
}
