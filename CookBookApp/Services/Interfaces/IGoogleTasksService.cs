using Google.Apis.Tasks.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CookBookApp.Services.Interfaces
{
    public interface IGoogleTasksService
    {
        IEnumerable<TaskList> GetTaskLists();
        //TaskList GetTaskList(Expression<Func<TaskList, bool>> predicate);
        //void AddTaskList(TaskList taskList);
        //void DeleteTaskList(TaskList taskList);

        //IEnumerable<Task> GetTasks(TaskList taskList);
        //Task GetTask(TaskList taskList, Expression<Func<Tasks, bool>> predicate);
        //void AddTask(TaskList taskList, Task task);
        //void DeleteTask(TaskList taskList, Task task);
    }
}
