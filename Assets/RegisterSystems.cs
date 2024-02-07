using System.Collections.Generic;

public class RegisterSystems
{
    public static List<ISystem> GetListOfSystems()
    {
        // determine order of systems to add
        var toRegister = new List<ISystem>();
        
        // Add your systems here
        toRegister.Add(new Initialize());        
        toRegister.Add(new Movement());
        toRegister.Add(new Collision());
        toRegister.Add(new Explosion());
        toRegister.Add(new Protection());
        toRegister.Add(new Draw());

        return toRegister;
    }
}