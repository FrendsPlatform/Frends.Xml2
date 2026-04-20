using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frends.XSLT.Transform.Definitions
{
    /// <summary>
    /// Transform task options.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Controls whether external entities are allowed in the XML processing.
        /// </summary>
        /// <example>false</example>
        public bool EnableExternalEntities { get; set; } = false;
    }
}
