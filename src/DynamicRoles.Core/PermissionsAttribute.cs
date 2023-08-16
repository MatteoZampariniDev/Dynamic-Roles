using System;

namespace DynamicRoles.Core
{
    /// <summary>
    /// Attribute for marking enums as permissions
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public class PermissionsAttribute : FlagsAttribute
    {
        public string UniqueName { get; }

        public PermissionsAttribute(string uniqueName)
        {
            UniqueName = uniqueName;
        }
    }
}