namespace AutoresApi.Services
{
    public interface IService
    {

        Guid GetTransient();
        Guid GetScoped();
        Guid GetSingleton();
        void performTask();
    }

    public class ServiceA : IService
    {
        private readonly ILogger<ServiceA> _logger;
        private readonly ServiceTransient _transient;
        private readonly ServiceScoped _scoped;
        private readonly ServiceSingleton _singleton;

        public ServiceA(ILogger<ServiceA> logger, ServiceTransient transient, ServiceScoped scoped, ServiceSingleton singleton)
        {
            _logger = logger;
            this._transient = transient;
            this._scoped = scoped;
            this._singleton = singleton;
        }

        public Guid GetTransient() { return _transient.guid; }
        public Guid GetScoped() { return _scoped.guid; }
        public Guid GetSingleton() { return _singleton.guid; }


        public void performTask()
        {
            throw new NotImplementedException();
        }
        
    }
    public class ServiceB : IService
    {
        public Guid GetScoped()
        {
            throw new NotImplementedException();
        }

        public Guid GetSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid GetTransient()
        {
            throw new NotImplementedException();
        }

        public void performTask()
        {
            throw new NotImplementedException();
        }
    }

    //Examples types services
    public class ServiceTransient
    {
       public Guid guid = Guid.NewGuid();
    }
    public class ServiceScoped
    {
       public Guid guid = Guid.NewGuid();
    }
    public class ServiceSingleton
    {
       public Guid guid = Guid.NewGuid();
    }
}
