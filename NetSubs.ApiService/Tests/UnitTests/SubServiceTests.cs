using Moq;
using NetSubs.ApiService.Domain.Entities;
using NetSubs.ApiService.Domain.Service;
using NetSubs.ApiService.Infrastructure.Persistence.Repository;
using Xunit;

namespace NetSubs.ApiService.Tests.UnitTests;

public class SubServiceTests
    {
        private readonly Mock<ISubscribtionRepository> _mockRepo;
        private readonly SubService _service;
        private readonly ILogger<SubService> _logger;
        
        public SubServiceTests()
        {
            _logger = Mock.Of<ILogger<SubService>>();
            _mockRepo = new Mock<ISubscribtionRepository>();
            _service = new SubService(_mockRepo.Object, _logger);
        }
        
        [Fact]
        public async Task GetAllActiveSubscriptionsAsync_ReturnsAllSubscriptions()
        {
            var expectedSubscriptions = new List<Subscription>
            {
                new Subscription { Id = Guid.NewGuid(), IsActive = true },
                new Subscription { Id = Guid.NewGuid(), IsActive = true }
            };
            _mockRepo.Setup(repo => repo.GetAllSubscriptionsAsync()).ReturnsAsync(expectedSubscriptions);
            
            var result = await _service.GetAllActiveSubscriptionsAsync();
            
            Assert.Equal(expectedSubscriptions.Count, result.Count);
            Assert.Equal(expectedSubscriptions.OrderBy(x => x.Id), result.OrderBy(x => x.Id));
        }
        
        [Fact]
        public async Task CreateSubscriptionAsync_CreatesNewSubscription()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var subscriptionTypeId = Guid.Parse("2da0beff-871f-4348-8260-1366a408cf06"); // Предполагаемый тип подписки
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow + TimeSpan.FromDays(30);

            // Act
            var createdSubscription = await _service.CreateSubscriptionAsync(userId, subscriptionTypeId, startDate, endDate);

            // Assert
            Assert.NotNull(createdSubscription);
            Assert.Equal(userId, createdSubscription.UserId);
            Assert.Equal(subscriptionTypeId, createdSubscription.SubscriptionTypeId);
            Assert.Equal(startDate, createdSubscription.StartDate);
            Assert.Equal(endDate, createdSubscription.EndDate);
            Assert.True(createdSubscription.IsActive);
        }
        
        [Fact]
        public async Task DeleteSubscriptionAsync_DeletesExistingSubscription()
        {
            var subscriptionId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.ExistsAsync(subscriptionId)).ReturnsAsync(true);
            
            await _service.DeleteSubscriptionAsync(subscriptionId);
            
            _mockRepo.Verify(repo => repo.RemoveSubscriptionAsync(It.IsAny<Guid>()), Times.Once);
        }
        
        [Fact]
        public async Task DeleteSubscriptionAsync_ThrowsForNonexistentSubscription()
        {
            var nonExistentId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.ExistsAsync(nonExistentId)).ReturnsAsync(false);
            
            await Assert.ThrowsAsync<Exception>(async () => await _service.DeleteSubscriptionAsync(nonExistentId));
        }
        
        [Fact]
        public async Task UpdateSubscriptionAsync_SuccessfulUpdate()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var subGuid = Guid.NewGuid();
            var existingSubscription = new Subscription
            {
                Id = subGuid,
                UserId = userGuid,
                SubscriptionTypeId = Guid.NewGuid(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                IsActive = true
            };

            var updatedSubscription = new Subscription
            {
                Id = subGuid,
                UserId = userGuid,
                SubscriptionTypeId = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddYears(1),
                IsActive = false
            };

            _mockRepo.Setup(r => r.GetSubscriptionByIdAsync(subGuid)).ReturnsAsync(existingSubscription);

            await _service.UpdateSubscriptionAsync(userGuid, subGuid, updatedSubscription);

            _mockRepo.Verify(r => r.UpdateSubscriptionByIdAsync(updatedSubscription), Times.Once);
        }
        
        [Fact]
        public async Task UpdateSubscriptionAsync_FailsIfSubscriptionBelongsToAnotherUser()
        {
            var userGuid = Guid.NewGuid();
            var anotherUserGuid = Guid.NewGuid();
            var subGuid = Guid.NewGuid();
            var existingSubscription = new Subscription
            {
                Id = subGuid,
                UserId = anotherUserGuid,
                SubscriptionTypeId = Guid.NewGuid(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                IsActive = true
            };

            var updatedSubscription = new Subscription
            {
                Id = subGuid,
                UserId = userGuid,
                SubscriptionTypeId = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddYears(1),
                IsActive = false
            };

            _mockRepo.Setup(r => r.GetSubscriptionByIdAsync(subGuid)).ReturnsAsync(existingSubscription);

            await Assert.ThrowsAsync<Exception>(async () => await _service.UpdateSubscriptionAsync(userGuid, subGuid, updatedSubscription));
        }
        
        [Fact]
        public async Task UpdateSubscriptionAsync_ChangeActiveStatus()
        {
            var userGuid = Guid.NewGuid();
            var subGuid = Guid.NewGuid();
            var existingSubscription = new Subscription
            {
                Id = subGuid,
                UserId = userGuid,
                SubscriptionTypeId = Guid.NewGuid(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                IsActive = true
            };

            var updatedSubscription = new Subscription
            {
                Id = subGuid,
                UserId = userGuid,
                SubscriptionTypeId = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddYears(1),
                IsActive = false
            };

            _mockRepo.Setup(r => r.GetSubscriptionByIdAsync(subGuid)).ReturnsAsync(existingSubscription);

            await _service.UpdateSubscriptionAsync(userGuid, subGuid, updatedSubscription);

            _mockRepo.Verify(r => r.UpdateSubscriptionByIdAsync(updatedSubscription), Times.Once);
        }
        
        [Fact]
        public async Task UpdateSubscriptionAsync_KeepOriginalOwner()
        {
            var userGuid = Guid.NewGuid();
            var subGuid = Guid.NewGuid();
            var existingSubscription = new Subscription
            {
                Id = subGuid,
                UserId = userGuid,
                SubscriptionTypeId = Guid.NewGuid(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                IsActive = true
            };

            var updatedSubscription = new Subscription
            {
                Id = subGuid,
                UserId = Guid.NewGuid(),
                SubscriptionTypeId = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddYears(1),
                IsActive = false
            };

            _mockRepo.Setup(r => r.GetSubscriptionByIdAsync(subGuid)).ReturnsAsync(existingSubscription);
            
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateSubscriptionAsync(userGuid, subGuid, updatedSubscription));
        }
    }