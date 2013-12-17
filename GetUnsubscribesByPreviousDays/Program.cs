using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace GetUnsubscribesByPreviousDays
{
	class Program
	{
		static void Main(string[] args)
		{
			//Get the items out of the command line
			if (args.Length != 3)
			{
				Console.WriteLine("usage: GetUnsubscribesByPreviousDays <username> <password> <num days>");
				return;
			}

			//First item is the sendgrid username
			string username = args[0];

			//second item it the sendgrid password
			string pwd = args[1];

			//third item is the number of days to get records for
			int numDays = Convert.ToInt32(args[2]);

			var client = new RestClient("https://api.sendgrid.com");
			var request = new RestRequest("api/unsubscribes.get.json");
			request.AddParameter("api_user", username); // used on every request
			request.AddParameter("api_key", pwd); // used on every request
			request.AddParameter("date", 1);
			request.AddParameter("start_date", ConvertToSendGridDateTime(DateTime.Now.Subtract(new TimeSpan(24 * numDays, 0, 0))));
			request.AddParameter("end_date", ConvertToSendGridDateTime(DateTime.Now));

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
			return string.Format("{0:yyyy-MM-dd}", dt);
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
