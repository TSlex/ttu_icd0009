﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resourses.BLL.App.DTO.Comments {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Comments {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Comments() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resourses.BLL.App.DTO.Comments.Comments", typeof(Comments).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
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
        ///   Ищет локализованную строку, похожую на .
        /// </summary>
        public static string CommentDateTime {
            get {
                return ResourceManager.GetString("CommentDateTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на .
        /// </summary>
        public static string CommentValue {
            get {
                return ResourceManager.GetString("CommentValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на .
        /// </summary>
        public static string Post {
            get {
                return ResourceManager.GetString("Post", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на .
        /// </summary>
        public static string PostId {
            get {
                return ResourceManager.GetString("PostId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на .
        /// </summary>
        public static string Profile {
            get {
                return ResourceManager.GetString("Profile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на .
        /// </summary>
        public static string ProfileId {
            get {
                return ResourceManager.GetString("ProfileId", resourceCulture);
            }
        }
    }
}
