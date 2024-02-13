namespace Elima.Common.Application;

public interface IEntityDto
{

}

public interface IEntityDto<TKey> : IEntityDto
{
    TKey Id { get; set; }
}
