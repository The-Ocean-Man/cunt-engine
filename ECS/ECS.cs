using System.ComponentModel;
using System.Reflection;


/// ToDo:
/// - Entities
/// Components
/// Systems
/// Querying
/// Running the app
/// Plugins - like bevy plugins 
/// 
namespace Cunt.ECS;

public class Entity
{
	internal Guid id;
	internal Dictionary<Type, Component> components;

	internal Entity(Guid id)
	{
		this.id = id;
		components = new();
	}

	//internal void IterComponents(Action<Component> action) => components.LoopAll(pair => action(pair.Value));
	//internal IEnumerable<Component> QueryComponents(Func<Component, bool> pred) => components.Where(pair => pred(pair.);

	/// <summary>
	/// Creates a new T component with "new()" ctor, and calls Init() function.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public Entity Insert<T>() where T : Component, new()
	{
		var c = new T();

		c.Init();

		c.owner = this;

		components[typeof(T)] = c;

		return this;
	}

	/// <summary>
	/// Adds a provided component to this entity.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="comp"></param>
	/// <returns></returns>
	public Entity Insert<T>(T comp) where T : Component
	{
		//components.Add(comp);
		components[typeof(T)] = comp;

		return this;
	}

	public static bool operator ==(Entity a, Entity b)
	{
		return a.id == b.id;
	}
	public static bool operator !=(Entity a, Entity b)
	{
		return a.id == b.id;
	}
    public override bool Equals(object? obj)
    {
		if (obj is Entity e)
			return id == e.id;

		return false;
    }
    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
    public override string ToString()
    {
		return $"Entity: {id}";
    }
}

public class System
{
	internal MethodInfo method;
	internal bool IsStartSystem;
	internal bool TakesCommands;

	internal void CallSystem(object? args)
	{
		method.Invoke(null, args != null ? new object[] { args } : new object[] { });
	}

	internal void CallSystem(App app)
	{
		object[] args;

		if(TakesCommands)
			args = new object[] { app.GetCommands() };
		else 
			args = new object[0];

		method.Invoke(null, args);
	}

	internal static System Create(Action a, bool start)
	{
		var s = new System();
		s.IsStartSystem = start;
		s.method = a.Method;
		return s;
	}

	internal static System Create(Action<Commands> a, bool start)
	{
		var s = new System();
		s.IsStartSystem = start;
		s.method = a.Method;
		return s;
	}

    internal static System Create(MethodInfo m, bool start)
    {
        var s = new System();
        s.IsStartSystem = start;
        s.method = m;
        return s;
    }
}

public abstract class Component
{
	internal Entity owner;

	virtual internal void VerifyComponent()
	{

	}

	public Entity GetOwner() => owner;

	public virtual void Init() { }
}

public class MultiComponent : Component
{
    internal override void VerifyComponent()
    {
        base.VerifyComponent();
    }
}


public abstract class Plugin
{
	public abstract void Build(App app);
}