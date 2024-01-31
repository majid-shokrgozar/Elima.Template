namespace Elima.Common.Domain.Entities;

public interface IHasConcurrencyStamp
{
    string ConcurrencyStamp { get; set; }
}
