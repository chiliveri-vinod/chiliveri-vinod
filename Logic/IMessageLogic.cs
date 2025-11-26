using CodeChallenge.Api.Models;
using CodeChallenge.Api.Logic;

namespace CodeChallenge.Api.Logic;

public interface IMessageLogic
{
    //Task<Result> CreateMessageAsync(Guid organizationId, CreateMessageRequest request);
    //Task<Result> UpdateMessageAsync(Guid organizationId, Guid id, UpdateMessageRequest request);
    //Task<Result> DeleteMessageAsync(Guid organizationId, Guid id);
    //Task<Message?> GetMessageAsync(Guid organizationId, Guid id);
    //Task<IEnumerable<Message>> GetAllMessagesAsync(Guid organizationId);
    Task<DataResult<Message>> CreateMessageAsync(Guid organizationId, CreateMessageRequest request);
    Task<DataResult<Message>> UpdateMessageAsync(Guid organizationId, Guid id, UpdateMessageRequest request, Message updated);
    Task<Result> DeleteMessageAsync(Guid organizationId, Guid id);
    Task<DataResult<Message>> GetMessageAsync(Guid organizationId, Guid id);
    Task<DataResult<IEnumerable<Message>>> GetAllMessagesAsync(Guid organizationId);
   
}
