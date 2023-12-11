namespace Application.Abstractions.Time;
public interface IClock
{
	DateTime Current();
	DateTime CurrentUtc();
}
