using TZ.Models;

namespace TZ.Services
{
    internal class ServiceA : IRealizeServiceA, IHostedService
    {
        private Timer? _timer = null;
        private readonly unitsdbbContext _context;

        public ServiceA()
        {
            _context = new unitsdbbContext();
            StartAsync(new CancellationToken());
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateAllStatus, null, TimeSpan.Zero,TimeSpan.FromSeconds(3));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void UpdateAllStatus(object? state)
        {
            try
            {
                var units = _context.Units.ToArray();
                foreach (var unit in units)
                {
                    unit.Status = !unit.Status;
                }
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                
            }
        }

        string IRealizeServiceA.GetStatus(Unit unit)
        {
            return unit.Status ? "Активно" : "Заблокировано";
        }
    }
}
