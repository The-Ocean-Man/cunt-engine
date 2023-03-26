

namespace Cunt.ECS;

public interface IQueryFilter<TSelf> where TSelf : new()
{
    public bool MatchesFilter(Entity e);
}