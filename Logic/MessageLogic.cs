using CodeChallenge.Api.Models;
using CodeChallenge.Api.Repositories;

namespace CodeChallenge.Api.Logic
{
    public class MessageLogic : IMessageLogic
    {
        private readonly IMessageRepository _repository;

        public MessageLogic(IMessageRepository repository)
        {
            _repository = repository;
        }

       async Task<DataResult<Message>> IMessageLogic.CreateMessageAsync(Guid organizationId,CreateMessageRequest request)

        {
            // Validation: Title required, length 3–200
            if (string.IsNullOrWhiteSpace(request.Title) ||
                request.Title.Length < 3 || request.Title.Length > 200)
            {
                return DataResult<Message>.Fail("Title must be 3 to 200 characters.");
            }

            // Validation: Content 10–1000
            if (request.Content.Length < 10 || request.Content.Length > 1000)
            {
                return DataResult<Message>.Fail("Content must be 10 to 1000 characters.");
            }

            // Title must be unique per organization
            var existing = await _repository.GetByTitleAsync(organizationId, request.Title);
            if (existing != null)
            {
                return DataResult<Message>.Fail("Title already exists for this organization.");
            }

            // Map request to Message entity
            var message = new Message
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _repository.CreateAsync(message);

            return DataResult<Message>.Ok(message);


        }


      


        async Task<DataResult<Message>> IMessageLogic.UpdateMessageAsync(Guid organizationId, Guid id, UpdateMessageRequest request, Message updated)
        {
            //throw new NotImplementedException();
            var current = await _repository.GetByIdAsync(organizationId, id);
            if (current == null)
                return DataResult<Message>.Fail("Message not found.");

            if (!current.IsActive)
                return DataResult<Message>.Fail("Cannot update inactive message.");

           

            if (string.IsNullOrWhiteSpace(updated.Title) ||
                updated.Title.Length < 3 || updated.Title.Length > 200)
                return DataResult<Message>.Fail("Title must be 3 to 200 characters.");

            if (updated.Content.Length < 10 || updated.Content.Length > 1000)
                return DataResult<Message>.Fail("Content must be 10 to 1000 characters.");

            var existingTitle = await _repository.GetByTitleAsync(current.OrganizationId, updated.Title);
            if (existingTitle != null && existingTitle.Id != id)
                return DataResult<Message>.Fail("Title already exists for this organization.");

            current.Title = updated.Title;
            current.Content = updated.Content;
            current.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(current);

            return DataResult<Message>.Ok(current);
        }

        public async Task<Result> DeleteMessageAsync(Guid organizationId, Guid id)
        {
            var msg = await _repository.GetByIdAsync(organizationId, id);
            if (msg == null)
                return new NotFound("Message not found.");

            if (!msg.IsActive)
                return new Conflict("Cannot delete inactive message.");

            msg.IsActive = false;
            msg.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(msg);

            return new Deleted();
        }

        async Task<DataResult<Message>> IMessageLogic.GetMessageAsync(Guid organizationId, Guid id)
        {
            var message = await _repository.GetByIdAsync(organizationId, id);

            if (message == null)
                return DataResult<Message>.Fail("Message not found.");

            return DataResult<Message>.Ok(message);
        }


        async Task<DataResult<IEnumerable<Message>>> IMessageLogic.GetAllMessagesAsync(Guid organizationId)
        {
            var messages = await _repository.GetAllByOrganizationAsync(organizationId);

            return DataResult<IEnumerable<Message>>.Ok(messages);
        }

        public async Task CreateMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
