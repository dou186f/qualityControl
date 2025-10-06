using qualityControl.MOBILE.Services;
using qualityControl.SHARED.Dtos;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static qualityControl.MOBILE.AppShell;

namespace qualityControl.MOBILE.ViewModels
{
    [QueryProperty(nameof(LogicalRef), "LogicalRef")]
    public class DetailViewModel : INotifyPropertyChanged
    {
        private readonly WorkOrderService _workOrderService;

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private WorkOrderDto _workOrderDto = new();
        public WorkOrderDto workOrderDto 
        { 
            get => _workOrderDto; 
            set 
            { 
                _workOrderDto = value; 
                OnPropertyChanged(); 
            } 
        }

        private int _logicalRef;
        public int LogicalRef
        {
            get => _logicalRef;
            set
            {
                _logicalRef = value;
                _ = LoadAsync(_logicalRef);
            }
        }

        public ICommand OpenAddQualityControlCommand { get; }

        public DetailViewModel(WorkOrderService workOrderService)
        {
            _workOrderService = workOrderService;
            OpenAddQualityControlCommand = new Command<WorkOrderDto>(async workorder => await OpenAddQualityControlCommandAsync(workorder));
        }

        private async Task LoadAsync(int logref)
        {
            if (logref <= 0 || IsBusy) return;
            IsBusy = true;
            try
            {
                var dto = await _workOrderService.GetWorkOrderAsync(logref);
                workOrderDto = dto ?? new WorkOrderDto { LogicalRef = logref};
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OpenAddQualityControlCommandAsync(WorkOrderDto workorder)
        {
            if (workorder == null) return;
            var nav = new Dictionary<string, object> { ["LogicalRef"] = workorder.LogicalRef };
            await Shell.Current.GoToAsync(QualityControlAdderRoute, nav);
        }
    }
}
