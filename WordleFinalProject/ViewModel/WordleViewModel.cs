using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WordleFinalProject.Model;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WordleFinalProject.ViewModel;
public partial class WordleViewModel : ObservableObject
{
    int rowIndex;
    int columnIndex;
    private char[] correctAnswer;
    private int numberOfGuesses;
    private Words words = new Words();
    public char[] KeyboardRow1 { get; }
    public char[] KeyboardRow2 { get; }
    public char[] KeyboardRow3 { get; }
    [ObservableProperty]
    private WordleModel[] model;
    public WordleViewModel()
    {
        InitializeGameAsync();
        model = new WordleModel[6]
        {
            new WordleModel(),
            new WordleModel(),
            new WordleModel(),
            new WordleModel(),
            new WordleModel(),
            new WordleModel()
        };
        KeyboardRow1 = "QWERTYUIOP".ToCharArray();
        KeyboardRow2 = "ASDFGHJKL".ToCharArray();
        KeyboardRow3 = "<ZXCVBNM>".ToCharArray();
    }
    public static async Task<WordleViewModel> CreateAsync()
    {
        var viewModel = new WordleViewModel();
        await viewModel.InitializeGameAsync();
        return viewModel;
    }
    private async Task InitializeGameAsync()
    {
        await words.getWordList();  // Initialize and download words list
        var randomWord = words.GenerateRandomWord(); // Get a random word
        correctAnswer = randomWord.ToCharArray();
    }
    public int NumberOfGuesses
    {
        get => numberOfGuesses;
        set => SetProperty(ref numberOfGuesses, value);
    }
    public void Enter()
    {
        Debug.WriteLine("Enter method called.");

        if (columnIndex != 5)
            return;

        NumberOfGuesses++;
        Debug.WriteLine($"NumberOfGuesses incremented to: {NumberOfGuesses}");

        var correct = Model[rowIndex].Validate(correctAnswer);

        if (correct)
        {
            string successMessage = $"You've guessed the word in {NumberOfGuesses} guesses!";
            App.Current.MainPage.DisplayAlert("You Win!", successMessage, "OK");
            return;
        }
        if (rowIndex == 5) 
        {
            string answerString = new string(correctAnswer);
            App.Current.MainPage.DisplayAlert("Game Over!", $"You are out of turns. The correct word was {answerString}.", "OK");
        }
        else
        {
            rowIndex++;
            columnIndex = 0;
        }
    }
    [RelayCommand]
    public void EnterLetter(char letter)
    {
        if (letter == '>')
        {
            Enter();
            return;
        }

        if (letter == '<')
        {
            if (columnIndex == 0)
                return;
            columnIndex--;
            Model[rowIndex].Letters[columnIndex].Input = ' ';

            return;
        }

        if (columnIndex == 5)
            return;

        Model[rowIndex].Letters[columnIndex].Input = letter;
        columnIndex++;
    }
    [RelayCommand]
    public async Task RestartGame()
    {
        // Reset game state
        rowIndex = 0;
        columnIndex = 0;
        NumberOfGuesses = 0;

        // Re-initialize the game with a new random word
        await InitializeGameAsync();

        // Reset the game board model
        foreach (var wordleModel in Model) 
        {
            wordleModel.Reset(); 
        }
        OnPropertyChanged(nameof(Model));
    }
}
