using WordleFinalProject.ViewModel;

namespace WordleFinalProject;
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent(); // Correctly initialize components
        InitializeViewModelAsync(); // Call an async method to initialize ViewModel
    }
    private async void InitializeViewModelAsync()
    {
        WordleViewModel viewModel = await WordleViewModel.CreateAsync(); // Asynchronously create ViewModel
        BindingContext = viewModel; // Set the ViewModel as the BindingContext
    }
}