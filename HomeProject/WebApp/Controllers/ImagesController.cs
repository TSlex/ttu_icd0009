using System;
using System.IO;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    /// <summary>
    /// Images - avatar, post, gifts etc...
    /// </summary>
    public class ImagesController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly IWebHostEnvironment _hostEnvironment;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        /// <param name="hostEnvironment"></param>
        public ImagesController(IAppBLL bll, IWebHostEnvironment hostEnvironment)
        {
            _bll = bll;
            _hostEnvironment = hostEnvironment;
            _bll.Images.RootPath = hostEnvironment.WebRootPath;
        }

        /// <summary>
        /// Get image by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{controller}/{id?}")]
        public async Task<IActionResult> GetImage(Guid? id)
        {
            var undefined = base.File(_bll.Images.GetImageUndefinedPath(), "image/jpeg");

            if (id == null)
            {
                return undefined;
            }

            var image = await _bll.Images.FindAsync((Guid) id);

            if (image == null)
            {
                return undefined;
            }

            if (!_bll.Images.ImagePathExists(image.ImageUrl))
            {
                return undefined;
            }

            return base.File(_bll.Images.GetImagePath(image.ImageUrl), "image/jpeg");
        }

        /// <summary>
        /// Get original image version by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{controller}/{id?}/original")]
        public async Task<IActionResult> GetOriginalImage(Guid? id)
        {
            var undefined = base.File(_bll.Images.GetImageUndefinedPath(), "image/jpeg");

            if (id == null)
            {
                return undefined;
            }

            var image = await _bll.Images.FindAsync((Guid) id);

            if (image == null)
            {
                return undefined;
            }
            

            if (!_bll.Images.ImagePathExists(image.OriginalImageUrl))
            {
                return undefined;
            }

            return base.File(_bll.Images.GetImagePath(image.OriginalImageUrl), "image/jpeg");
        }
    }
}