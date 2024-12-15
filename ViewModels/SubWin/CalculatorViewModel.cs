using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.ViewModels.SubWin
{
    public class CalculatorViewModel : BaseViewModel
    {
        #region commands
        public ICommand AddNumberCommand { get; set; }
        public ICommand AddOperatorCommand { get; set; }
        public ICommand CalculateCommand { get; set; }
        public ICommand BackspaceCommand { get; set; }
        public ICommand ClearNumberCommand { get; set; }
        public ICommand ClearFormulaCommand { get; set; }
        #endregion

        private string _formula;
        public string Formula
        {
            get { return _formula; }
            set
            {
                if (_formula != value)
                {
                    _formula = value;
                    OnPropertyChanged(nameof(Formula));
                }
            }
        }

        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                if (_number != value)
                {
                    _number = value;
                    OnPropertyChanged(nameof(Number));
                }
            }
        }

        private StringBuilder _formulaString; 
        private StringBuilder _numberString;

        private List<int> _numbers;
        private List<char> _operators;

        public CalculatorViewModel()
        {
            Reset();
            AddNumberCommand = new RelayCommand<string>((p)=> true, (p) => AddNumber(p));
            AddOperatorCommand = new RelayCommand<string>((p) => true, (p) => AddOperator(p));
        }

        private void AddNumber(string number)
        {
            _numberString.Append(number);
            Number = _numberString.ToString();
        }

        private void AddOperator(string op)
        {
            if (string.IsNullOrEmpty(Formula))
            {
                _formulaString.Append(Number);
                _formulaString.Append(op);
                Formula = _formulaString.ToString();
                _numbers.Add(int.Parse(Number));
                _operators.Add(op[0]);
                _numberString.Clear();
                Number = "0";
            }
            else
            {
                _formulaString.Append(Number);
                _formulaString.Append(op);
                Formula = _formulaString.ToString();
                _numbers.Add(int.Parse(Number));
                _operators.Add(op[0]);
                _numberString.Clear();
                Number = "0";
            }
        }

        private void _init()
        {
            _formulaString = new StringBuilder();
            _numberString = new StringBuilder();
            _numbers = new List<int>();
            _operators = new List<char>();
        }

        public void Reset()
        {
            _init();
            Number = "0";
            Formula = "";
        }
    }
}
