using System.Text.Json;

namespace ConsoleApp1
{
    internal class JsonMethods
    {
        string pathDataJson = "https://raw.githubusercontent.com/thewhitesoft/student-2022-assignment/main/data.json";
        string pathReplacementJson = "replacement.json";
        HttpClient httpClient = new HttpClient();

        public async Task<List<string>> getDataJsonList()
        {

            httpClient.BaseAddress = new Uri(pathDataJson);
            var streamTask = httpClient.GetStreamAsync(pathDataJson);
            var dataList = await JsonSerializer.DeserializeAsync<List<string>>(await streamTask);           
            return dataList;
        }
        public async Task<List<Replacement>> getReplacementJsonListAsync()
        {
            List<Replacement> replacements;
            using (FileStream fs = new FileStream(pathReplacementJson, FileMode.OpenOrCreate))
            {
                replacements = await JsonSerializer.DeserializeAsync<List<Replacement>>(fs);
            }
            return replacements;
        }

        async Task resultToJsonAsync(List<string> results)
        {
            using (FileStream fs = new FileStream("result.json", FileMode.OpenOrCreate))
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                await JsonSerializer.SerializeAsync(fs, results, options);
                Console.WriteLine();
                Console.WriteLine("Данные были сериализованы");
            }
        }

    }
}
