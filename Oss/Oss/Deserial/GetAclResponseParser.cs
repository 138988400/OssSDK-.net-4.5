using Oss.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Oss.Utilities;

namespace Oss.Deserial
{
    internal class GetAclResponseParser : ResponseDeserializer<AccessControlList, AccessControlPolicy>
    {
       // private IDeserializer<System.IO.Stream, AccessControlPolicy> deserializer;

        public GetAclResponseParser(IDeserializer<Stream, AccessControlPolicy> contentDeserializer)
            : base(contentDeserializer)
        {
        }

        //public GetAclResponseParser(IDeserializer<Stream, AccessControlPolicy> deserializer)
        //{
        //    // TODO: Complete member initialization
        //    this.deserializer = deserializer;
        //}

        public override async Task<AccessControlList> Deserialize(HttpResponseMessage response)
        {
            AccessControlPolicy model = base.ContentDeserializer.Deserialize(await response.Content.ReadAsStreamAsync());
            AccessControlList acl = new AccessControlList
            {
                Owner = new Owner(model.Owner.Id, model.Owner.DisplayName)
            };
            foreach (string grant in model.Grants)
            {
                if (grant == CannedAccessControlList.PublicRead.GetStringValue())
                {
                    acl.GrantPermission(GroupGrantee.AllUsers, Permission.Read);
                }
                else if (grant == CannedAccessControlList.PublicReadWrite.GetStringValue())
                {
                    acl.GrantPermission(GroupGrantee.AllUsers, Permission.FullControl);
                }
            }
            return acl;
        }
    }
}
