using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace calc.engine
{
	public class JsonConsoleServicePortal : IDisposable {
		public Dictionary<string,object> Receive() {
			var json = "";
			var l = "";
			while ((l = Console.ReadLine()) != null) {
				if (json != "") json += "\n";
				json += l;
			}
			return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
		}

		public void Send(Dictionary<string,object> data) {
			var json = JsonConvert.SerializeObject(data, Formatting.Indented);
			Console.Write(json);
		}

		#region IDisposable implementation
		public void Dispose () {}
		#endregion
	}
}
