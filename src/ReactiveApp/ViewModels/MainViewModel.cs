using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace ReactiveApp.ViewModels;

public class MainViewModel : ReactiveObject, IActivatableViewModel
{
    [Reactive] public TodoItem NewItem { get; set; } = new();

    private ReadOnlyObservableCollection<TodoItem> items;
    public ReadOnlyObservableCollection<TodoItem> Items => items;

    protected SourceList<TodoItem> ItemsSourceList { get; } = new();

    [Reactive] public string SearchTerm { get; set; }

    public ReactiveCommand<Unit, Unit> AddCommand { get; }

    public ReactiveCommand<TodoItem, Unit> RemoveCommand { get; }

    public ViewModelActivator Activator { get; } = new();

    public MainViewModel()
    {
        AddCommand = ReactiveCommand.Create(() =>
        {
            ItemsSourceList.Add(NewItem);
            NewItem = new TodoItem();
        });

        RemoveCommand = ReactiveCommand.Create<TodoItem>(item =>
        {
            ItemsSourceList.Remove(item);
        });

        this.WhenActivated(disposables =>
        {
            var observableFilter = this
                .WhenAnyValue(viewModel => viewModel.SearchTerm)
                .Select(MakeFilter);

            var observableSort = ItemsSourceList.Connect()
                .WhenValueChanged(x => x.IsCompleted)
                .Select(MakeSort);

            ItemsSourceList.Connect()
                .Filter(observableFilter)
                .Sort(observableSort)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out items)
                .Subscribe()
                .DisposeWith(disposables);

            this.RaisePropertyChanged(nameof(Items));
        });
    }

    private Func<TodoItem, bool> MakeFilter(string term)
    {
        return x => string.IsNullOrEmpty(term) || x.Text.Contains(term);
    }

    private SortExpressionComparer<TodoItem> MakeSort(bool isCompleted)
    {
        return SortExpressionComparer<TodoItem>.Ascending(t => t.IsCompleted)
            .ThenByAscending(x => x.CreatedAt);
    }
}

public class TodoItem : ReactiveObject
{
    public TodoItem()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public Guid Id { get; }

    [Reactive] public string Text { get; set; } = string.Empty;

    [Reactive] public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; }
}