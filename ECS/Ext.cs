

using System.Reflection;

namespace Cunt.ECS;

public static class CuntExtensions
{
    static Type[] SystemAttrs = { typeof(StartSystemAttribute), typeof(TickSystemAttribute) };

    public static System[] GetSystems(this Type type)
    {
        if (type == null) throw new ArgumentNullException("Provided type is null");

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

        decimal[] list = { 1, 2, 3 };

        List<System> result = new List<System>();

        foreach (var m in methods)
        {
            if (!m.GetCustomAttributes().Any(a => SystemAttrs.Any(ty => a.GetType() == ty)))
                continue;

            System sys;

            if (m.GetCustomAttribute<StartSystemAttribute>() != null)
                sys = System.Create(m, true);
            else if (m.GetCustomAttribute<TickSystemAttribute>() != null)
                sys = System.Create(m, false);
            else continue;
        }

        return result.ToArray();
    }


    public static void LoopAll<T>(this IEnumerable<T> e, Action<T> a)
    {
        foreach (var t in e)
            a(t);
    }


    //internal static Type[] GetQueryTypes<T>(T item)
    //{
    //    var t = typeof(T);
    //    if(t.)
    //}
}