﻿using AutoMapper;
using Core.Application.Database;
using Core.Application.Requests.GroupMemberships.Commands;
using Core.Application.Services;
using Core.Domain.Entities;
using Core.Domain.Resources.GroupMemberships;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Core.Application.Test.Requests.GroupMemberships.Commands
{
    public class CreateMembershipCommandTests
    {
        private readonly IMapper _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IDateProvider> _dateProviderMock;

        public CreateMembershipCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _dateProviderMock = new Mock<IDateProvider>();

            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMap<GroupMembership, GroupMembershipResource>();
            });

            _mapperMock = mapperConfiguration.CreateMapper();
        }

        [Fact]
        public async Task CreateMembershipCommandHandler_ShouldCreateMembership_AndReturnCreatedMembership()
        {
            // Arrange
            CreateMembershipCommand request = new CreateMembershipCommand
            {
                GroupId = 1,
                UserId = 1,
                IsAdmin = true
            };

            IEnumerable<User> expectedUsers = new[]
            {
                new User
                {
                    UserId = 1,
                    UserName = "alfred_miller"
                }
            };

            IQueryable<User> queryableMock = expectedUsers
                .AsQueryable()
                .BuildMock()
                .Object;

            _unitOfWorkMock
                .Setup(m => m.GroupMemberships.Add(It.IsAny<GroupMembership>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(m => m.Recipients.Add(It.IsAny<Recipient>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(m => m.CommitAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(2);

            _unitOfWorkMock
                .Setup(m => m.Users.GetById(It.IsAny<int>()))
                .Returns(queryableMock);

            

            CreateMembershipCommand.Handler handler = new CreateMembershipCommand.Handler(_dateProviderMock.Object, _unitOfWorkMock.Object, _mapperMock);

            // Act
            GroupMembershipResource membership = await handler.Handle(request);

            // Assert
            Assert.NotNull(membership);

            _unitOfWorkMock
                .Verify(m => m.GroupMemberships.Add(It.IsAny<GroupMembership>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);

            _unitOfWorkMock
                .Verify(m => m.Recipients.Add(It.IsAny<Recipient>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);

            _unitOfWorkMock
                .Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}
