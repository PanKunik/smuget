using Application.Abstractions.Time;

namespace Infrastructure.Time;
internal sealed class Clock : IClock
{
	public DateTime Current()
	{
		return DateTime.Now;
	}

	public DateTime CurrentUtc()
	{
		return DateTime.UtcNow;
	}
}
