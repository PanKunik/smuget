namespace Domain.Exceptions
{
    internal class InvalidIpAddressException
        : SmugetException
    {
        public InvalidIpAddressException()
            : base("IP address cannot be null or whitespace.") { }
    }
}
