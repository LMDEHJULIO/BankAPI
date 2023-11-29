namespace BankAPI.Exceptions
{
    public class InvalidCustomerDataException : Exception
    {
        public InvalidCustomerDataException(string message) : base(message) { }
    }

    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(int customerId) : base($"Customer with ID {customerId} not found."){ }
    }

    public class InvalidCreationDataException : Exception 
    {
        public InvalidCreationDataException(string message) : base(message) { }
    }

    public class ResourceNotFoundException: Exception
    {
        public ResourceNotFoundException(string message) : base(message) { }
    }
}
