using EWSoftware.PDI.Objects;
using EWSoftware.PDI.Parser;
using Microsoft.Extensions.FileProviders;
using PDI.NetCoreDemos.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PDI.NetCoreDemos.Data
{
    public class DbInitializer
    {
        public static void Initialize(VCardContext context, IFileProvider fileProvider)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.VCards.Any())
            {
                return;   // DB has been seeded
            }

            // Read from vCard file.
            var fileInfo = fileProvider.GetFileInfo("/DemoFiles/RFC2426.vcf");
            if (!fileInfo.Exists) return;

            using (var stream = fileInfo.CreateReadStream())
            {
                var vCards = VCardParser.ParseFromStream(new StreamReader(stream));
                vCards.Sort(VCardSorter);
                // Convert to Dto
                var vCardDtoes = VCardConverter.CollectionToDtoes(vCards);
                foreach (var vCardDto in vCardDtoes)
                {
                    context.VCards.Add(vCardDto);
                }
                context.SaveChanges();
            }
        }

        private static int VCardSorter(VCard x, VCard y)
        {
            string sortName1, sortName2;

            // Get the names to compare.  Precedence is given to the SortStringProperty as that is the purpose
            // of its existence.
            sortName1 = x.SortString.Value;

            if (String.IsNullOrWhiteSpace(sortName1))
                sortName1 = x.Name.SortableName;

            sortName2 = y.SortString.Value;

            if (String.IsNullOrWhiteSpace(sortName2))
                sortName2 = y.Name.SortableName;

            // For descending order, change this to compare name 2 to name 1 instead.
            return String.Compare(sortName1, sortName2, StringComparison.CurrentCulture);
        }
    }
}
