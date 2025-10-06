using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace qualityControl.MOBILE.ViewModels
{
    public class QualityControlViewModel : INotifyPropertyChanged
    {
        public int QcRef { get; init; }
        public string Name { get; init; } = "";
        public int? SetRef { get; init; }
        public double? MinVal { get; init; }
        public double? MaxVal { get; init; }
        public int? ResultLogicalRef { get; set; }

        private bool? _result;
        public bool? Result
        {   
            get => _result;
            set
            {
                if (_result == value) return;
                _result = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ResultText));
                OnPropertyChanged(nameof(IsOk));
                OnPropertyChanged(nameof(IsNotOk));
            }
        }

        public string ResultText =>
            Result == null ? "Not Selected"
          : Result == true ? "Suitable"
          : "Not suitable";

        public bool IsOk
        {
            get => Result == true;
            set { if (value) Result = true; }
        }

        public bool IsNotOk
        {
            get => Result == false;
            set { if (value) Result = false; }
        }

        public ICommand SetSuitableCommand { get; }
        public ICommand SetNotSuitableCommand { get; }

        public QualityControlViewModel()
        {
            SetSuitableCommand = new Command(() => Result = true);
            SetNotSuitableCommand = new Command(() => Result = false);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
