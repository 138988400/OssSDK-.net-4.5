using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    public sealed class GroupGrantee : IGrantee
    {
        private static GroupGrantee _allUsers = new GroupGrantee("http://oss.service.aliyun.com/acl/group/ALL_USERS");
        private string _identifier;

        private GroupGrantee(string identifier)
        {
            this._identifier = identifier;
        }

        public override bool Equals(object obj)
        {
            GroupGrantee g = obj as GroupGrantee;
            if (g == null)
            {
                return false;
            }
            return (g.Identifier == this.Identifier);
        }

        public override int GetHashCode()
        {
            return ("[GroupGrantee ID=" + this.Identifier + "]").GetHashCode();
        }

        public static GroupGrantee AllUsers
        {
            get
            {
                return _allUsers;
            }
        }

        public string Identifier
        {
            get
            {
                return this._identifier;
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}
