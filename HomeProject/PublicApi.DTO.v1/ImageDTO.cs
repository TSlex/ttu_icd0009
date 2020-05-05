using System;
using System.Collections.Generic;
using Domain.Enums;

namespace PublicApi.DTO.v1
{
    public class ImageDTO
    {
        public Guid? Id { get; set; }
        public string ImageScr { get; set; }
    }
}