using ReactiveApp.ViewModels;

namespace ReactiveApp;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
