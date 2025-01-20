using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = "https://swapi.dev/api/people/";
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            string responseBody = await response.Content.ReadAsStringAsync();

            // Convert JSON string to JObject
            JObject jsonObject = JObject.Parse(responseBody);

            // Get the array of results
            JArray results = (JArray)jsonObject["results"];

            Console.Write("Choose a Star Wars character: ");
            string heroInput = Console.ReadLine();

            List<string> heroes = new List<string>();

            foreach (JObject character in results)
            {
                heroes.Add((string)character["name"]);
            }

            for (int i = 0; i < heroes.Count; i++)
            {
                if (heroInput.Equals(heroes[i], StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Name: {heroes[i]}");
                    Console.WriteLine($"Height: {results[i]["height"]}");
                    Console.WriteLine($"Mass: {results[i]["mass"]}");
                    Console.WriteLine($"Hair Color: {results[i]["hair_color"]}");
                    Console.WriteLine($"Eye Color: {results[i]["eye_color"]}");
                    Console.WriteLine($"Gender: {results[i]["gender"]}");
                    Console.WriteLine($"Date of Birth: {results[i]["birth_year"]}");
                    Console.WriteLine($"Movies that had {heroes[i]} in them:");

                    foreach (string filmUrl in (results[i]["films"]))
                    {
                        HttpResponseMessage filmResponse = await client.GetAsync(filmUrl);
                        string filmResponseBody = await filmResponse.Content.ReadAsStringAsync();

                        // Convert JSON string to JObject
                        JObject filmJsonObject = JObject.Parse(filmResponseBody);
                        Console.WriteLine($"Star Wars. Episode {filmJsonObject["episode_id"]}: {filmJsonObject["title"]}");
                    }
                }
            }
        }
    }
}
