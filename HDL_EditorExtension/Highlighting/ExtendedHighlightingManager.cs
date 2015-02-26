using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Highlighting;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace HDL_EditorExtension.Highlighting
{
    public sealed class ExtendedHighlightingManager : HighlightingManager
    {
        public new static readonly ExtendedHighlightingManager Instance = new ExtendedHighlightingManager();

        public ExtendedHighlightingManager()
        {
            ExtentionResources.RegisterBuiltInHighlightings(this);
        }

        // Registering a built-in highlighting
        public void RegisterHighlighting(string name, string[] extensions, string resourceName)
        {
            try
            {
#if DEBUG
                // don't use lazy-loading in debug builds, show errors immediately
                ICSharpCode.AvalonEdit.Highlighting.Xshd.XshdSyntaxDefinition xshd;
                using (Stream s = ExtentionResources.OpenStream(resourceName))
                {
                    using (XmlTextReader reader = new XmlTextReader(s))
                    {
                        xshd = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.LoadXshd(reader);
                    }
                }
                Debug.Assert(name == xshd.Name);
                if (extensions != null)
                    Debug.Assert(System.Linq.Enumerable.SequenceEqual(extensions, xshd.Extensions));
                else
                    Debug.Assert(xshd.Extensions.Count == 0);

                // round-trip xshd:
                //					string resourceFileName = Path.Combine(Path.GetTempPath(), resourceName);
                //					using (XmlTextWriter writer = new XmlTextWriter(resourceFileName, System.Text.Encoding.UTF8)) {
                //						writer.Formatting = Formatting.Indented;
                //						new Xshd.SaveXshdVisitor(writer).WriteDefinition(xshd);
                //					}
                //					using (FileStream fs = File.Create(resourceFileName + ".bin")) {
                //						new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(fs, xshd);
                //					}
                //					using (FileStream fs = File.Create(resourceFileName + ".compiled")) {
                //						new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(fs, Xshd.HighlightingLoader.Load(xshd, this));
                //					}

                RegisterHighlighting(name, extensions, ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd, this));
#else
					RegisterHighlighting(name, extensions, LoadHighlighting(resourceName));
#endif
            }
            catch (HighlightingDefinitionInvalidException ex)
            {
                throw new InvalidOperationException("The built-in highlighting '" + name + "' is invalid.", ex);
            }
        }

        Func<IHighlightingDefinition> LoadHighlighting(string resourceName)
        {
            Func<IHighlightingDefinition> func = delegate
            {
                ICSharpCode.AvalonEdit.Highlighting.Xshd.XshdSyntaxDefinition xshd;
                using (Stream s = ExtentionResources.OpenStream(resourceName))
                {
                    using (XmlTextReader reader = new XmlTextReader(s))
                    {
                        // in release builds, skip validating the built-in highlightings
                        xshd = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.LoadXshd(reader);
                    }
                }
                return ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd, this);
            };
            return func;
        }
    }
}
