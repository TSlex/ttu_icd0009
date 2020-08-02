using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum ProfileGender
    {
        [Display(Name = "GenderMale", ResourceType = typeof(Resourses.Views.Enums.Gender))]
        Male = 0,

        [Display(Name = "GenderFemale", ResourceType = typeof(Resourses.Views.Enums.Gender))]
        Female,

        [Display(Name = "GenderOwn", ResourceType = typeof(Resourses.Views.Enums.Gender))]
        Own = 127,

        [Display(Name = "GenderUndefined", ResourceType = typeof(Resourses.Views.Enums.Gender))]
        Undefined
    }
}