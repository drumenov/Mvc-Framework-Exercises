using SIS.Demo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Demo.Services.Contracts
{
    public interface IAlbumsService
    {
        ICollection<Album> GetAllAlbums(string userId);

        Album GetAlbumById(string id);
    }
}
