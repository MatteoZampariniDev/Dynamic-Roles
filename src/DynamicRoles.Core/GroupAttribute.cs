using System;

namespace DynamicRoles.Core
{

    /// <summary>
    /// Attribute for creating permission groups with hierarchy
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class GroupAttribute : Attribute
    {
        public string Name { get; } = string.Empty;
        public int Level { get; } = 0;

        public GroupAttribute(string groupName, int level = 0)
        {
            this.Name = groupName;
            this.Level = level;
        }
    }
}