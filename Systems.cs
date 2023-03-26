
using Cunt.ECS;

public class TestPlugin : Plugin
{
    public override void Build(App app)
    {
        app
            .AddSystem(Systems.SpawnEntities)
            .AddSystem(Systems.PrintEntities)
            .AddSystem(Systems.MultiQuery)

            .AddComponent<TestComponent>()
            .AddComponent<MoreComponent>()
            
            ;
    }
}

public class Systems : SystemProvider<Systems>
{
    [StartSystem] 
    public static void SpawnEntities(Commands cmd)
    {
        cmd.Spawn().Insert(new TestComponent(123, "Hello"));
        cmd.Spawn().Insert(new TestComponent(321, "Goodbye"));
        cmd.Spawn().Insert(new MoreComponent(123.321, true));

        cmd.Spawn().Insert(new TestComponent(456, "Double component"))
            .Insert(new MoreComponent(666.666, false));
    }

    [TickSystem]
    public static void MultiQuery(Commands cmd)
    {
        var q = cmd.MultiQuery<(TestComponent, MoreComponent)>();

        Console.WriteLine($"Doing multiquery {q.Count}");

        q.ForEach((tuple) => Console.WriteLine($"MultiQuery: Comp = {{{tuple.Item1}}}, More = {{{tuple.Item2}}}"));
    }

    [TickSystem]
    public static void PrintEntities(Commands cmd)
    {
        var q = cmd.Query<TestComponent>();

        q.ForEach(c => Console.WriteLine($"BasicQuery: i = {c.i}, s = {c.s}"));
    }




    [StartSystem]
    public static void StartSys()
    {
        Console.WriteLine("Start system");
    }

    [TickSystem]
    public static void TickSys(Commands cmd)
    {
        Console.WriteLine("Tick system");
    }
}

public class TestComponent : Component
{
    public int i;
    public string s;

    public TestComponent(int i, string s)
    {
        this.i = i;
        this.s = s;
    }

    public override string ToString()
    {
        return $"i = {i}, s = {s}";
    }
}

public class MoreComponent : Component
{
    public double d;
    public bool b;

    public MoreComponent(double d, bool b)
    {
        this.d = d;
        this.b = b;
    }

    public override string ToString()
    {
        return $"d = {d}, b = {b}";
    }
}