using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum ImageType
    {
        [Display(Name = nameof(ProfileAvatar), ResourceType = typeof(Resourses.Views.Enums.ImageType))]
        ProfileAvatar = 0,

        [Display(Name = nameof(Post), ResourceType = typeof(Resourses.Views.Enums.ImageType))]
        Post,

        [Display(Name = nameof(Gift), ResourceType = typeof(Resourses.Views.Enums.ImageType))]
        Gift,

        [Display(Name = "Misc", ResourceType = typeof(Resourses.Views.Enums.ImageType))]
        Undefined
    }
}