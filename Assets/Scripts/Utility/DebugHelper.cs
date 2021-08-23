namespace DefaultNamespace
{
    public static class DebugHelper
    {
        public static string Context(this object obj, string functionName)
        {
            return $"{obj.GetType().Name}::{functionName}() --";
        }
    }
}