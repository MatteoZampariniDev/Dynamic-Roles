using System;

namespace DynamicRoles.Core
{
    /// <summary>
    /// Attribute for permission enum or value description
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class DetailsAttribute : Attribute
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DetailsAttribute(string name = "", string description = "")
        {
            this.Name = name;
            this.Description = description;
        }
    }
}