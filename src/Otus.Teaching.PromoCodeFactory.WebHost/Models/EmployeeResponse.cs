using System;
using System.Collections.Generic;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class EmployeeResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public RoleItemResponse Role { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}