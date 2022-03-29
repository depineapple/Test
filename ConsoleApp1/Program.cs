using ConsoleApp1;

JsonMethods jsonMethods = new JsonMethods();
List<string> resultsList = new List<string>(), dataList = new List<string>();
List<Replacement> replacementsList;
string? rawData = null;


dataList = jsonMethods.getDataJsonList().Result;
replacementsList = jsonMethods.getReplacementJsonListAsync().Result;
doReplacements();

void doReplacements()
{
    foreach (var dataRaw in dataList)
    {
        replacementsList = replacementsList.FindAll(r => dataRaw.Contains(r.replacement));
        if (replacementsList.Count > 1)
        {
            for (int j = 0; j < replacementsList.Count - 1; j++)
            {
                if (replacementsList[j].replacement.Contains(replacementsList[j + 1].replacement) || replacementsList[j + 1].replacement.Contains(replacementsList[j].replacement))
                    if (replacementsList[j].replacement.Length > replacementsList[j + 1].replacement.Length)
                        replacementsList.RemoveAt(j + 1);
                    else replacementsList.RemoveAt(j);
            }
            for (int j = 0; j < replacementsList.Count - 1; j++)
            {
                if (replacementsList[j].replacement == replacementsList[j + 1].replacement && replacementsList[j].source == replacementsList[j + 1].source)
                    replacementsList.RemoveAt(j);
            }
            for (int j = 0; j < replacementsList.Count - 1; j++)
            {
                if (replacementsList[j].replacement == replacementsList[j + 1].replacement && replacementsList[j].source != replacementsList[j + 1].source)
                    replacementsList.RemoveAt(j);
            }
        }
        foreach (var rep in replacementsList)
        {
            rawData = dataRaw.Replace(dataRaw, dataRaw.Replace(rep.replacement, rep.source));
        }
        if (dataRaw != "")
        {
            Console.WriteLine(dataRaw);
            resultsList.Add(dataRaw);
        }
    }
}
