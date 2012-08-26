using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Oss
{
    [XmlRoot("Owner")]
    public class Owner : ICloneable
    {
        public Owner()
        {
        }

        public Owner(string id, string displayName)
        {
            this.Id = id;
            this.DisplayName = displayName;
        }

        public object Clone()
        {
            return new Owner(this.Id, this.DisplayName);
        }

        public override string ToString()
        {
            object[] temp = new object[] { this.Id ?? string.Empty, this.DisplayName ?? string.Empty };
            return string.Format(CultureInfo.InvariantCulture, "[Owner Id={0}, DisplayName={1}]", temp);
        }

        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }

        [XmlElement("ID")]
        public string Id { get; set; }
    }
}
