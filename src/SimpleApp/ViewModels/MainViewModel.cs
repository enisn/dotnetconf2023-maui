using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SimpleApp.ViewModels;
public class MainViewModel : INotifyPropertyChanged
{
    private TodoItem newItem = new();
    public TodoItem NewItem
    {
        get => newItem;
        set
        {
            newItem = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<TodoItem> Items { get; } = new();

    public ICommand AddCommand { get; }

    public ICommand RemoveCommand { get; }

    public MainViewModel()
    {
        AddCommand = new Command(() =>
        {
            Items.Add(NewItem);
            NewItem = new TodoItem();
        });

        RemoveCommand = new Command<TodoItem>(item =>
        {
            Items.Remove(item);
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class TodoItem : BindableObject
{
    public TodoItem()
    {
        CreatedAt = DateTime.Now;
    }

    private string text = string.Empty;
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