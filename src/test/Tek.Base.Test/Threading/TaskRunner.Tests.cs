namespace Tek.Base.Test;

public class TaskRunnerTests
{
    [Fact]
    public void RunSync_ShouldReturnResult_WhenTaskCompletesSuccessfully()
    {
        // Arrange
        static async Task<int> action()
        {
            await Task.Delay(10); // Simulate async work
            return 42;
        }

        // Act
        var result = TaskRunner.RunSync(action);

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void RunSync_ShouldThrowException_WhenTaskThrowsException()
    {
        // Arrange
        static async Task<int> action()
        {
            await Task.Delay(10); // Simulate async work
            throw new InvalidOperationException("Test exception");
        }

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => TaskRunner.RunSync(action));
        Assert.Null(ex.InnerException);
        Assert.Equal("Test exception", ex.Message);
    }

    [Fact]
    public void RunSync_ShouldHandleNullTaskAction_Gracefully()
    {
        // Arrange
        Func<Task<int>>? action = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => TaskRunner.RunSync(action));
    }

    [Fact]
    public void RunSync_ShouldWorkWithVoidTask()
    {
        // Arrange
        bool taskCompleted = false;
        async Task<bool> action()
        {
            await Task.Delay(10);
            taskCompleted = true;
            return taskCompleted;
        }

        // Act
        var result = TaskRunner.RunSync(action);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void RunSync_ShouldReturnDefault_WhenTaskReturnsDefault()
    {
        // Arrange
        static async Task<int> action()
        {
            await Task.Delay(10);
            return default;
        }

        // Act
        var result = TaskRunner.RunSync(action);

        // Assert
        Assert.Equal(default, result);
    }
}