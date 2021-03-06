using System;
using System.Linq;
using System.Reflection;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An attribute used to identify the corresponding NuGet package and Id for an assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class NuGetPackageAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageAttribute" /> class.
        /// </summary>
        public NuGetPackageAttribute() : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageAttribute" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public NuGetPackageAttribute(string id) : this(id, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageAttribute" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        public NuGetPackageAttribute(string id, string version)
        {
            Id = id;
            Version = version;
        }

        private string Id { get; set; }

        private string Version { get; set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The NuGet package Id for the specified <paramref name="assembly"/>.</returns>
        public string GetId(Assembly assembly)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                return Id;
            }

            return Id = assembly.GetName().Name;
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The NuGet package version for the specified <paramref name="assembly"/>.</returns>
        public string GetVersion(Assembly assembly)
        {
            if (!string.IsNullOrEmpty(Version))
            {
                return Version;
            }

            var infoVersion = assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false).Cast<AssemblyInformationalVersionAttribute>().SingleOrDefault();
            if (infoVersion != null)
            {
                return Version = infoVersion.InformationalVersion;
            }

            return Version = assembly.GetName().Version.ToString();
        }
    }
}