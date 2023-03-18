namespace AutoresApi.Services
{
    public class WriteInFile : IHostedService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string nameFile = "theFile.txt";
        private Timer timer;

        public WriteInFile(IWebHostEnvironment env)
        {
            this._env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Write("Hello World File");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //Stop Time wirh Dispose
            timer.Dispose();
            Write("Wrote Hello World in the File");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Write("Ongoing process" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        private void Write(string message)
        {
            var route = $@"{_env.ContentRootPath}\wwwroot\{nameFile}";
            using (StreamWriter writer = new StreamWriter(route, append: true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
