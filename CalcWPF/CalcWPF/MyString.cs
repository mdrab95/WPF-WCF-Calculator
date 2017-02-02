using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CalcWPF
{
    class MyString : INotifyPropertyChanged
    {
        private string fieldString;
        public string nFieldString
        {
            set
            {
                if (fieldString != value)
                {
                    fieldString = value;
                    OnPropertyChanged("nFieldString");
                }
            } 
            get
            {
                return fieldString;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

/*
public class RelayCommand : ICommand
{

    private Action<object> execute;

    private Predicate<object> canExecute;

    private event EventHandler CanExecuteChangedInternal;

    public RelayCommand(Action<object> execute)
        : this(execute, DefaultCanExecute)
    {
    }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        if (execute == null)
        {
            throw new ArgumentNullException("execute");
        }

        if (canExecute == null)
        {
            throw new ArgumentNullException("canExecute");
        }

        this.execute = execute;
        this.canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add
        {
            CommandManager.RequerySuggested += value;
            this.CanExecuteChangedInternal += value;
        }

        remove
        {
            CommandManager.RequerySuggested -= value;
            this.CanExecuteChangedInternal -= value;
        }
    }

    public bool CanExecute(object parameter)
    {
        return this.canExecute != null && this.canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        this.execute(parameter);
    }

    public void OnCanExecuteChanged()
    {
        EventHandler handler = this.CanExecuteChangedInternal;
        if (handler != null)
        {
            //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
            handler.Invoke(this, EventArgs.Empty);
        }
    }

    public void Destroy()
    {
        this.canExecute = _ => false;
        this.execute = _ => { return; };
    }

    private static bool DefaultCanExecute(object parameter)
    {
        return true;
    }
}
class MainWindowViewModel
{
    MainWindow o1;

    private ICommand eraseCommand;

    private ICommand toggleExecuteCommand { get; set; }

    private bool canExecute = true;

    public bool CanExecute
    {
        get
        {
            return this.canExecute;
        }

        set
        {
            if (this.canExecute == value)
            {
                return;
            }

            this.canExecute = value;
        }
    }

    public ICommand ToggleExecuteCommand
    {
        get
        {
            return toggleExecuteCommand;
        }
        set
        {
            toggleExecuteCommand = value;
        }
    }

    public ICommand EraseCommand
    {
        get
        {
            return eraseCommand;
        }
        set
        {
            eraseCommand = value;
        }
    }

    public MainWindowViewModel()
    {
        EraseCommand = new RelayCommand(EraseTextBoxes, param => this.canExecute);
        toggleExecuteCommand = new RelayCommand(ChangeCanExecute);
    }

    public void EraseTextBoxes(object obj)
    {
        o1 = new MainWindow();
        o1.Erase();
    }

    public void ChangeCanExecute(object obj)
    {
        canExecute = !canExecute;
    }




           private void OperationButton_Click(object sender, RoutedEventArgs eRoutedEventArgs)
        {
            if ((Operation.none != LastOperationSelected) || (Operation.result != LastOperationSelected))
            {
                ResultButton_Click(this, eRoutedEventArgs);
            }
            Button button = (Button)sender;
            switch (button.Content.ToString())
            {
                case "+":
                    LastOperationSelected = Operation.addition;
                    break;
                case "-":
                    LastOperationSelected = Operation.subtraction;
                    break;
                case "*":
                    LastOperationSelected = Operation.multiplication;
                    break;
                case "/":
                    LastOperationSelected = Operation.division;
                    break;
                case "+/-":
                    LastOperationSelected = Operation.signchange;
                    break;
                default:
                    MessageBox.Show("Nieznana operacja!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }
            MemoryText = DisplayText;
            OperationText = button.Content.ToString();
            DisplayText = string.Empty;          
        }

    
        private void NumberButton_Click(object sender, RoutedEventArgs eRoutedEventArgs)
        {
            if (Operation.result == LastOperationSelected)
            {
                DisplayText = string.Empty;
                LastOperationSelected = Operation.none;
            }
            Button button = (Button)sender;
            DisplayText += button.Content;
        }

            private void CommaButton_Click(object sender, RoutedEventArgs eRoutedEventArgs)
        {
            if (Operation.result == LastOperationSelected)
            {
                DisplayText = string.Empty;
                LastOperationSelected = Operation.none;
            }
            if ((DisplayText.Contains(',')) ||
                (0 == DisplayText.Length))
            {
                return;
            }
            DisplayText += ",";
        }

    
        private void ResultButton_Click(object sender, RoutedEventArgs eRoutedEventArgs)
        {
            if ((Operation.result == LastOperationSelected) ||
                (Operation.none == LastOperationSelected))
            {
                return;
            }
            if (string.IsNullOrEmpty(DisplayText))
            {
                DisplayText = "0";
            }
            switch (LastOperationSelected)
            {
                case Operation.addition:
                    try
                    {
                        DisplayText = Operations.AddNumber(double.Parse(MemoryText),
                      double.Parse(DisplayText)).ToString();
                    }
                    catch
                    {
                        DisplayText = String.Empty;
                    }
                    break;
                case Operation.subtraction:
                    try
                    {
                        DisplayText = Operations.SubNumber(double.Parse(MemoryText),
                            double.Parse(DisplayText)).ToString();
                    }
                    catch
                    {
                        DisplayText = String.Empty;
                    }
                    break;
                case Operation.multiplication:
                    try
                    {
                        DisplayText = Operations.MultNumber(double.Parse(MemoryText),
                            double.Parse(DisplayText)).ToString();
                    }
                    catch
                    {
                        DisplayText = String.Empty;
                    }
                    break;
                case Operation.division:
                    try
                    {
                        DisplayText = Operations.DivNumber(double.Parse(MemoryText),
                            double.Parse(DisplayText)).ToString();
                    }
                    catch
                    {
                        DisplayText = String.Empty;
                    }
                    break;
                case Operation.signchange:
                    try
                    {
                        DisplayText = Operations.SignChange(double.Parse(MemoryText)).ToString();
                    }
                    catch
                    {
                        DisplayText = String.Empty;
                    }
                    break;
            }
            LastOperationSelected = Operation.result;
            OperationText = string.Empty;
            MemoryText = string.Empty;
        }

    
            // Binding mt = new Binding() { Source = memoryText, Path = new PropertyPath("nFieldString") };
            // Binding ot = new Binding() { Source = operationText, Path = new PropertyPath("nFieldString") };
            // Binding dt = new Binding() { Source = displayText, Path = new PropertyPath("nFieldString") };

            // BindingOperations.SetBinding(textMemory, TextBox.TextProperty, mt);
            // BindingOperations.SetBinding(textOperation, TextBox.TextProperty, ot);
            // BindingOperations.SetBinding(textDisplay, TextBox.TextProperty, dt);

            <ListView Margin="10" Name="lvOperations">
            <ListView.View>
                <GridView>
                    <GridViewColumn Header="Guid" Width="240" DisplayMemberBinding="{Binding Guid}" />
                    <GridViewColumn Header="Date" Width="140" DisplayMemberBinding="{Binding Date}" />
                    <GridViewColumn Header="Operation" Width="160" DisplayMemberBinding="{Binding Operation}" />
                </GridView>
            </ListView.View>
        </ListView>

}*/
