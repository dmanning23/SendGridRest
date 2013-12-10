using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace GetUnsubscribes
{
	class Program
	{
		static void Main(string[] args)
		{
			//Get the items out of the command line
			if (args.Length != 2)
			{
				Console.WriteLine("usage: GetUnsubscribes <username> <password>");
				return;
			}

			//First item is the sendgrid username
			string username = args[0];

			//second item it the sendgrid password
			string pwd = args[1];

			var client = new RestClient("https://api.sendgrid.com");
			var request = new RestRequest("api/unsubscribes.get.json");
			request.AddParameter("api_user", username); // used on every request
			request.AddParameter("api_key", pwd); // used on every request
			request.AddParameter("date", 1);

			IRestResponse response = client.Execute(request);
			string responseText = response.Content;

			var thing = JsonConvert.DeserializeObject<List<Unsubscription>>(response.Content);

			Console.WriteLine(responseText);
		}

		public class Unsubscription
		{
			[JsonProperty("email")]
			public string Email { get; set; }

			[JsonProperty("created")]
			public DateTime Created { get; set; }
		}
	}
}
