using ReactiveApp.Sources;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace ReactiveApp.ViewModels;
public class AppShellViewModel : ReactiveObject
{
    public AppShellViewModel(ProfileDataSource profileDataSource)
    {
        profileDataSource
            .WhenAnyValue(source => source.UserName)
            .Select(username => $"Welcome, {username}!")
            .BindTo(this, vm => vm.HeaderMessage);
    }

    [Reactive] public string HeaderMessage { get; set; }
}
