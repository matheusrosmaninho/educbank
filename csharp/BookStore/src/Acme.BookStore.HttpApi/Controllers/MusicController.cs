using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.BookStore.Musics;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using static Acme.BookStore.HttpApiConsts;

namespace Acme.BookStore.Controllers;

[Produces("application/json")]
[Route($"{RouteMusic}")]
[ApiExplorerSettings(GroupName = GroupNameMusic)]
public class MusicController : BookStoreController
{
    private readonly IRepository<Music, Guid> _musicRepository;

    public MusicController(IRepository<Music, Guid> musicRepository)
    {
        _musicRepository = musicRepository;
    }

    [ApiExplorerSettings(GroupName = GroupNameMusic)]
    [HttpGet]
    public async Task<List<Music>> GetMusics()
    {
        var musics = await _musicRepository.GetListAsync();
        return musics;
    }

    [ApiExplorerSettings(GroupName = GroupNameMusic)]
    [HttpGet($"id/{{musicId}}")]
    public async Task<Music> GetMusicAsync(Guid musicId)
    {
        var music = await _musicRepository.GetAsync(musicId);
        return music;
    }
}