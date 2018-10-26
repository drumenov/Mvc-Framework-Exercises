using SIS.Demo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Demo.Services.Contracts
{
    public interface ITrackService
    {
        Track GetTrackById(string id);

        void AddTrackToDb(Track track, string albumId);
    }
}
