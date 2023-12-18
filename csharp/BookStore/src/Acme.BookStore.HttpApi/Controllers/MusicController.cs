using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Acme.BookStore.Musics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using static Acme.BookStore.HttpApiConsts;

namespace Acme.BookStore.Controllers;

[Produces("application/json")]
[Route($"{RouteMusic}")]
[ApiExplorerSettings(GroupName = GroupNameMusic)]
public class MusicController : BookStoreController
{
    private readonly IRepository<Music, Guid> _musicRepository;

    private readonly IFileAppService _fileAppService;

    public MusicController(IRepository<Music, Guid> musicRepository, IFileAppService fileAppService)
    {
        _musicRepository = musicRepository;
        _fileAppService = fileAppService;
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

    [ApiExplorerSettings(GroupName = GroupNameMusic)]
    [HttpDelete($"id/{{musicId}}")]
    public async Task<IActionResult> DeleteMusicAsync(Guid musicId)
    {
        try {
            await _musicRepository.DeleteAsync(musicId);
            return Ok($"Registro \"{musicId}\"  deletado com sucesso.");
        } catch(BusinessException exception) {
            return BadRequest($"Falha ao executar a remoção: {exception}");
        }
    }

    [ApiExplorerSettings(GroupName = GroupNameMusic)]
    [HttpPut($"id/{{musicId}}")]
    public async Task<MusicDto> UpdateMusicAsync(Guid musicId, string name, int star)
    {
        var music = await _musicRepository.GetAsync(musicId);

        if(music == null) {
            BadRequest("Musica não encontrada");
            return new MusicDto{};
        }

        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNull(star, nameof(star));

        music.Name = name;
        music.Star = star;

        try {
            await _musicRepository.UpdateAsync(music);
            return new MusicDto{
                Name = music.Name,
                Star = music.Star
            };
        } catch(Exception exception) {
            BadRequest("Falha ao processar a requisição");
            return new MusicDto{};
        }
    }
}