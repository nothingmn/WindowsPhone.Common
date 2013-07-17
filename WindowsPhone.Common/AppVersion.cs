using System;
using WindowsPhone.Contracts.Core;

namespace WindowsPhone.Common
{
    /// <summary>
    /// The major number is increased when there are significant jumps in functionality, 
    /// the minor number is incremented when only minor features or significant fixes have been added, 
    /// The revision number is incremented when minor bugs are fixed.
    /// the build number is incremented as part of the CI projects, for the given major.minor.revision.
    /// http://en.wikipedia.org/wiki/Software_versioning
    /// </summary>
    public class AppVersion : IVersion,  IEquatable<IVersion>, IComparable, IComparable<IVersion>
    {
        public AppVersion() : this(typeof(AppVersion)) {
        }
        public AppVersion(Type type) : this(type.FullName)
        {
        }
        public AppVersion(string typeName)
        {
            try
            {
                var t = Type.GetType(typeName);
                if (t != null)
                {
                    var v = t.Assembly.GetName().Version;
                    if (v != null)
                    {
                        this.Major = v.Major;
                        this.Minor = v.Minor;
                        this.Revision = v.Revision;
                        this.Build = v.Build;
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public AppVersion(int major = 0, int minor = 0, int revision = 0, int build = 0) : this()
        {
            Major = major;
            Minor = minor;
            Revision = revision;
            Build = build;
        }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Revision { get; set; }
        public int Build { get; set; }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", Major, Minor, Revision, Build);
        }

        public static bool TryParse(string input,out IVersion result)
        {
            result = new AppVersion();
            try
            {
                result = new AppVersion();
                string[] parts = input.Split('.');
                for (int i = 0; i < parts.Length; i++)
                {
                    int version = 0;
                    if (int.TryParse(parts[i], out version))
                    {
                        switch (i)
                        {
                            case 0:
                                result.Major = version;
                                break;
                            case 1:
                                result.Minor = version;
                                break;
                            case 2:
                                result.Revision = version;
                                break;
                            default:
                                result.Build = version;
                                break;

                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;

            }

        }

        public override bool Equals(System.Object obj)
        {
            // If parameter cannot be cast to ThreeDPoint return false:
            var p = obj as AppVersion;
            if ((object)p == null)
            {
                return false;
            }
            return Equals(p);
        }

        public bool Equals(IVersion p)
        {
            // Return true if the fields match:
            return Major == p.Major && Minor == p.Minor && Revision == p.Revision && Build == p.Build;
        }


        public static bool operator ==(AppVersion v1, IVersion v2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(v1, v2))
            {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)v1 == null) || ((object)v2 == null))
            {
                return false;
            }
            return (v1.Major == v2.Major && v1.Minor == v2.Minor && v1.Revision == v2.Revision && v1.Build == v2.Build);
        }

        public static bool operator !=(AppVersion v1, IVersion v2)
        {
            return !(v1 == v2);
        }

        public static bool operator >(AppVersion v1, IVersion v2)
        {
            if (v1.Major > v2.Major) return true;
            if (v1.Minor > v2.Minor) return true;
            if (v1.Revision > v2.Revision) return true;
            if (v1.Build > v2.Build) return true;
            return false;
        }

        public static bool operator <(AppVersion v1, IVersion v2)
        {
            if (v1.Major < v2.Major) return true;
            if (v1.Minor < v2.Minor) return true;
            if (v1.Revision < v2.Revision) return true;
            if (v1.Build < v2.Build) return true;
            return false;
        }


        public int CompareTo(IVersion other)
        {
            if (Major > other.Major || Minor > other.Minor || Revision > other.Revision || Build > other.Build) return 1;
            if (Major == other.Major && Minor == other.Minor && Revision == other.Revision && Build == other.Build) return 0;
            //if (Major < other.Major || Minor < other.Minor || Revision < other.Revision || Build < other.Build) return -1;
            return -1;

        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as IVersion);

        }
    }
}
