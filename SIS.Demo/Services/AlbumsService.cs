using SIS.Demo.Data;
using SIS.Demo.Models;
using SIS.Demo.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.Demo.Services
{
    public class AlbumsService : IAlbumsService
    {
        private readonly IRunesDbContext dbContext;

        public AlbumsService(IRunesDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public ICollection<Album> GetAllAlbums(string username) {
            var tre = this.dbContext.Albums.ToArray();
            var ert = this.dbContext.UsersAlbums.ToArray();
            ICollection<Album> allUserAlbums = this.dbContext.Users.First(u => u.Username == username).Albums.Select(ua => ua.Album).ToList();
            return allUserAlbums;
        }
    }
}
