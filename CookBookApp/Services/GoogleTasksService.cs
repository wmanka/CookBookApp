using Google.Apis.Auth.OAuth2;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CookBookApp.Services.Interfaces;
using System.Linq.Expressions;

namespace CookBookApp.Services
{
    public class GoogleTasksService : IGoogleTasksService
    {
        readonly TasksService taskService;

        public GoogleTasksService()
        {
            string[] scopes = { TasksService.Scope.Tasks };
            string applicationName = "CookBookApp";
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None).Result;
            }

            taskService = new TasksService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });
        }

        IEnumerable<TaskList> IGoogleTasksService.GetTaskLists()
        {
            var listRequest = taskService.Tasklists.List();
            var taskLists = listRequest.Execute().Items;

            return taskLists;
        }
    }
}
