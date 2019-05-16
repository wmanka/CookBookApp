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
        private readonly TasksService taskService;

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


        public void AddTask(TaskList taskList, Task task)
        {
            taskService.Tasks.Insert(task, taskList.Id).Execute();
        }

        public void AddTaskList(string title)
        {
            TaskList taskList = new TaskList { Title = "Shopping List - " + title };
            taskService.Tasklists.Insert(taskList).Execute();
        }

        public void DeleteTask(TaskList taskList, Task task)
        {
            taskService.Tasks.Delete(taskList.Id, task.Id);
        }

        public void DeleteTaskList(TaskList taskList)
        {
            taskService.Tasklists.Delete(taskList.Id).Execute();
        }

        public Task GetTask(TaskList taskList, Func<Task, bool> predicate)
        {
            var item = taskService.Tasks.List(taskList.Id).Execute().Items.FirstOrDefault(predicate);

            return item;
        }

        public TaskList GetTaskList(Func<TaskList, bool> predicate)
        {
            var listRequest = taskService.Tasklists.List();
            var taskLists = listRequest.Execute().Items;

            var list = taskLists.FirstOrDefault(predicate);

            return list;
        }

        public TaskList GetTaskList(string title)
        {
            var listRequest = taskService.Tasklists.List();
            var taskLists = listRequest.Execute().Items;

            var list = taskLists.FirstOrDefault(tl => tl.Title == title);

            return list;
        }

        public IEnumerable<Task> GetTasks(TaskList taskList)
        {
            var items = taskService.Tasks.List(taskList.Id).Execute().Items;

            return items;
        }

        IEnumerable<TaskList> IGoogleTasksService.GetTaskLists()
        {
            var listRequest = taskService.Tasklists.List();
            var taskLists = listRequest.Execute().Items;

            return taskLists;
        }
    }
}
