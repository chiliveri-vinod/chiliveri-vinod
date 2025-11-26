using CodeChallenge.Api.Logic;
using CodeChallenge.Api.Models;
using CodeChallenge.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Api.Controllers;

[ApiController]
[Route("api/v1/organizations/{organizationId}/messages")]
public class MessagesController : ControllerBase
{
    private readonly IMessageRepository _repository;
    private readonly ILogger<MessagesController> _logger;
     private readonly IMessageLogic _logic;
    public MessagesController(IMessageRepository repository, ILogger<MessagesController> logger, IMessageLogic logic)
    {
        _repository = repository;
        _logger = logger;
        _logic = logic;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Message>>> GetAll(Guid organizationId)
    {
        // TODO: Implement
        //throw new NotImplementedException();
        var messages = await _repository.GetAllByOrganizationAsync(organizationId);
        return Ok(messages);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Message>> GetById(Guid organizationId, Guid id)
    {
        // TODO: Implement
        //throw new NotImplementedException();
        var message = await _repository.GetByIdAsync(organizationId, id);

        if (message == null)
            return NotFound();

        return Ok(message);
    }

    [HttpPost]
    public async Task<ActionResult<Message>> Create(Guid organizationId, [FromBody] CreateMessageRequest request)
    {
        // TODO: Implement
        //throw new NotImplementedException();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newMessage = new Message
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(newMessage);

        return CreatedAtAction(nameof(GetById),
            new { organizationId = organizationId, id = newMessage.Id },
            newMessage
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid organizationId, Guid id, [FromBody] UpdateMessageRequest request)
    {
        // TODO: Implement
        // throw new NotImplementedException();
        var existing = await _repository.GetByIdAsync(organizationId, id);

        if (existing == null)
            return NotFound();

        existing.Title = request.Title;
        existing.Content = request.Content;
        existing.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(existing);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid organizationId, Guid id)
    {
        // TODO: Implement
        //throw new NotImplementedException();
        var existing = await _repository.GetByIdAsync(organizationId, id);

        if (existing == null)
            return NotFound();

        var deleted = await _repository.DeleteAsync(organizationId, id);

        if (!deleted)
            return BadRequest("Failed to delete message.");

        return NoContent();
    }




    //task2 controller updated MessageLogic

    [HttpPost]
    public async Task<IActionResult> CreateLogic(Guid organizationId, [FromBody] CreateMessageRequest request)
    {
        var result = await _logic.CreateMessageAsync(organizationId, request);

        return result.Success
            ? Ok(result.Data)
            : BadRequest(result.Error);
    }


    [HttpPut("{id}")] 
    public async Task<IActionResult> UpdateLogic(Guid organizationId, Guid id, UpdateMessageRequest request, Message updated) 
    { 
        var result = await _logic.UpdateMessageAsync(organizationId, id, request, updated); 
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }

    

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid organizationId, Guid id)
    {
        var result = await _logic.GetMessageAsync(organizationId, id);
        return result.Success ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLogic(Guid organizationId)
    {
        var result = await _logic.GetAllMessagesAsync(organizationId);
        return Ok(result.Data);
    }

}
