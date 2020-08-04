using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.aleksi.Contracts.DAL.Base;
using ee.itcollege.aleksi.DAL.Base;

namespace PublicApi.DTO.v1
{
    /// <summary>
    /// Get only
    /// </summary>
    public class RankDTO
    {
        public string RankTitle { get; set; } = default!;
        public string? RankDescription { get; set; }

        public string? RankIcon { get; set; }

        public string RankColor { get; set; } = default!;
        public string RankTextColor { get; set; } = default!;

        public int MaxExperience { get; set; } = default!;
        public int MinExperience { get; set; } = default!;
    }

    public class RankAdminDTO : DomainEntityBaseMetaSoftUpdateDelete
    {
        [Display(Name = nameof(RankCode), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string RankCode { get; set; } = default!;

        [Display(Name = nameof(RankTitleId), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        public Guid RankTitleId { get; set; } = default!;

        [Display(Name = nameof(RankTitle), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string? RankTitle { get; set; } = default!;

        [Display(Name = nameof(RankDescriptionId), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        public Guid? RankDescriptionId { get; set; }

        [Display(Name = nameof(RankDescription), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(300, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? RankDescription { get; set; }

        [Display(Name = nameof(RankColor), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string RankColor { get; set; } = default!;

        [Display(Name = nameof(RankTextColor), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public string RankTextColor { get; set; } = default!;

        [Display(Name = nameof(RankIcon), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        public string? RankIcon { get; set; }

        [Display(Name = nameof(MaxExperience), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public int MaxExperience { get; set; } = default!;

        [Display(Name = nameof(MinExperience), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        [Required(ErrorMessageResourceType = typeof(Resourses.BLL.App.DTO.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        public int MinExperience { get; set; } = default!;

        [Display(Name = nameof(PreviousRankId), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        public Guid? PreviousRankId { get; set; }

        [Display(Name = nameof(NextRankId), ResourceType = typeof(Resourses.BLL.App.DTO.Ranks.Ranks))]
        public Guid? NextRankId { get; set; }
    }
}