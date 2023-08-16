using System;

namespace DynamicRoles.Core
{
    public class PermissionsException : Exception
    {
        public PermissionsException(string message) : base(message)
        {

        }
    }
}