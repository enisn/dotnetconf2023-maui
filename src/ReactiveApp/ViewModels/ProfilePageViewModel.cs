using ReactiveApp.Sources;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Disposables;

namespace ReactiveApp.ViewModels;
public class ProfilePageViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive] public string UserName { get; set; }

    public ProfilePageViewModel(ProfileDataSource profileDataSource)
    {
        this.WhenActivated(disposables =>
        {
            profileDataSource.WhenAnyValue(s => s.UserName)
                .BindTo(this, vm => vm.UserName)
                .DisposeWith(disposables);

            this.WhenAnyValue(viewModel => viewModel.UserName)
                .BindTo(profileDataSource, source => source.UserName)
                .DisposeWith(disposables);
        });
    }
}
