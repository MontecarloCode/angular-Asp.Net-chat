﻿using ChatClient.Core.Models.Domain;
using ChatClient.Core.Models.ViewModels.Message;
using ChatClient.Core.Repositories;
using ChatClient.Data.Database;
using ChatClient.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatClient.Data.Repositories
{
    public class MessageRecipientRepository : Repository<ChatContext>, IMessageRecipientRepository
    {
        public MessageRecipientRepository(ChatContext context) : base(context) { }

        public async Task<IEnumerable<LatestMessageViewModel>> GetLatestMessages(int userId)
        {
            IQueryable<LatestMessageViewModel> latestMessages = Context.MessageRecipients
                .AsNoTracking()
                .Where(mr =>
                    (mr.RecipientGroupId != null && mr.RecipientGroup.UserId == userId) ||
                    (mr.RecipientUserId != null && (mr.RecipientUserId == userId || mr.Message.AuthorId == userId))
                )
                .GroupByTargetAndGetLatestMessages(userId)
                .OrderByDescending(mr => mr.Message.CreatedAt)
                .Select(mr => new LatestMessageViewModel
                {
                    AuthorId = mr.Message.AuthorId,
                    AuthorName = mr.Message.Author.DisplayName,
                    CreatedAt = mr.Message.CreatedAt,
                    IsRead = mr.IsRead,
                    MessageId = mr.MessageId,
                    MessageRecipientId = mr.MessageRecipientId,
                    TextContent = mr.Message.TextContent,
                    UnreadMessagesCount = mr.RecipientGroupId == null
                        ? mr.RecipientUser.ReceivedPrivateMessages.Count(m => m.IsRead == false)
                        : mr.RecipientGroup.ReceivedGroupMessages.Count(m => m.IsRead == false && mr.RecipientGroup.UserId != userId),
                    RecipientGroup = mr.RecipientGroupId == null ? null : new RecipientGroupViewModel
                    {
                        GroupId = mr.RecipientGroup.GroupId,
                        Name = mr.RecipientGroup.Group.Name,
                    },
                    RecipientUser = mr.RecipientUserId == null ? null : new RecipientUserViewModel
                    {
                        UserId = mr.RecipientUserId == userId
                            ? mr.Message.Author.UserId
                            : mr.RecipientUser.UserId,

                        DisplayName = mr.RecipientUserId == userId
                            ? mr.Message.Author.DisplayName
                            : mr.RecipientUser.DisplayName,
                    }
                });

            return await latestMessages.ToListAsync();
        }

        public async Task<IEnumerable<MessageRecipient>> GetGroupMessages(int userId, int groupId)
        {
            IQueryable<MessageRecipient> groupMessages = Context.MessageRecipients
                .AsNoTracking()
                .Include(mr => mr.RecipientGroup)
                .Include(mr => mr.Message)
                .ThenInclude(m => m.Author)
                .Where(mr =>
                    mr.RecipientGroupId != null &&
                    mr.RecipientGroup.GroupId == groupId &&
                    mr.RecipientGroup.UserId == userId
                );

            return await groupMessages.ToListAsync();
        }

        public async Task<IEnumerable<MessageRecipient>> GetPrivateMessages(int userId, int targetUserId)
        {
            IQueryable<MessageRecipient> privateMessages = Context.MessageRecipients
                .AsNoTracking()
                .Include(mr => mr.Message)
                .ThenInclude(m => m.Author)
                .Where(mr =>
                    (mr.Message.AuthorId == userId && mr.RecipientUserId == targetUserId) ||
                    (mr.Message.AuthorId == targetUserId && mr.RecipientUserId == userId)
                );

            return await privateMessages.ToListAsync();
        }

        public async Task AddGroupMessage(IEnumerable<MessageRecipient> recipients)
        {
            await Context.MessageRecipients.AddRangeAsync(recipients);
        }

        public async Task AddPrivateMessage(MessageRecipient recipient)
        {
            await Context.MessageRecipients.AddAsync(recipient);
        }
    }
}
