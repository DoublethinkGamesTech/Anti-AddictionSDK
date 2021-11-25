using System;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor.Android;


namespace Doublethink.Editor.BuildPostprocess
{
    public class AndroidConfigurator : IPostGenerateGradleAndroidProject
    {
        private string _manifestFilePath;

        public int callbackOrder
        {
            get { return 1; }
        }

        public void OnPostGenerateGradleAndroidProject(string basePath)
        {
            // If needed, add condition checks on whether you need to run the modification routine.
            // For example, specific configuration/app options enabled
            var androidManifest = new AndroidManifest(GetManifestPath(basePath));

            androidManifest.SetApplicationName("com.doublethink.Application.GameApplication");
            androidManifest.SetLaunchMode("singleTop");
            androidManifest.SetHardwareAccel();

            // Add your XML manipulation routines
            androidManifest.Save();
        }

        private string GetManifestPath(string basePath)
        {
            if (string.IsNullOrEmpty(_manifestFilePath))
            {
                var pathBuilder = new StringBuilder(basePath);
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
                _manifestFilePath = pathBuilder.ToString();
            }
            return _manifestFilePath;
        }

        internal class AndroidXmlDocument : XmlDocument
        {
            private string m_Path;
            protected XmlNamespaceManager nsMgr;
            public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
            public AndroidXmlDocument(string path)
            {
                m_Path = path;
                using (var reader = new XmlTextReader(m_Path))
                {
                    reader.Read();
                    Load(reader);
                }
                nsMgr = new XmlNamespaceManager(NameTable);
                nsMgr.AddNamespace("android", AndroidXmlNamespace);
            }

            public string Save()
            {
                return SaveAs(m_Path);
            }

            public string SaveAs(string path)
            {
                using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
                {
                    writer.Formatting = Formatting.Indented;
                    Save(writer);
                }
                return path;
            }
        }


        internal class AndroidManifest : AndroidXmlDocument
        {
            private readonly XmlElement ApplicationElement;

            public AndroidManifest(String path) : base(path)
            {
                ApplicationElement = SelectSingleNode("/manifest/application") as XmlElement;
            }

            private XmlAttribute CreateAndroidAttribute(String key, String value)
            {
                XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
                attr.Value = value;
                return attr;
            }

            internal XmlNode GetActivityWithLaunchIntent()
            {
                return SelectSingleNode("/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and " +
                        "intent-filter/category/@android:name='android.intent.category.LAUNCHER']", nsMgr);
            }

            internal void SetApplicationTheme(String appTheme)
            {
                ApplicationElement.Attributes.Append(CreateAndroidAttribute("theme", appTheme));
            }

            internal void SetApplicationName(string applicationName)
            {
                ApplicationElement.Attributes.Append(CreateAndroidAttribute("name", applicationName));
            }

            internal void SetStartingActivityName(String activityName)
            {
                GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("name", activityName));
            }
            internal void SetLaunchMode(String launchMode)
            {
                GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("launchMode", launchMode));
            }
            internal void SetHardwareAccel()
            {
                GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("hardwareAccelerated", "true"));
            }
        }
    }
}
