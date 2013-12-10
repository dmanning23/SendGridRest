using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Resubscribe
{
	class Program
	{
		static void Main(string[] args)
		{
			//Get the items out of the command line
			if (args.Length != 2)
			{
				Console.WriteLine("usage: Resubscribe <username> <password> <email>");
				return;
			}

			//First item is the sendgrid username
			string username = args[0];

			//second item is the sendgrid password
			string pwd = args[1];

			//third item is the email to resubscribe
			string email = args[2];

			//hit the sendgrid rest endpoint
			var client = new RestClient("https://api.sendgrid.com");
			var request = new RestRequest("api/unsubscribes.delete.json");
			request.AddParameter("api_user", username); // used on every request
			request.AddParameter("api_key", pwd); // used on every request
			request.AddParameter("email", email);

			IRestResponse response = client.Execute(request);
			string responseText = response.Content;

			var thing = JsonConvert.DeserializeObject<ResubscribeResponse>(response.Content);

			Console.WriteLine(responseText);
		}

		public class ResubscribeResponse
		{
			[JsonProperty("message")]
			public string Message { get; set; }

			[JsonProperty("errors")]
			public List<string> Errors { get; set; }
		}
	}
}
