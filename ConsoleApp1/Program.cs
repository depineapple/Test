using System.Text.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ConsoleApp1;
using System.Linq;

var path = "https://raw.githubusercontent.com/thewhitesoft/student-2022-assignment/main/data.json";
HttpClient client = new HttpClient();
client.BaseAddress = new Uri(path);
Replacement storage = new Replacement();
List<Replacement> replacement;       
var streamTask = client.GetStreamAsync(path);
List<string> storages = await JsonSerializer.DeserializeAsync<List<string>>(await streamTask);
using (FileStream fs = new FileStream("replacement.json", FileMode.OpenOrCreate))
{
    replacement = await JsonSerializer.DeserializeAsync<List<Replacement>>(fs);
}
string n = "";
List<string> result = new List<string>();
foreach (var s in storages)
{
    n = s;
    List<string> words = new List<string>();
    List<Replacement> replacements = replacement.FindAll(r => s.Contains(r.replacement));
    if (replacements.Count > 1)
    {
        for (int j = 0; j < replacements.Count - 1; j++)
        {
            if (replacements[j].replacement.Contains(replacements[j + 1].replacement) || replacements[j + 1].replacement.Contains(replacements[j].replacement))
                if (replacements[j].replacement.Length > replacements[j + 1].replacement.Length)
                    replacements.RemoveAt(j + 1);
                else replacements.RemoveAt(j);
        }
        for (int j = 0; j < replacements.Count - 1; j++)
        {
            if (replacements[j].replacement == replacements[j + 1].replacement && replacements[j].source == replacements[j + 1].source)
                replacements.RemoveAt(j);
        }
        for (int j = 0; j < replacements.Count - 1; j++)
        {
            if (replacements[j].replacement == replacements[j + 1].replacement && replacements[j].source != replacements[j + 1].source)
                replacements.RemoveAt(j);
        }
       
    }
    foreach (var rep in replacements)
    {
        n = n.Replace(n, n.Replace(rep.replacement, rep.source));
    }
    if (n != "")
    {
        Console.WriteLine(n);
        result.Add(n);
    }
}
using (FileStream fs = new FileStream("result.json", FileMode.OpenOrCreate))
{
    var options = new JsonSerializerOptions
    {
        WriteIndented = true

    };
    await JsonSerializer.SerializeAsync<List<string>>(fs, result, options);
    Console.WriteLine();
    Console.WriteLine("Данные были сериализованы");
}
