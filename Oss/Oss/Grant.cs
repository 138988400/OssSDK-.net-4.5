using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oss
{
    public class Grant
    {
        public Grant(IGrantee grantee, Permission permission)
        {
            if (grantee == null)
            {
                throw new ArgumentNullException("grantee");
            }
            this.Grantee = grantee;
            this.Permission = permission;
        }

        public override bool Equals(object obj)
        {
            Grant g = obj as Grant;
            if (g == null)
            {
                return false;
            }
            return ((this.Grantee.Identifier == g.Grantee.Identifier) && (this.Permission == g.Permission));
        }

        public override int GetHashCode()
        {
            return (this.Grantee.Identifier + ":" + this.Permission.ToString()).GetHashCode();
        }

        public IGrantee Grantee { get; private set; }

        public Permission Permission { get; private set; }
    }
}
