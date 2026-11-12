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

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                _errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasError));
            }
        }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

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
            ErrorMessage = null;
            try
            {
                var list = await _qcService.GetChecklistAsync(WorkOrderRef);
                Items.Clear();
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
            catch (Exception ex) { ErrorMessage = GetErrorMessage(ex); }
            finally { IsBusy = false; }
        }

        private async Task SaveAsync()
        {
            if (IsBusy || WorkOrderRef <= 0) return;
            IsBusy = true;
            ErrorMessage = null;
            try
            {
                var payload = Items.Where(i => i.Result.HasValue).Select(i => new QcChecklistItemDto
                {
                    QcRef = i.QcRef,
                    Name = i.Name,
                    SetRef = i.SetRef,
                    MinVal = i.MinVal,
                    MaxVal = i.MaxVal,
                    Result = i.Result,
                    ResultLogicalRef = i.ResultLogicalRef
                }).ToList();
                if (payload.Count == 0)
                {
                    ErrorMessage = "Select at least one test result to save.";
                    return;
                }

                await _qcService.SaveAllAsync(WorkOrderRef, payload);

                await Application.Current.MainPage.DisplayAlert(
                    "Saved", "All test results you selected have been written to Result DB.", "Okey");
            }
            catch (Exception ex) { ErrorMessage = GetErrorMessage(ex); }
            finally { IsBusy = false; }
        }

        private static string GetErrorMessage(Exception exception) => exception switch
        {
            OperationCanceledException => "The request timed out. Please try again.",
            HttpRequestException { StatusCode: null } => "The server could not be reached. Check your network connection and API address.",
            HttpRequestException { StatusCode: System.Net.HttpStatusCode.NotFound } => "The work order, product or test could not be found. Refresh the list.",
            HttpRequestException { StatusCode: System.Net.HttpStatusCode.BadRequest } => "The results could not be validated. Check the work order and selected tests, then refresh the list.",
            HttpRequestException ex when (int?)ex.StatusCode >= 500 => "A server error occurred. The operation could not be completed. Please try again.",
            HttpRequestException => "The server did not accept the request. Please try again.",
            _ => "An unexpected error occurred. Please try again."
        };

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
