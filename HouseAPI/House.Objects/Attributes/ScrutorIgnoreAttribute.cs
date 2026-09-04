using System;

namespace House.Objects.Attributes
{
    //Used for objects that are not referenced in API output but need to be in the schema.
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ScrutorIgnoreAttribute : Attribute
    {
    }
}
