using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
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

            return result;
        }

        [HttpGet("GetAllTask/memberId={memberId}")]
        [ProducesResponseType(typeof(GetAllTasksByMemberQueryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTaskByMemberId(Guid? memberId)
        {
            var result = await _taskService.GetAllTasksByMemberQueryHandler((Guid)memberId);

            return Ok(result);
        }

        [HttpGet("GetAllTask")]
        [ProducesResponseType(typeof(GetAllTasksQueryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTask()
        {
            var result = await _taskService.GetAllTasksQueryHandler();

            return Ok(result);
        }

        [HttpPut("UpdateTask/{id}")]
        [ProducesResponseType(typeof(UpdateTaskCommandResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, UpdateTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _taskService.UpdateTaskCommandHandler(command);

                return Ok(result);
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }
        }
    }
}
