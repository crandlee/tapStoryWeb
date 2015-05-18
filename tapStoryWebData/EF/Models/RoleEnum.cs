using System;

namespace tapStoryWebData.EF.Models
{
    public sealed class Roles
    {
        private readonly String _name;
        private readonly int _value;

        public static readonly Roles User = new Roles(1, "user");
        public static readonly Roles Admin = new Roles(2, "admin");
        public static readonly Roles SuperAdmin = new Roles(3, "superadmin");

        public Roles(int value, string name)
        {
            _name = name;
            _value = value;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
