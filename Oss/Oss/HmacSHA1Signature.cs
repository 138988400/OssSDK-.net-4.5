using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    internal class HmacSHA1Signature : ServiceSignature
    {
        private Encoding _encoding = Encoding.UTF8;

        protected override string ComputeSignatureCore(string key, string data)
        {
            using (KeyedHashAlgorithm algorithm = KeyedHashAlgorithm.Create(this.SignatureMethod.ToString().ToUpperInvariant()))
            {
                algorithm.Key = this._encoding.GetBytes(key.ToCharArray());
                return Convert.ToBase64String(algorithm.ComputeHash(this._encoding.GetBytes(data.ToCharArray())));
            }
        }

        public override string SignatureMethod
        {
            get
            {
                return "HmacSHA1";
            }
        }

        public override string SignatureVersion
        {
            get
            {
                return "1";
            }
        }
    }
}
