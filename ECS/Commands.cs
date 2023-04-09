using System.Runtime.CompilerServices;

namespace Cunt.ECS;

public sealed class Commands
{
	private App app;

	internal Commands(App app)
	{
		this.app = app;
	}

	public List<T> Query<T>() where T : Component
	{
		List<T> list = new();

		var ty = typeof(T);

		foreach (var e in app.entities)
		{
			foreach (var c in e.components.Values)
			{
				if (c.GetType() == ty)
				{
					list.Add((T)c);
					break;
				} 
					
			}
		}

		return list;
	}

	public List<T> MultiQuery<T>() where T : ITuple, new()
	{
		List<T> list = new();

		var types = GetTupleTypes<T>();

		foreach (var e in app.entities)
		{
			int matchCount = 0;

			List<object> tupleItems = new();

			foreach (var c in e.components.Values)
			{
				if (types.Contains(c.GetType()))
				{
					matchCount++;
					tupleItems.Add(c);
				}
			}

			if (matchCount == types.Length)
				list.Add((T)(CreateTuple(tupleItems.ToArray())));
		}

		return list;
	}

	private static Type[] GetToupleTypes<T>() where T: ITuple, new()
	{
		var t = new T();
		Console.WriteLine($"t == null: {0}");

		var arr = new Type[t.Length];

		for(int i = 0; i < t.Length; i++)
		{
			arr[i] = t[i].GetType()!;
		}

		Console.WriteLine();

		foreach(var a in arr)
		{
			Console.WriteLine(a == null);
		}

		Console.WriteLine();

		return arr;
	}

	private static object CreateTuple(object[] values) 
	{
		Type genericType = Type.GetType("System.ValueTuple`" + values.Length);
		Type[] typeArgs = values.Select(obj => obj.GetType()).ToArray();
		Type specificType = genericType.MakeGenericType(typeArgs);
		object[] constructorArguments = values.Cast<object>().ToArray();
		return Activator.CreateInstance(specificType, constructorArguments);
	}

	private static Type[] GetTupleTypes<T>() where T : ITuple
	{
		return typeof(T).GetFields().Select(f => f.FieldType).ToArray();
	}

	public void QueryLoop<T>(Action<T> action) where T : Component
	{
		Query<T>().ForEach(action);
	}

	public Entity Spawn()
	{
		return Spawn(_ => { });
	}

	public Entity Spawn(Action<Entity> spawnFunc)
	{
		var e = new Entity(Guid.NewGuid());

		spawnFunc(e);

		app.AddEntity(e);

		return e;
	}
}
