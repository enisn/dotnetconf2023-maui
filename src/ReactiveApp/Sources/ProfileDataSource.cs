using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ReactiveApp.Sources;
public class ProfileDataSource : ReactiveObject
{
    [Reactive] public string UserName { get; set; } = "enisn";
}
