using CommunityToolkit.Mvvm.ComponentModel;

namespace WordleFinalProject.Model;
public class WordleModel
{
    public WordleModel()
    {
        Letters = new Letter[5]
        {
            new Letter(),
            new Letter(),
            new Letter(),
            new Letter(),
            new Letter()
        };
    }
    public Letter[] Letters { get; set; }
    public bool Validate(char[] correctAnswer)
    {
        int count = 0;

        for (int i = 0; i < Letters.Length; i++)
        {
            var letter = Letters[i];
            if (letter.Input == correctAnswer[i])
            {
                letter.Color = Colors.Green;
                count++;
            }
            else if (correctAnswer.Contains(letter.Input))
            {
                letter.Color = Colors.Orange;
            }
            else
            {
                letter.Color = Colors.Red;
            }
        }
        return count == 5;
    }
    public void Reset()
    {
        foreach (var letter in Letters)
        {
            letter.Reset(); // Ensure each Letter can be reset
        }
    }
}
public partial class Letter : ObservableObject
{
    public Letter()
    {
        Color = Colors.Black;
    }
    public void Reset()
    {
        Input = ' '; // Reset the letter input
        Color = Colors.Black; // Reset the color 
    }
    [ObservableProperty]
    private char input;

    [ObservableProperty]
    private Color color;
}
