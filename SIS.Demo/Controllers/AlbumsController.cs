﻿using SIS.Demo.Models;
using SIS.Demo.Services.Contracts;
using SIS.Demo.ViewModels;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Controllers.Base;
using SIS.HTTP.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Album album = this.albumsService.GetAlbumById(albumId);
            if (album == null) {
                this.ViewModel.Data["Error"] = "No such album";
                return this.View();
            }
            else {
                AlbumDetailsViewModel albumDetailsViewModel = new AlbumDetailsViewModel {
                    Id = album.Id,
                    Name = album.Name,
                    Price = Math.Round(album.Price, 2),
                    ImageSource = album.Cover,
                    Tracks = album.Tracks.Select(at => new TrackViewModel {
                        Name = at.Track.Name,
                        AlbumId = album.Id,
                        Id = at.TrackId,
                        Price = Math.Round(at.Track.Price, 2)
                    })
                };
                this.ViewModel.Data["AlbumDetailsViewModel"] = albumDetailsViewModel;
                return this.View();
            }
        }

        public IActionResult Create() => this.View();

        [HttpPost]
        [Authorise]
        public IActionResult Create(InputAlbumModel model) {
            Album album = new Album {
                Name = model.Name,
                Cover = model.Cover,
            };
            this.albumsService.AddAlbumToDb(album, this.Identity.Username);
            return this.RedirectToAction("/albums/all");
        }
    }
}

