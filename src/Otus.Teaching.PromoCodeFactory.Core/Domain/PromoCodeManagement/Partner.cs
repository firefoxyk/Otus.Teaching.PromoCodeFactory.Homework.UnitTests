using System;
using System.Collections.Generic;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Partner
        : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int NumberIssuedPromoCodes  { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<PartnerPromoCodeLimit> PartnerLimits { get; set; }
    }
}