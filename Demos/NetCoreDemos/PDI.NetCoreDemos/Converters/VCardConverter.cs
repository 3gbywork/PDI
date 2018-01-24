using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWSoftware.PDI.Objects;
using EWSoftware.PDI.Properties;
using PDI.NetCoreDemos.Models;

namespace PDI.NetCoreDemos.Converters
{
    public class VCardConverter
    {
        public static IEnumerable<VCardDto> CollectionToDtoes(VCardCollection vCards)
        {
            return from vCard in vCards
                   select VCardToDto(vCard, new VCardDto());
        }

        public static VCard DtoToVCard(VCardDto dto)
        {
            var vCard = new VCard
            {
                //general
                Version = "2.1".Equals(dto.Version) ? SpecificationVersions.vCard21 : SpecificationVersions.vCard30,
            };
            vCard.LastRevision.DateTimeValue = dto.LastRevision;
            //name
            vCard.Name.FamilyName = dto.LastName;
            vCard.Name.GivenName = dto.FirstName;
            vCard.Name.AdditionalNames = dto.MiddleName;
            vCard.Name.NamePrefix = dto.Title;
            vCard.Name.NameSuffix = dto.Suffix;
            vCard.SortString.Value = dto.SortString;
            vCard.FormattedName.Value = dto.FormattedName;
            vCard.Nickname.NicknamesString = dto.Nickname;
            //work
            vCard.Organization.Name = dto.Organization;
            vCard.Title.Value = dto.JobTitle;
            vCard.Role.Value = dto.Role;
            vCard.Organization.UnitsString = dto.Units;
            vCard.Categories.CategoriesString = dto.Categories;

            return vCard;
        }

        public static VCardDto VCardToDto(VCard vCard, VCardDto dto)
        {
            //general
            dto.Version = vCard.Version == SpecificationVersions.vCard21 ? "2.1" : "3.0";
            dto.LastRevision = vCard.LastRevision.DateTimeValue;
            //name
            dto.SortableName = vCard.Name.SortableName;
            dto.LastName = vCard.Name.FamilyName;
            dto.FirstName = vCard.Name.GivenName;
            dto.MiddleName = vCard.Name.AdditionalNames;
            dto.Title = vCard.Name.NamePrefix;
            dto.Suffix = vCard.Name.NameSuffix;
            dto.SortString = vCard.SortString.Value;
            dto.FormattedName = vCard.FormattedName.Value;
            dto.Nickname = vCard.Nickname.NicknamesString;
            //work
            dto.Organization = vCard.Organization.Name;
            dto.JobTitle = vCard.Title.Value;
            dto.Role = vCard.Role.Value;
            dto.Units = vCard.Organization.UnitsString;
            dto.Categories = vCard.Categories.CategoriesString;

            return dto;
        }
    }
}
