using ReactiveApp.Sources;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Disposables;

namespace ReactiveApp.ViewModels;
public class ProfilePageViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    public ProfileDataSource ProfileDataSource { get; }

    public ProfilePageViewModel(ProfileDataSource profileDataSource)
    {
        ProfileDataSource = profileDataSource;
    }
}
