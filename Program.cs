using System;
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

            // Преобразуйте строку JSON в объект JObject
            JObject jsonObject = JObject.Parse(responseBody);

            // Получите массив результатов
            JArray results = (JArray)jsonObject["results"];

            Console.Write("Choose a Star Wars character: ");
            string hero = Console.ReadLine();

            List<string> heros = new List<string> {};

            foreach (JObject character in results)
            {
                heros.Add((string)character["name"]);
            }
            for (int i = 0; i < heros.Count; i++)
            {
                if (hero == heros[i])
                {
                    Console.WriteLine($"Name: " + heros[i]);
                    Console.WriteLine($"Height: " + (results[i]["height"]));
                    Console.WriteLine($"Mass: " + (results[i]["mass"]));
                    Console.WriteLine($"Hair color: " + (results[i]["hair_color"]));
                    Console.WriteLine($"Eye color: " + (results[i]["eye_color"]));
                    Console.WriteLine($"Gender: " + (results[i]["gender"]));
                    Console.WriteLine($"Date of birth: " + (results[i]["birth_year"]));
                    Console.WriteLine("Movies that had " + heros[i] + " in them:");
                    foreach (string film in (results[i]["films"]))
                    {
                        HttpResponseMessage response_film = await client.GetAsync(film);
                        string responseBodyFilm = await response_film.Content.ReadAsStringAsync();

                        // Преобразуйте строку JSON в объект JObject
                        JObject jsonObjectFilm = JObject.Parse(responseBodyFilm);
                        Console.WriteLine("Star Wars. Episode " + (string)(jsonObjectFilm["episode_id"]) + ": " + (string)(jsonObjectFilm["title"]));
                    }
                }
            }
        }
    }
}