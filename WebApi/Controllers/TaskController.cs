using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Abstractions.Services;
using Domain.Commands;
using Domain.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(CreateTaskCommandResult), StatusCodes.Status200OK)]
        public async Task<CreateTaskCommandResult> Create(CreateTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
            }

            var result = await _taskService.CreateTaskCommandHandler(command);

            return  result;
        }
        [HttpGet]
        [ProducesResponseType(typeof(GetAllTasksQueryResult), StatusCodes.Status200OK)]
        [Route("GetAllTask/{id}")]
        public async Task<IActionResult> GetAllTaskByMemberId(Guid memberId)
        {
            var result = await _taskService.GetAllTasksQueryHandler(memberId);

            return Ok(result);
        }
    }
}
