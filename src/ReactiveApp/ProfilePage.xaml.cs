using ReactiveApp.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;

namespace ReactiveApp;

public partial class ProfilePage : ReactiveContentPage<ProfilePageViewModel>
{
	public ProfilePage(ProfilePageViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        this.WhenActivated(_ => { });
    }
}