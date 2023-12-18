using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Musics;

public class Music : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }

    public int Star { get; set; }

    public string Description { get; set; }

    public string ImageProfile { get; set; } = "";
}