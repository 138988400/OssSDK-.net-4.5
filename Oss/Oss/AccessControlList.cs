using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    public class AccessControlList
    {
        private HashSet<Grant> _grants = new HashSet<Grant>();

        public void GrantPermission(IGrantee grantee, Permission permission)
        {
            if (grantee == null)
            {
                throw new ArgumentNullException("grantee");
            }
            this._grants.Add(new Grant(grantee, permission));
        }

        public void RevokeAllPermissions(IGrantee grantee)
        {
            if (grantee == null)
            {
                throw new ArgumentNullException("grantee");
            }
            this._grants.RemoveWhere(e => e.Grantee == grantee);
        }

        public override string ToString()
        {
            StringBuilder grantsBuilder = new StringBuilder();
            foreach (Grant g in this.Grants)
            {
                grantsBuilder.Append(g.ToString()).Append(",");
            }
            return string.Format(CultureInfo.InvariantCulture, "[AccessControlList: Owner={0}, Grants={1}]", new object[] { this.Owner, grantsBuilder.ToString() });
        }

        public IEnumerable<Grant> Grants
        {
            get
            {
                return this._grants;
            }
        }

        public Owner Owner { get; set; }
    }
}
