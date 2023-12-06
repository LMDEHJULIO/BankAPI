namespace BankAPI.Services
{
    public abstract class BaseService
    {
        private readonly ILogger<BaseService> _logger;

        protected BaseService(ILogger<BaseService> logger)
        {
            _logger = logger;
        }

        protected T Execute<T>(Func<T> action, string operationName)
        {
            try
            {
                _logger.LogInformation($"Executing {operationName} operation.");
                return action();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred during {operationName} operation.");
                throw; // Re-throw the original exception for further handling.
            }
        }
    }
}
