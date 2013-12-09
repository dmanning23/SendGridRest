using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace Unsubscribe
{
	class Program
	{
		static void Main(string[] args)
		{
			//Get the items out of the command line
			if (args.Length < 2)
			{
				Console.WriteLine("usage: Unsubscribe <username> <password> <email>");
				return;
			}

			//First item is the sendgrid username
			string username = args[0];

			//second item is the sendgrid password
			string pwd = args[1];

			//third item is the email to unsubscribe
			string email = args[2];

			//hit the sendgrid rest endpoint
			var client = new RestClient();
			client.BaseUrl = "https://api.sendgrid.com";
			var request = new RestRequest("api/unsubscribes.add.json");
			request.AddParameter("api_user", username); // used on every request
			request.AddParameter("api_key", pwd); // used on every request
			request.AddParameter("email", email);

			IRestResponse response = client.Execute(request);
			string responseText = response.Content;

			var thing = JsonConvert.DeserializeObject<UnsubscribeResponse>(response.Content);

			Console.WriteLine(responseText);
		}

		public class UnsubscribeResponse
		{
			[JsonProperty("message")]
			public string Message { get; set; }

			[JsonProperty("errors")]
			public List<string> Errors { get; set; }
		}
	}
}
