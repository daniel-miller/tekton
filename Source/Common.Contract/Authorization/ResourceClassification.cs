namespace Common.Contract
{
    public enum ResourceClassification 
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