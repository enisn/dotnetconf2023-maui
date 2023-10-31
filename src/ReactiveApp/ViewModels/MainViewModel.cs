﻿using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ReactiveApp.ViewModels;

public class MainViewModel : ReactiveObject, IActivatableViewModel
{
    [Reactive] public TodoItem NewItem { get; set; } = new(string.Empty);

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
            NewItem = new TodoItem(string.Empty);
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

public class TodoItem : BindableObject
{
    public TodoItem(string text, bool isCompleted = false)
    {
        Id = Guid.NewGuid();
        Text = text;
        IsCompleted = isCompleted;
        CreatedAt = DateTime.Now;
    }

    public Guid Id { get; }

    private string text;
    public string Text
    {
        get => text;
        set
        {
            text = value;
            OnPropertyChanged();
        }
    }

    private bool isCompleted;
    public bool IsCompleted
    {
        get => isCompleted;
        set
        {
            isCompleted = value;
            OnPropertyChanged();
        }
    }

    public DateTime CreatedAt { get; set; }
}