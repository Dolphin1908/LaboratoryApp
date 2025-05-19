using LaboratoryApp.src.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.src.Modules.Toolkits.CalculatorFunction.ViewModels
{
    public class CalculatorViewModel : BaseViewModel
    {
        #region Commands
        public ICommand AddNumberCommand { get; set; } // Add number
        public ICommand AddOperatorCommand { get; set; } // Add operator
        public ICommand CalculateCommand { get; set; } // Calculate
        public ICommand SquareRootCommand { get; set; } // Square root
        public ICommand SquareCommand { get; set; } // Square
        public ICommand OneDivideXCommand { get; set; } // 1/x
        public ICommand ChangeSignCommand { get; set; } // Change sign
        public ICommand PercentCommand { get; set; } // Percent
        public ICommand BackspaceCommand { get; set; } // Backspace
        public ICommand ClearNumberCommand { get; set; } // Clear number
        public ICommand ClearFormulaCommand { get; set; } // Reset formula
        #endregion

        #region Properties
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
        #endregion

        private decimal? _currentValue = null;
        private char? _lastOperator = null;
        private bool _isOperatorAdded = false; // Check if operator is added
        private bool _isCalculated = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public CalculatorViewModel()
        {
            Reset();
            AddNumberCommand = new RelayCommand<string>((p)=> true, (p) => AddNumber(p));
            AddOperatorCommand = new RelayCommand<string>((p) => true, (p) => AddOperator(p));
            BackspaceCommand = new RelayCommand<string>((p) => true, (p) => Backspace());
            ClearFormulaCommand = new RelayCommand<string>((p) => true, (p) => Reset());
            ClearNumberCommand = new RelayCommand<string>((p) => true, (p) => ClearNumber());
            CalculateCommand = new RelayCommand<string>((p) => true, (p) => Calculate());
            SquareRootCommand = new RelayCommand<string>((p) => true, (p) => SquareRoot());
            SquareCommand = new RelayCommand<string>((p) => true, (p) => Square());
            OneDivideXCommand = new RelayCommand<string>((p) => true, (p) => OneDivideX());
            ChangeSignCommand = new RelayCommand<string>((p) => true, (p) => ChangeSign());
            PercentCommand = new RelayCommand<string>((p) => true, (p) => Percent());
        }

        /// <summary>
        /// Thêm số vào màn hình
        /// </summary>
        /// <param name="number"></param>
        private void AddNumber(string numStr)
        {
            if (_isCalculated)
            {
                // Nếu đã tính toán thì reset lại
                Reset();
                _isCalculated = false; // Đánh dấu chưa tính toán
            }    
            if (_isOperatorAdded || Number == "0")
            {
                Number = numStr;
                _isOperatorAdded = false; // Reset operator added flag
            }
            else
            {
                Number += numStr;
            }
        }

        /// <summary>
        /// Thêm toán tử vào màn hình
        /// </summary>
        /// <param name="op"></param>
        private void AddOperator(string opStr)
        {
            char op = opStr[0];
            char displayOp = op switch
            {
                '*' => 'x',
                '/' => '÷',
                _ => op
            };

            // Nếu nhập liên tiếp toán tử thì lấy cái cuối cùng
            if (_isOperatorAdded)
            {
                if(!string.IsNullOrEmpty(Formula))
                {
                    Formula = $"{Formula.TrimEnd()[..^1]}{displayOp} ";
                }
                _lastOperator = op;
                return;
            }

            decimal numberValue = GetNumberValue(Number);

            // Nếu chưa có giá trị tính toán, chuyển number thành giá trị hiện tại
            if (_currentValue == null)
            {
                _currentValue = numberValue;
                Formula = $"{Number} {displayOp} ";
            }
            else if (_lastOperator != null) 
            {
                try
                {
                    _currentValue = Evaluate(_currentValue.Value, numberValue, _lastOperator.Value);
                    Formula = $"{_currentValue.ToString()} {displayOp} ";
                }
                catch (OverflowException ex)
                {
                    Number = ex.Message;
                    Reset();
                    return;
                }
            }

            _isCalculated = false;
            _lastOperator = op;
            _isOperatorAdded = true; // Đánh dấu đã thêm toán tử
        }

        private void SquareRoot()
        {
            // Tính toán phép tính có sẵn trước khi tính căn bậc 2
            decimal left = _currentValue != null ? _currentValue.Value : 0m;
            char op = _lastOperator != null ? _lastOperator.Value : ' ';
            decimal numberValue = Evaluate(left, GetNumberValue(Number), op);

            // Tính căn bậc 2
            try
            {
                decimal result = (decimal)Math.Sqrt((double)numberValue);
                Formula = $"√{numberValue} = ";
                Number = result.ToString("G7");
            }
            catch (OverflowException ex)
            {
                Number = ex.Message;
            }
        }

        private void Square()
        {
            // Tính toán phép tính có sẵn trước khi tính bình phương
            decimal left = _currentValue != null ? _currentValue.Value : 0m;
            char op = _lastOperator != null ? _lastOperator.Value : ' ';
            decimal numberValue = Evaluate(left, GetNumberValue(Number), op);

            // Tính bình phương
            try
            {
                decimal result = (decimal)Math.Pow((double)numberValue, 2);
                Formula = $"Sqr({numberValue}) = ";
                Number = result.ToString("G7");
            }
            catch (OverflowException ex)
            {
                Number = ex.Message;
            }
        }

        private void OneDivideX()
        {
            // Tính toán phép tính có sẵn trước khi tính 1/x
            decimal left = _currentValue != null ? _currentValue.Value : 0m;
            char op = _lastOperator != null ? _lastOperator.Value : ' ';
            decimal numberValue = Evaluate(left, GetNumberValue(Number), op);

            // Tính 1/x
            try
            {
                decimal result = 1 / numberValue;
                Formula = $"1/{numberValue} = ";
                Number = result.ToString("G7");
            }
            catch (DivideByZeroException ex)
            {
                Number = ex.Message;
            }
        }

        private void ChangeSign()
        {
            if (_isCalculated)
            {
                Formula = $"negate({Number})";
            }

            // Đổi dấu số
            if (Number != "0")
            {
                if (Number.StartsWith("-"))
                {
                    Number = Number[1..];
                }
                else
                {
                    Number = "-" + Number;
                }
            }
        }

        private void Percent()
        {
            // Tính toán phép tính có sẵn trước khi tính phần trăm
            decimal left = _currentValue != null ? _currentValue.Value : 0m;
            char op = _lastOperator != null ? _lastOperator.Value : ' ';
            decimal numberValue = Evaluate(left, GetNumberValue(Number), op);

            // Tính phần trăm
            try
            {
                decimal result = numberValue / 100;
                Formula = $"{numberValue}% = ";
                Number = result.ToString("G7");
            }
            catch (OverflowException ex)
            {
                Number = ex.Message;
            }
        }

        /// <summary>
        /// Thực hiện 1 phép toán
        /// </summary>
        /// <param name="left">Giá trị 1</param>
        /// <param name="right">Giá trị 2</param>
        /// <param name="op">Toán tử</param>
        /// <returns></returns>
        /// <exception cref="DivideByZeroException"></exception>
        /// <exception cref="OverflowException"></exception>
        private decimal Evaluate(decimal left, decimal right, char op)
        {
            _isCalculated = true; // Đánh dấu đã tính toán

            try
            {
                return op switch
                {
                    '+' => checked(left + right),
                    '-' => checked(left - right),
                    '*' => checked(left * right),
                    '/' => right != 0 ? left / right : throw new DivideByZeroException("Không thể chia cho 0"),
                    _ => right, // Nếu không có toán tử thì trả về giá trị bên phải
                };
            }
            catch (OverflowException)
            {
                throw new OverflowException("Kết quả vượt giới hạn");
            }
        }

        /// <summary>
        /// Remove last character from number
        /// </summary>
        private void Backspace()
        {
            if(_isCalculated)
            {
                Reset();
            }
            else
            {
                if (!string.IsNullOrEmpty(Number) && Number.Length > 1)
                {
                    Number = Number.Substring(0, Number.Length - 1);
                }
                else
                {
                    Number = "0";
                }
            }
        }

        /// <summary>
        /// Clear number
        /// </summary>
        private void ClearNumber()
        {
            Number = "0";
        }

        private void Calculate()
        {
            if(_lastOperator == null || _currentValue == null)
            {
                Formula = $"{Number} = ";
                return;
            }

            decimal numberValue = GetNumberValue(Number);

            try
            {
                decimal result = Evaluate(_currentValue.Value, numberValue, _lastOperator.Value);
                Formula += $"{Number} = ";
                Number = result.ToString("G7");
            }
            catch (DivideByZeroException ex)
            {
                Number = ex.Message;
            }
            catch (OverflowException ex)
            {
                Number = ex.Message;
            }

            // Reset sau tính toán
            _currentValue = null;
            _lastOperator = null;
            _isOperatorAdded = false;
        }

        private decimal GetNumberValue(string numStr)
        {
            if (!decimal.TryParse(numStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal numberValue))
            {
                Number = "0";
                numberValue = 0;
            }

            return numberValue;
        }

        /// <summary>
        /// Reset all
        /// </summary>
        public void Reset()
        {
            Formula = string.Empty;
            Number = "0";
            _currentValue = null;
            _lastOperator = null;
            _isOperatorAdded = false;
        }
    }
}
