using System;
using Domain.Enums;

namespace WebApp.Helpers
{
    /// <summary>
    /// Helps translate stuff
    /// </summary>
    public static class Translations
    {
        /// <summary>
        /// Return gender translation
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string GetProfileGenderTranslations(ProfileGender gender)
        {
            switch (gender)
            {
                case ProfileGender.Male:
                    return Resourses.Views.Enums.Gender.GenderMale;
                case ProfileGender.Female:
                    return Resourses.Views.Enums.Gender.GenderFemale;
                case ProfileGender.Own:
                    return Resourses.Views.Enums.Gender.GenderOwn;
                case ProfileGender.Undefined:
                    return Resourses.Views.Enums.Gender.GenderUndefined;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
        }

        /// <summary>
        /// Return type of image translation
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string GetImageTypeTranslations(ImageType type)
        {
            switch (type)
            {
                case ImageType.ProfileAvatar:
                    return Resourses.Views.Enums.ImageType.ProfileAvatar;
                case ImageType.Post:
                    return Resourses.Views.Enums.ImageType.Post;
                case ImageType.Gift:
                    return Resourses.Views.Enums.ImageType.Gift;
                case ImageType.Undefined:
                    return Resourses.Views.Enums.ImageType.Misc;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}