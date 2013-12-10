using System.Text;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace GetUnsubscribesByDateRange
{
	class Program
	{
		static void Main(string[] args)
		{
			//Get the items out of the command line
			if (args.Length != 4)
			{
				Console.WriteLine("usage: GetUnsubscribesByDateRange <username> <password> <start date> <end date>");
				return;
			}

			//First item is the sendgrid username
			string username = args[0];

			//second item it the sendgrid password
			string pwd = args[1];

			//third item is the start date
			DateTime start = Convert.ToDateTime(args[2]);
			StringBuilder strStart = new StringBuilder();
			strStart.Append(start.Year);
			strStart.Append("-");
			strStart.Append(start.Month);
			strStart.Append("-");
			strStart.Append(start.Day);

			//last item is the end date
			DateTime end = Convert.ToDateTime(args[3]);
			StringBuilder strEnd = new StringBuilder();
			strEnd.Append(end.Year);
			strEnd.Append("-");
			strEnd.Append(end.Month);
			strEnd.Append("-");
			strEnd.Append(end.Day);

			var client = new RestClient("https://api.sendgrid.com");
			var request = new RestRequest("api/unsubscribes.get.json");
			request.AddParameter("api_user", username); // used on every request
			request.AddParameter("api_key", pwd); // used on every request
			request.AddParameter("date", 1);
			request.AddParameter("start_date", ConvertToSendGridDateTime(start));
			request.AddParameter("end_date", ConvertToSendGridDateTime(end));

			IRestResponse response = client.Execute(request);
			string responseText = response.Content;

			var thing = JsonConvert.DeserializeObject<List<Unsubscription>>(response.Content);

			Console.WriteLine(request.ToString());

			Console.WriteLine(responseText);
		}

		/// <summary>
		/// SendGrid requires dates to be in YYYY-MM-DD format or it shits the bed.
		/// </summary>
		/// <param name="dt">DateTime object to convert</param>
		/// <returns>correct string for SendGrid</returns>
		public static string ConvertToSendGridDateTime(DateTime dt)
		{
			StringBuilder strDateTime = new StringBuilder();
			strDateTime.Append(dt.Year);
			strDateTime.Append("-");
			strDateTime.Append(dt.Month);
			strDateTime.Append("-");
			strDateTime.Append(dt.Day);

			return strDateTime.ToString();
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
