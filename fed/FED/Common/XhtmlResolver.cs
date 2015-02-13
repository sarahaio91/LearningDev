using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    /// <summary>
    /// <para>XmlResolver for XHTML</para>
    /// </summary>
    public class XhtmlResolver : XmlUrlResolver
    {
        private const string ResourcePrefix = "Common.";

        private static readonly Dictionary<string, string> _knownDtds = new Dictionary<string, string>
        {
            { "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd", ResourcePrefix + "xhtml1-strict.dtd" },
            { "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", ResourcePrefix + "xhtml1-transitional.dtd" },
            { "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd", ResourcePrefix + "xhtml1-frameset.dtd" },
            { "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", ResourcePrefix + "xhtml1-transitional.dtd" },
            { "http://www.daisy.org/z3986/2005/ncx-2005-1.dtd", ResourcePrefix + "ncx-2005-1.dtd" },
            { "http://www.w3.org/TR/xhtml1/DTD/-//W3C//ENTITIES Latin 1 for XHTML//EN", ResourcePrefix + "xhtml-lat1.ent" },
            { "http://www.w3.org/TR/xhtml1/DTD/-//W3C//ENTITIES Special for XHTML//EN", ResourcePrefix + "xhtml-special.ent" },
            { "http://www.w3.org/TR/xhtml1/DTD/-//W3C//ENTITIES Symbols for XHTML//EN", ResourcePrefix + "xhtml-symbol.ent" },
            { "http://www.w3.org/TR/xhtml11/DTD/-//W3C//ENTITIES Latin 1 for XHTML//EN", ResourcePrefix + "xhtml-lat1.ent" },
            { "http://www.w3.org/TR/xhtml11/DTD/-//W3C//ENTITIES Special for XHTML//EN", ResourcePrefix + "xhtml-special.ent" },
            { "http://www.w3.org/TR/xhtml11/DTD/-//W3C//ENTITIES Symbols for XHTML//EN", ResourcePrefix + "xhtml-symbol.ent" }
        };

        /// <summary>
        /// <para>URN string to identify all the Entities</para>
        /// </summary>
        private const string ENTITIES_URN = "urn:Entities";

        /// <summary>
        /// <para>Get Entity</para>
        /// </summary>
        public override object GetEntity(
                Uri absoluteUri, string role,
                Type ofObjectToReturn)
        {

            if (absoluteUri.AbsoluteUri == ENTITIES_URN)
            {

                return "<!ENTITY nbsp \"&#x000A0;\" >";
            }

            if (absoluteUri.IsFile)
            {

                return File.OpenRead(absoluteUri.AbsolutePath.Replace("%20", " "));
            }

            if (_knownDtds.ContainsKey(absoluteUri.OriginalString))
            {
                string resourceName = _knownDtds[absoluteUri.OriginalString];
                Assembly assembly = Assembly.GetAssembly(typeof(XhtmlResolver));
                return assembly.GetManifestResourceStream(resourceName);
            }

            return null;
        }

        /// <summary>
        /// <para>Resolves XHTML DOCTYPE</para>
        /// </summary>
        public override Uri ResolveUri(
                Uri baseUri, string relativeUri)
        {

            // make sure the doc is declared as XHTML
            if (relativeUri.Equals("-//W3C//DTD XHTML 1.0 Transitional//EN", StringComparison.OrdinalIgnoreCase)
                || relativeUri.Equals("-//W3C//DTD XHTML 1.0 Strict//EN", StringComparison.OrdinalIgnoreCase)
                || relativeUri.Equals("-//W3C//DTD XHTML 1.0 Frameset//EN", StringComparison.OrdinalIgnoreCase)
                || relativeUri.Equals("-//W3C//DTD XHTML 1.1//EN", StringComparison.OrdinalIgnoreCase))
            {

                return new Uri(ENTITIES_URN);
            }

            return base.ResolveUri(baseUri, relativeUri);
        }
    }
}
