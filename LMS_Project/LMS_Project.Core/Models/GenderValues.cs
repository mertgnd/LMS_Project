using LMS_Project.Common.Enums;
using System.Collections.Generic;

namespace LMS_Project.Core.Models
{
    public class GenderValues
    {
        public static readonly Dictionary<string, GenderEnum> Values = new Dictionary<string, GenderEnum>
        {
            {"Male", GenderEnum.Male },
            {"Female", GenderEnum.Female },
            {"Erkek", GenderEnum.Male},
            {"Kadın", GenderEnum.Female},
            {"Musko", GenderEnum.Male },
            {"Zensko", GenderEnum.Female },
            {"M", GenderEnum.Male },
            {"F", GenderEnum.Female },
            {"E", GenderEnum.Male },
            {"Z", GenderEnum.Female },
            {"K", GenderEnum.Female }
        };
    }
}