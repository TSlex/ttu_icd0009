﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resourses.Views.ChatRooms {
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
    public class ChatRooms {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ChatRooms() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resourses.Views.ChatRooms.ChatRooms", typeof(ChatRooms).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You cannot send messages to this chat!.
        /// </summary>
        public static string CannotWrite {
            get {
                return ResourceManager.GetString("CannotWrite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Leave.
        /// </summary>
        public static string LeaveNav {
            get {
                return ResourceManager.GetString("LeaveNav", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Members.
        /// </summary>
        public static string MembersNav {
            get {
                return ResourceManager.GetString("MembersNav", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Change Title.
        /// </summary>
        public static string RenameNav {
            get {
                return ResourceManager.GetString("RenameNav", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Write.
        /// </summary>
        public static string WriteButton {
            get {
                return ResourceManager.GetString("WriteButton", resourceCulture);
            }
        }
    }
}
