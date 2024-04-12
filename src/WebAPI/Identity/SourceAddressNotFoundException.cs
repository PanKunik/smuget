using Application.Exceptions;

namespace WebAPI.Identity
{
    public class SourceAddressNotFoundException
        : IdentityException
    {
        public SourceAddressNotFoundException()
            : base("Remote IP address not found.") { }
    }
}
