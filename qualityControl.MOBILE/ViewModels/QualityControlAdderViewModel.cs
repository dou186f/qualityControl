using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using qualityControl.MOBILE.Services;
using qualityControl.SHARED.Dtos;

namespace qualityControl.MOBILE.ViewModels
{
    [QueryProperty(nameof(WorkOrderRef), "LogicalRef")]
    public class QualityControlAdderViewModel : INotifyPropertyChanged
    {
        private readonly QualityControlService _qcService;

        public ObservableCollection<QualityControlViewModel> Items { get; } = new();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
                (SaveCommand as Command)?.ChangeCanExecute(); 
                (RefreshCommand as Command)?.ChangeCanExecute();
            }
        }

        public bool IsNotBusy => !IsBusy; 

        private int _workOrderRef;
        public int WorkOrderRef
        {
            get => _workOrderRef;
            set { _workOrderRef = value; _ = LoadAsync(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand RefreshCommand { get; }

        public QualityControlAdderViewModel(QualityControlService qcService)
        {
            _qcService = qcService;
            SaveCommand = new Command(async () => await SaveAsync(), () => !IsBusy);
            RefreshCommand = new Command(async () => await LoadAsync(), () => !IsBusy);
        }

        private async Task LoadAsync()
        {
            if (IsBusy || WorkOrderRef <= 0) return;
            IsBusy = true;
            try
            {
                Items.Clear();
                var list = await _qcService.GetChecklistAsync(WorkOrderRef);
                foreach (var d in list)
                {
                    Items.Add(new QualityControlViewModel
                    {
                        QcRef = d.QcRef,
                        Name = d.Name ?? "",
                        SetRef = d.SetRef,
                        MinVal = d.MinVal,
                        MaxVal = d.MaxVal,
                        Result = d.Result,
                        ResultLogicalRef = d.ResultLogicalRef
                    });
                }
            }
            finally { IsBusy = false; }
        }

        private async Task SaveAsync()
        {
            if (IsBusy || WorkOrderRef <= 0) return;
            IsBusy = true;
            try
            {
                var payload = Items.Select(i => new QcChecklistItemDto
                {
                    QcRef = i.QcRef,
                    Name = i.Name,
                    SetRef = i.SetRef,
                    MinVal = i.MinVal,
                    MaxVal = i.MaxVal,
                    Result = i.Result,
                    ResultLogicalRef = i.ResultLogicalRef
                });

                await _qcService.SaveAllAsync(WorkOrderRef, payload);

                await Application.Current.MainPage.DisplayAlert(
                    "Saved", "All test results you selected have been written to Result DB.", "Okey");
            }
            finally { IsBusy = false; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
