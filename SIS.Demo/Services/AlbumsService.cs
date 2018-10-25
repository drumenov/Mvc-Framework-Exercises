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

        public Album GetAlbumById(string id) {
            return this.dbContext.Albums.FirstOrDefault(a => a.Id == id);
        }

        public ICollection<Album> GetAllAlbums(string username) {
            ICollection<Album> allUserAlbums = this.dbContext.Users.First(u => u.Username == username).Albums.Select(ua => ua.Album).ToList<Album>();
            if(allUserAlbums.Count == 0) {
                allUserAlbums.Add(new Album {
                    Name = "There are currenly no albums." //This is neede so that if there are not albums for the current user a message indicationg that to be displayed without adding another View for handling this particular case.
                });
            }
            return allUserAlbums;
        }
    }
}
