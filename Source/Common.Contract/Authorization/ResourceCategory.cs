namespace Common.Contract.Security
{
    public enum ResourceCategory 
    { 
        // Undefined
        None, 
        
        // API
        Endpoint, 
        
        // Database
        Aggregate,
        Entity,
        Field,

        // User Interface
        Directory, 
        Form, 
        Element 
    }
}