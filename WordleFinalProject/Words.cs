using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace WordleFinalProject;
public class Words
{
    List<string> words = new List<string>();
    HttpClient httpClient;
    string savedfilelocation = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "words.txt");
    public async Task getWordList()
    {
        if (File.Exists(savedfilelocation))
        {
            ReadFileIntoList();
        }
        else
        {
            await DownloadFile();
            ReadFileIntoList();
        }
    }
    public void ReadFileIntoList()
    {
        // Ensure the file exists before trying to read it
        if (!File.Exists(savedfilelocation))
        {
            Console.WriteLine($"No file found at {savedfilelocation}");
            return;
        }

        try
        {
            // Open the file and read line by line
            using StreamReader sr = new StreamReader(savedfilelocation);
            string word;
            while ((word = sr.ReadLine()) != null)
            {
                words.Add(word);
                Console.WriteLine($"Read word: {word}");  // Debugging statement
            }
        }
        catch (Exception ex)
        {
            // Log any exceptions encountered during reading
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }
    public async Task DownloadFile()
    {
        using (var httpClient = new HttpClient())
        {
            var responseStream = await httpClient.GetStreamAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");
            using var fileStream = new FileStream(savedfilelocation, FileMode.Create);
            await responseStream.CopyToAsync(fileStream);
        }
    }
    public String GenerateRandomWord()
    {
        if (words == null || words.Count == 0)
            throw new InvalidOperationException("Word list is empty or not initialized.");

        Random random = new Random();
        int which = random.Next(words.Count);
        return words[which].Trim().ToUpper();
    }
}
