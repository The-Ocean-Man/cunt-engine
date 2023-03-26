
namespace Cunt.ECS;

//Creates a system that is called when the app is first ran
public sealed class StartSystemAttribute : Attribute
{

}

//Systems called in a loop after start systems
public sealed class TickSystemAttribute : Attribute
{

}

[Obsolete("This is supposed to query for the type of the first argument, and then call the system a bunch of times")]
public sealed class QueryIterAttribute : Attribute
{

}
