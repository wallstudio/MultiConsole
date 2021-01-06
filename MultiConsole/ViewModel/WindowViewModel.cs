using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProgramWrapper;

namespace MultiConsole.ViewModel
{
    public class Property<T> : INotifyPropertyChanged
    {
        public Action<T> OnValueChange;
        event PropertyChangedEventHandler m_PropertyChanged;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => m_PropertyChanged += value;
            remove => m_PropertyChanged -= value;
        }

        T m_Value;
        public T Value
        {
            get => m_Value;
            set
            {
                m_Value = value;
                m_PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                OnValueChange?.Invoke(value);
            }
        }
        public static implicit operator Property<T>(T value) => new Property<T>() { Value = value };
        public static implicit operator T(Property<T> property) => property.Value;
    }


    public class Command : ICommand
    {
        public string Name { get; }
        public event EventHandler CanExecuteChanged;

        public event Action OnExecuted;
        public Command(Action command = null) : this(command?.Method.Name, command) { }
        public Command(string name, Action command = null)
        {
            Name = name;
            OnExecuted += command;
        }

        public bool CanExecute(object parameter) => OnExecuted != null;

        public void Execute(object parameter) => OnExecuted();
    }


    public class WindowViewModel
    {

        public ObservableCollection<SingleConsoleViewModel> Consoles { get; } = new ObservableCollection<SingleConsoleViewModel>();

        public Property<Visibility> InputVisibility { get; } = System.Windows.Visibility.Visible;
        public ICommand RunCommand => new Command(Run);
        public string Inputs { get; set; } = @"
..\..\..\..\TestRunApp\bin\Debug\netcoreapp3.1\TestRunApp.exe 10
..\..\..\..\TestRunApp\bin\Debug\netcoreapp3.1\TestRunApp.exe 7
..\..\..\..\TestRunApp\bin\Debug\netcoreapp3.1\TestRunApp.exe 8
    ".Trim();

        readonly MainWindow view;

        public WindowViewModel()
        {
            view = new MainWindow();
            view.DataContext = this;
            view.Show();
        }

        void Run()
        {
            var commands = Inputs.Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in commands)
            {
                Consoles.Add(new SingleConsoleViewModel(command));
            }
        }
    }

    public class SingleConsoleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string FileName => program.FileName;
        public string Argments => program.Argments;
        public StringBuilder Outputs { get; } = new StringBuilder();

        readonly Program program;


        public SingleConsoleViewModel(string command)
        {
            program = new Program(command);
            program.OnOutput += l =>
            {
                Outputs.AppendLine(l);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Outputs)));
            };
            _ = program.Execute();
        }
    }
}
