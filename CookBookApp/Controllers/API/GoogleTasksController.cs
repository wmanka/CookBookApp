using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Util.Store;
using System.Reflection;
using CookBookApp.Services;
using CookBookApp.Services.Interfaces;

namespace CookBookApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleTasksController : ControllerBase
    {
        private IGoogleTasksService Service;

        public GoogleTasksController(IGoogleTasksService service) => Service = service;

        public void Index()
        {
            
        }
    }
}