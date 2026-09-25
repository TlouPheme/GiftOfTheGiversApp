using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftOfTheGivers.Helpers
{
    public class DonationHelper
    {
        public static string FormatCertificateNumber(int donationId, DateTime donationDate)
        {
            return $"GOTG-{donationDate.Year}-{donationId:D6}";
        }
    }
}