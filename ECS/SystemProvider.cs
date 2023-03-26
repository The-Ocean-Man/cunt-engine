
namespace Cunt.ECS;

public abstract class SystemProvider<TSelf>
{
	public static System[] GetSystems() => typeof(TSelf).GetSystems();
}