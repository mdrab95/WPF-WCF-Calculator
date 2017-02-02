using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace CalcWPF
{
    enum Operation
    {
        none,
        addition,
        subtraction,
        multiplication,
        division,
        signchange,
        result
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string memoryText;
        public string MemoryText
        {
            get
            {
                return memoryText;
            }
            set
            {
                if (MemoryText != value)
                {
                    memoryText = value;
                    OnPropertyChanged("MemoryText");
                }
            }
        }

        public string operationText;
        public string OperationText
        {
            get
            {
                return operationText;
            }
            set
            {
                if (OperationText != value)
                {
                    operationText = value;
                    OnPropertyChanged("OperationText");
                }
            }
        }

        private string displayText;
        public string DisplayText
        {
            get
            {
                return displayText;
            }
            set
            {
                if (DisplayText != value)
                {
                    displayText = value;
                    OnPropertyChanged("DisplayText");
                }
            }
        }

        private ICommand _eraseCommand;
        public ICommand EraseCommand
        {
            get
            {
                return _eraseCommand;
            }
            set
            {
                if (EraseCommand != value)
                {
                    _eraseCommand = value;
                    OnPropertyChanged("EraseCommand");
                }
            }
        }

        private ICommand _buttonNumberCommand;
        public ICommand ButtonNumberCommand
        {
            get
            {
                return _buttonNumberCommand;
            }
            set
            {
                if (ButtonNumberCommand != value)
                {
                    _buttonNumberCommand = value;
                    OnPropertyChanged("ButtonCommand");
                }
            }
        }

        private ICommand _operationButtonCommand;
        public ICommand OperationButtonCommand
        {
            get
            {
                return _operationButtonCommand;
            }
            set
            {
                if (OperationButtonCommand != value)
                {
                    _operationButtonCommand = value;
                    OnPropertyChanged("OperationButtonCommand");
                }
            }
        }

        private ICommand _commaButtonCommand;
        public ICommand CommaButtonCommand
        {
            get
            {
                return _commaButtonCommand;
            }
            set
            {
                if (CommaButtonCommand != value)
                {
                    _commaButtonCommand = value;
                    OnPropertyChanged("CommaButtonCommand");
                }
            }
        }

        private ICommand _resultButtonCommand;
        public ICommand ResultButtonCommand
        {
            get
            {
                return _resultButtonCommand;
            }
            set
            {
                if (ResultButtonCommand != value)
                {
                    _resultButtonCommand = value;
                    OnPropertyChanged("ResultButtonCommand");
                }
            }
        }

        private ICommand _reportButtonCommand;
        public ICommand ReportButtonCommand
        {
            get
            {
                return _reportButtonCommand;
            }
            set
            {
                if (ReportButtonCommand != value)
                {
                    _reportButtonCommand = value;
                    OnPropertyChanged("ReportButtonCommand");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private Operation LastOperationSelected = Operation.none;

        Guid currentGuid = Guid.NewGuid();

     //   List<string> savedListFromServer = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            this._reportButtonCommand = new ICommandReportButton(this);
            this._resultButtonCommand = new ICommandResultButton(this);
            this._commaButtonCommand = new ICommandCommaButton(this);
            this._operationButtonCommand = new ICommandOperationButton(this);
            this._buttonNumberCommand = new ICommandButtonNumber(this);
            this._eraseCommand = new ICommandErase(this);
            this.DataContext = this;
        }

        ServiceReference1.ServiceClient Operations = new ServiceReference1.ServiceClient();

        public void Erase()
        {
            DisplayText = string.Empty;
            MemoryText = string.Empty;
            OperationText = string.Empty;
            LastOperationSelected = Operation.none;
        }

        public void GetButtonNumber(object buttonContent)
        {
            if (Operation.result == LastOperationSelected)
            {
                DisplayText = string.Empty;
                LastOperationSelected = Operation.none;
            }
            DisplayText += buttonContent;
        }

        public void OperationButtonClick(object buttonContent)
        {           
            if ((Operation.none != LastOperationSelected) || (Operation.result != LastOperationSelected))
            {
                ResultButtonClick();
            }            
            switch (buttonContent.ToString())
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
            OperationText = buttonContent.ToString();
            DisplayText = string.Empty;
        }

        public void CommaButtonClick()
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

        public void ResultButtonClick()
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
                      double.Parse(DisplayText), currentGuid).ToString();
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
                            double.Parse(DisplayText), currentGuid).ToString();
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
                            double.Parse(DisplayText), currentGuid).ToString();
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
                            double.Parse(DisplayText), currentGuid).ToString();
                    }
                    catch
                    {
                        DisplayText = String.Empty;
                    }
                    break;
                case Operation.signchange:
                    try
                    {
                        DisplayText = Operations.SignChange(double.Parse(MemoryText), currentGuid).ToString();
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

        public void ReportButtonClick()
        {
            List<string> lista = new List<string>();
            lista = Operations.HistoryReport(currentGuid).ToList();
            var newWindow = new Window1(lista);
            newWindow.ShowDialog();
        }
    }

    public class ICommandErase : ICommand
    {
        MainWindow _oknoGlowne = null;


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._oknoGlowne.Erase();
        }

        public ICommandErase(MainWindow oknoGlowne)
        {
            this._oknoGlowne = oknoGlowne;
        }
    }

    public class ICommandButtonNumber : ICommand
    {
        MainWindow _oknoGlowne = null;


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._oknoGlowne.GetButtonNumber(parameter);
        }

        public ICommandButtonNumber(MainWindow oknoGlowne)
        {
            this._oknoGlowne = oknoGlowne;
        }
    }

    public class ICommandOperationButton : ICommand
    {
        MainWindow _oknoGlowne = null;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._oknoGlowne.OperationButtonClick(parameter);
        }

        public ICommandOperationButton (MainWindow oknoGlowne)
        {
            this._oknoGlowne = oknoGlowne;
        }
    }

    public class ICommandCommaButton : ICommand
    {
        MainWindow _oknoGlowne = null;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._oknoGlowne.CommaButtonClick();
        }

        public ICommandCommaButton(MainWindow oknoGlowne)
        {
            this._oknoGlowne = oknoGlowne;
        }
    }

    public class ICommandResultButton : ICommand
    {
        MainWindow _oknoGlowne = null;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._oknoGlowne.ResultButtonClick();           
        }

        public ICommandResultButton(MainWindow oknoGlowne)
        {
            this._oknoGlowne = oknoGlowne;
        }
    }

    public class ICommandReportButton : ICommand
    {
        MainWindow _oknoGlowne = null;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._oknoGlowne.ReportButtonClick();
        }

        public ICommandReportButton(MainWindow oknoGlowne)
        {
            this._oknoGlowne = oknoGlowne;
        }
    }
}