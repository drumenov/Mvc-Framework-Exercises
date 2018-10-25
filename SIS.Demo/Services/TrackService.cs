using SIS.Demo.Data;
using SIS.Demo.Models;
using SIS.Demo.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.Demo.Services
{
    public class TrackService : ITrackService
    {
        private readonly IRunesDbContext dbContext;

        public TrackService(IRunesDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public Track GetTrackById(string id) {
            return this.dbContext.Tracks.FirstOrDefault(t => t.Id == id);
        }
    }
}
