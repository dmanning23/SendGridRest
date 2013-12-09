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
			if (args.Length < 2)
			{
				Console.WriteLine("usage: Resubscribe <username> <password> <email>");
				return;
			}

			//First item is the sendgrid username
			string username = args[0];

			//second item it the sendgrid password
			string pwd = args[1];

			//third item it the sendgrid password
			string email = args[2];

			/*
			 * POST 	https://api.sendgrid.com/api/unsubscribes.delete.json
POST Data 	api_user=your_sendgrid_username&api_key=your_sendgrid_password&email=emailToDelete@domain.com
			 * */

			var client = new RestClient();
			client.BaseUrl = "https://api.sendgrid.com";
			var request = new RestRequest("api/unsubscribes.delete.json");
			request.AddParameter("api_user", username); // used on every request
			request.AddParameter("api_key", pwd); // used on every request
			request.AddParameter("email", email); // used on every request

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
