﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gallery {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Config {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Config() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gallery.Config", typeof(Config).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /{0}/{1}.
        /// </summary>
        internal static string AlbumUriTemplate {
            get {
                return ResourceManager.GetString("AlbumUriTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ConfigServiceUri.
        /// </summary>
        internal static string ConfigServiceSetting {
            get {
                return ResourceManager.GetString("ConfigServiceSetting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Emily&apos;s Gallery.
        /// </summary>
        internal static string DefaultTitle {
            get {
                return ResourceManager.GetString("DefaultTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to emily_parker.
        /// </summary>
        internal static string DefaultUsername {
            get {
                return ResourceManager.GetString("DefaultUsername", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DefaultUsername.
        /// </summary>
        internal static string DefaultUserNameKey {
            get {
                return ResourceManager.GetString("DefaultUserNameKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /{0}.
        /// </summary>
        internal static string GalleryUriTemplate {
            get {
                return ResourceManager.GetString("GalleryUriTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MobileMe Gallery - {0}.
        /// </summary>
        internal static string MobileMePageTitleFormat {
            get {
                return ResourceManager.GetString("MobileMePageTitleFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hide Options.
        /// </summary>
        internal static string OptionsButtonTextHide {
            get {
                return ResourceManager.GetString("OptionsButtonTextHide", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Show Options.
        /// </summary>
        internal static string OptionsButtonTextShow {
            get {
                return ResourceManager.GetString("OptionsButtonTextShow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ../GalleryService.
        /// </summary>
        internal static string ServiceUri {
            get {
                return ResourceManager.GetString("ServiceUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GalleryServiceUri.
        /// </summary>
        internal static string ServiceUriKey {
            get {
                return ResourceManager.GetString("ServiceUriKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CurrentUsername.
        /// </summary>
        internal static string UsernameKey {
            get {
                return ResourceManager.GetString("UsernameKey", resourceCulture);
            }
        }
    }
}
