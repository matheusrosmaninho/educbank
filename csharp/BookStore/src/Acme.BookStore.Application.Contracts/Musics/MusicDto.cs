using System;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Musics;

public class MusicDto : FullAuditedEntityDto<Guid>
{
    public string Name {get; set;}

    public int Star {get; set;}

    public string Description {get; set;}
}