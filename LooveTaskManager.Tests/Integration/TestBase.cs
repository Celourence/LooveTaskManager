using System.Net.Http.Json;
using LooveTaskManager.Application.DTOs;
using LooveTaskManager.Domain.Entities;
using LooveTaskManager.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using TaskStatus = LooveTaskManager.Domain.Enums.TaskStatus;

namespace LooveTaskManager.Tests.Integration;

public abstract class TestBase : IClassFixture<TestFixture>, IAsyncLifetime
{
    protected readonly TestFixture Factory;
    protected readonly HttpClient Client;
    protected Guid CreatedTaskId;

    protected TestBase(TestFixture fixture)
    {
        Factory = fixture;
        Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    public async Task InitializeAsync()
    {
        Factory.ResetDatabase();
        CreatedTaskId = await CreateTestTaskAsync();
    }

    public async Task DisposeAsync()
    {
        await CleanupTestDataAsync();
    }

    protected async Task<Guid> CreateTestTaskAsync(string? title = null)
    {
        var request = new CreateTaskRequestDTO
        {
            Title = title ?? $"Test Task {Guid.NewGuid()}",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Status = (int)TaskStatus.Pending
        };

        var response = await Client.PostAsJsonAsync("/api/v1/task", request);
        var content = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, $"Failed to create test task. Status: {response.StatusCode}, Content: {content}");

        var result = await response.Content.ReadFromJsonAsync<TaskResponseDTO>();
        Assert.NotNull(result);
        return result.Id;
    }

    protected async Task CleanupTestDataAsync()
    {
        if (CreatedTaskId != Guid.Empty)
        {
            var response = await Client.DeleteAsync($"/api/v1/task/{CreatedTaskId}");
            Assert.True(response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }
    }
} 