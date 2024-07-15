namespace RPSSL.GameService.Domain.Abstractions;

public interface IRandomGeneratedNumberService
{
	Task<short> GetRandomNumber();
}