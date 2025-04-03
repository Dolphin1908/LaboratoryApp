using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryApp.ViewModels.SubWin
{
    public class CalculatorViewModel : BaseViewModel
    {
        #region Commands
        public ICommand AddNumberCommand { get; set; } // Add number
        public ICommand AddOperatorCommand { get; set; } // Add operator
        public ICommand CalculateCommand { get; set; } // Calculate
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

            // Nếu nhập liên tiếp toán tử thì lấy cái cuối cùng
            if (_isOperatorAdded)
            {
                if(!string.IsNullOrEmpty(Formula))
                {
                    Formula = Formula.Substring(0, Formula.Length - 2) + op + " ";
                }
                _lastOperator = op;
                return;
            }

            if (!decimal.TryParse(Number, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal numberValue))
            {
                Number = "0";
                numberValue = 0;
            }

            // Nếu chưa có giá trị tính toán, chuyển number thành giá trị hiện tại
            if (_currentValue == null)
            {
                _currentValue = numberValue;
                Formula = Number + " " + op + " ";
            }
            else if (_lastOperator != null) 
            {
                try
                {
                    _currentValue = Evaluate(_currentValue.Value, numberValue, _lastOperator.Value);
                    Formula = _currentValue.ToString() + " " + op + " ";
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
                Formula = Number + " = ";
                return;
            }

            if (!decimal.TryParse(Number, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal numberValue))
            {
                Number = "0";
                numberValue = 0;
            }

            try
            {
                decimal result = Evaluate(_currentValue.Value, numberValue, _lastOperator.Value);
                Formula += Number + " = ";
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
