using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Helpers
{
    public class EnumDisplay<T> : INotifyPropertyChanged where T : Enum
    {
        public T Value { get; }

        public string DisplayName { get; }

        public EnumDisplay(T value)
        {
            Value = value;
            DisplayName = GetDisplayName(value);
        }

        private static string GetDisplayName(T value)
        {
            var member = typeof(T).GetMember(value.ToString()).FirstOrDefault();
            var attr = member?.GetCustomAttribute<DisplayAttribute>();
            return attr?.Name ?? value.ToString();
        }

        public override string ToString() => DisplayName;

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SelectableEnumDisplay<T> : EnumDisplay<T> where T : Enum
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));  // kế thừa từ EnumDisplay<T> nếu nó đã implement INotifyPropertyChanged
            }
        }

        public SelectableEnumDisplay(T value) : base(value)
        {
            IsSelected = false;
        }
    }

    public static class EnumExtensions
    {
        /// <summary>
        /// Lấy DisplayAttribute.Name của enum, fallback về ToString() nếu không có.
        /// </summary>
        public static string GetDisplayName(this Enum enumValue)
        {
            var member = enumValue.GetType()
                                  .GetMember(enumValue.ToString())
                                  .FirstOrDefault();
            if (member == null)
                return enumValue.ToString();

            var displayAttr = member.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetName() ?? enumValue.ToString();
        }
    }
}
