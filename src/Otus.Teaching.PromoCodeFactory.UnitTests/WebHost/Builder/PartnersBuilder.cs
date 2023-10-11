using System;
using System.Collections.Generic;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Builder;

public static class PartnersBuilder
{
    public static Partner CreateBasePartner()
    {
        var partner = new Partner()
        {
            Id = Guid.NewGuid(),
            Name = "Mark&Spansor",
            IsActive = true,
            PartnerLimits = new List<PartnerPromoCodeLimit>() {
                new PartnerPromoCodeLimit() {
                    Id = Guid.NewGuid(),
                    CreateDate = new DateTime(2023, 01, 1),
                    EndDate = new DateTime(2023, 12, 31),
                    Limit = 100
                }
            }
        };

        return partner;
    }

    public static Partner SetNotActive(this Partner partner)
    {
        partner.IsActive = false;
        return partner;
    }

    public static Partner SetNotActiveLimit(this Partner partner)
    {
        partner.PartnerLimits = new List<PartnerPromoCodeLimit> {
            new PartnerPromoCodeLimit() {
                Id = Guid.NewGuid(),
                CreateDate = new DateTime(2023, 01, 1),
                EndDate = new DateTime(2023, 12, 31),
                CancelDate = new DateTime(2023, 03, 15),
                Limit = 100
            }
        };

        return partner;
    }

    public static Partner SetActiveLimit(this Partner partner)
    {
        partner.PartnerLimits = new List<PartnerPromoCodeLimit> {
            new PartnerPromoCodeLimit() {
                Id = Guid.NewGuid(),
                CreateDate = new DateTime(2023, 01, 1),
                EndDate = new DateTime(2023, 12, 31),
                Limit = 100
            }
        };
        partner.NumberIssuedPromoCodes = 20;

        return partner;
    }

    public static Partner ClearNumberIssuedPromoCodes(this Partner partner)
    {
        partner.NumberIssuedPromoCodes = 20;

        return partner;
    }

    public static Partner SetNegativeLimit(this Partner partner)
    {
        partner.PartnerLimits = new List<PartnerPromoCodeLimit> {
            new PartnerPromoCodeLimit() {
                Id = Guid.NewGuid(),
                CreateDate = new DateTime(2023, 01, 1),
                EndDate = new DateTime(2023, 12, 31),
                CancelDate = new DateTime(2023, 03, 15),
                Limit = -100
            }
        };
        return partner;
    }
}

