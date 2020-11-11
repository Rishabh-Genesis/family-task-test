using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using WebClient.Abstractions;
using WebClient.Shared.Models;
using Domain.Commands;
using System.Threading.Tasks;
using Core.Extensions.ModelConversion;
using Domain.ViewModel;
using Microsoft.AspNetCore.Components;
using Domain.Queries;

namespace WebClient.Services
{
    public class TaskDataService: ITaskDataService
    {
        private readonly HttpClient httpClient;
        public TaskDataService(IHttpClientFactory clientFactory)
        {
            httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            Tasks = new List<TaskModel>();
        }




        public List<TaskModel> Tasks { get; private set; }
        public TaskModel SelectedTask { get; private set; }


        public event EventHandler TasksUpdated;
        public event EventHandler TaskSelected;

        public void SelectTask(Guid id)
        {
            SelectedTask = Tasks.SingleOrDefault(t => t.Id == id);
            TasksUpdated?.Invoke(this, null);
        }

        public void ToggleTask(Guid id)
        {
            foreach (var taskModel in Tasks)
            {
                if (taskModel.Id == id)
                {
                    taskModel.IsComplete = !taskModel.IsComplete;
                }
            }

            TasksUpdated?.Invoke(this, null);
        }

        public async void AddTask(TaskVm model)
        {
            //Tasks.Add(model);
            var result = await Create(ModelConversionExtensions.ToCreateTaskCommand(model));
            if (result != null) {
                TasksUpdated?.Invoke(this, null);
            }
            
        }

        public async Task<CreateTaskCommandResult> Create(CreateTaskCommand command) {
            try
            {
                return await httpClient.PostJsonAsync<CreateTaskCommandResult>("https://localhost:5001/api/Task/Create", command);//Todo : put the url at appropriate place 
            }
            catch (Exception e) {
                return null;
            }
        }

        public async Task<List<TaskVm>> GetAllTask(MemberVm memberVm)
        {
            return  (await httpClient.GetJsonAsync<GetAllTasksQueryResult>("https://localhost:5001/api/Task/GetAll/" + memberVm.Id)).Payload;
        }

        //public async Task<GetAllTasksQueryResult> GetAllTask() { 
        // return await httpClient.PostJsonAsync<CreateTaskCommandResult>("https://localhost:5001/api/Task/GetAll/"+);
        //}
    }
}