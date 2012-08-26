using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss
{
    public class Bucket
    {
        public Bucket(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "OSS Bucket [Name={0}], [Owner={1}], [CreationTime={2}]", new object[] { this.Name, this.Owner, this.CreationDate });
        }

        public DateTime CreationDate { get; internal set; }

        public string Name { get; internal set; }

        public Owner Owner { get; internal set; }
    }
}
