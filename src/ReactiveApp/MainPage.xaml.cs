using ReactiveApp.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;

namespace ReactiveApp;

public partial class MainPage : ReactiveContentPage<MainViewModel>
{
    public MainPage(MainViewModel mainView)
    {
        ViewModel = mainView;
        InitializeComponent();
        this.WhenActivated(_ => { });
    }
}

