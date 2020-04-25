﻿using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Mappers;

namespace DAL.Repositories
{
    public class ProfileRankRepo : BaseRepo<Domain.ProfileRank, ProfileRank, ApplicationDbContext>, IProfileRankRepo
    {
        public ProfileRankRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new ProfileRankMapper())
        {
        }
    }
}