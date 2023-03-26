
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Cunt.ECS;

public class App
{
    internal List<System> startupSystems = new();
    internal List<System> tickSystems = new();

    internal List<Entity> entities = new();

    public void Run()
    {
        startupSystems.ForEach(s => s.CallSystem(this));

        if (tickSystems.Count == 0)
            return;

        while (true)
        {
            tickSystems.ForEach(s => s.CallSystem(GetCommands()));
        }
    }

    /// ToDo:
    /// - AddSystem
    /// AddComponent

    public App AddSystem(Action a)
    {
        System sys;
        if (a == null)
            throw new ArgumentNullException("System provided is null");

        if (a.Method.GetCustomAttribute<StartSystemAttribute>() != null)
            sys = System.Create(a, true);
        else if (a.Method.GetCustomAttribute<TickSystemAttribute>() != null)
            sys = System.Create(a, false);
        else throw new Exception("The provided system needs a start or tick system attribute");

        InsertSystem(sys);

        return this;
    }
    public App AddSystem(Action<Commands> a)
    {
        System sys;
        if (a == null)
            throw new ArgumentNullException("System provided is null");

        if (a.Method.GetCustomAttribute<StartSystemAttribute>() != null)
            sys = System.Create(a, true);
        else if (a.Method.GetCustomAttribute<TickSystemAttribute>() != null)
            sys = System.Create(a, false);
        else throw new Exception("The provided system needs a start or tick system attribute");

        InsertSystem(sys);

        return this;
    }

    private void InsertSystem(System sys) => (sys.IsStartSystem ? startupSystems : tickSystems).Add(SetCommandStatus(sys));

    private System SetCommandStatus(System sys)
    {
        if (sys == null) throw new ArgumentNullException("System provided is null");

        var args = sys.method.GetParameters();

        if (args.Length == 0)
        {
            sys.TakesCommands = false;
            return sys;
        }

        if (args.Length != 1)
            throw new Exception("A system has to either have no args, or a Commands argument.");

        if (args[0].ParameterType != typeof(Commands))
            throw new Exception("A system has to either have no args, or a Commands argument.");

        sys.TakesCommands = true;
        return sys;
    }

    public App AddSystems(IEnumerable<System> systems)
    {
        systems.LoopAll(InsertSystem);
        return this;
    }



    public App AddComponent<T>() where T : Component
    {
        //Do stuff
        return this;
    }


    public App AddPlugin<T>() where T : Plugin, new()
    {
        new T().Build(this);
        return this;
    }

    public App AddPlugin(Plugin p)
    {
        p.Build(this);
        return this;
    }
    public App AddPlugins(params Plugin[] plugins)
    {
        foreach (Plugin p in plugins)
            AddPlugin(p);

        return this;
    }

    /// Add variation with filtering
    
    //internal List<T> QueryComponents<T>() 
    //{
    //    List<Component> result = new();

    //    //For loop because more optimized
    //    for (int i = 0; i < entities.Count; i++)
    //    {

    //    }

    //    return null!;
    //} 

    internal Commands GetCommands()
    {
        return new Commands(this);
    }

    internal void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }
}