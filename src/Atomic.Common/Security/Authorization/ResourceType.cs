namespace Atomic.Common
{
    public enum ResourceType 
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