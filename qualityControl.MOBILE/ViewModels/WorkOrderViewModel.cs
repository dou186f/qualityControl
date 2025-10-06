using qualityControl.MOBILE.Services;
using qualityControl.SHARED.Dtos;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static qualityControl.MOBILE.AppShell;

namespace qualityControl.MOBILE.ViewModels
{
    public class WorkOrderViewModel : INotifyPropertyChanged
    {
        private readonly WorkOrderService _workOrderService;

        public ObservableCollection<WorkOrderDto> WorkOrders { get; } = new();

        private string? _query;
        public string? Query
        {
            get { return _query; }
            set { _query = value; OnPropertyChanged(); }
        }

        private bool _onlyfinished;
        public bool OnlyFinished
        {
            get => _onlyfinished;
            set
            {
                if (_onlyfinished == value) return;
                _onlyfinished = value;
                OnPropertyChanged();
            }
        }

        private bool _onlynotfinished;
        public bool OnlyNotFinished
        {
            get => _onlynotfinished;
            set
            {
                if (_onlynotfinished == value) return;
                _onlynotfinished = value;
                OnPropertyChanged();
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); }
        }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public ICommand SearchCommand { get; }
        public ICommand OpenWorkOrderDetailCommand { get; }

        public WorkOrderViewModel(WorkOrderService workorderservice)
        {
            _workOrderService = workorderservice ?? throw new ArgumentNullException(nameof(workorderservice));

            SearchCommand = new Command(async () => await RunSearch());
            OpenWorkOrderDetailCommand = new Command<WorkOrderDto>(async workorder => await OpenWorkOrderDetailAsync(workorder));
        }

        public async Task RunSearch()
        {
            if (IsBusy) return;
            IsBusy = true;
            ErrorMessage = null;

            try
            {
                WorkOrders.Clear();
                List<WorkOrderDto> list;
                try
                {
                    var data = await _workOrderService.SearchWorkOrderAsync(Query, OnlyFinished, OnlyNotFinished, default);
                    list = data?.ToList() ?? new List<WorkOrderDto>();
                }
                catch (HttpRequestException)
                {
                    ErrorMessage = "The server could not be reached. Please check your internet connection or API address.";
                    return;
                }
                catch (TaskCanceledException)
                {
                    ErrorMessage = "The request timed out. Try again.";
                    return;
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                    return;
                }
                foreach (var item in list) WorkOrders.Add(item);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OpenWorkOrderDetailAsync(WorkOrderDto workorder)
        {
            if (workorder == null) return;
            var nav = new Dictionary<string, object> { ["LogicalRef"] = workorder.LogicalRef };
            await Shell.Current.GoToAsync(WorkOrderDetailRoute, nav);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? n = null)
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
        }
    }
}
