using SIS.Demo.Models;
using SIS.Demo.Services.Contracts;
using SIS.Demo.ViewModels;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Controllers.Base;
using SIS.HTTP.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Demo.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumsService albumsService;

        public AlbumsController(IAlbumsService albumsService) {
            this.albumsService = albumsService;
        }

        [HttpGet]
        [Authorise]
        public IActionResult All() {
            ICollection<Album> albums = this.albumsService.GetAllAlbums(this.Identity.Username);
            AlbumsViewModel avm = new AlbumsViewModel {
                Albums = albums
            };
            this.ViewModel.Data["AlbumsViewModel"] = avm;
            return this.View("all-user-albums");
        }

        [HttpGet]
        [Authorise]
        public IActionResult Details() {
            string albumId = this.Request.QueryData["albumId"].ToString();
            return null; //TODO: Implement the rest of the action.
        }
    }
}
