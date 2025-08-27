using System.Text;
using etl_backend.Application.DataFile.Services;
using etl_backend.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace etl_backend.Test;

public class LocalFileStorageServiceTests
{
    [Fact]
    public async Task SaveFileAsync_Should_Save_And_Return_RelativePath()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.ContentRootPath.Returns(Path.GetTempPath());

        var settings = Options.Create(new StorageSettings
        {
            Root = "media" ,
            BaseUrl = "http://localhost",
        });

        var storage = new LocalFileStorageService(env, settings);

        var fileName = "test.txt";
        var content = "Hello, world!";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        // Act
        var relativePath = await storage.SaveFileAsync(stream, fileName);

        // Assert
        relativePath.Should().Contain("test.txt");

        var fullPath = Path.Combine(env.ContentRootPath, "media", relativePath);
        File.Exists(fullPath).Should().BeTrue();
        File.ReadAllText(fullPath).Should().Be(content);

        // Cleanup
        File.Delete(fullPath);
    }

    [Fact]
    public async Task DeleteFileAsync_Should_Delete_If_File_Exists()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.ContentRootPath.Returns(Path.GetTempPath());

        var settings = Options.Create(new StorageSettings {
            Root = "media" ,
            BaseUrl = "http://localhost",
        });
        var storage = new LocalFileStorageService(env, settings);

        var fileName = "delete_me.txt";
        var fullPath = Path.Combine(env.ContentRootPath, "media", fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        await File.WriteAllTextAsync(fullPath, "to be deleted");

        File.Exists(fullPath).Should().BeTrue();

        // Act
        await storage.DeleteFileAsync(fileName);

        // Assert
        File.Exists(fullPath).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteFileAsync_Should_Not_Throw_If_File_Not_Exists()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.ContentRootPath.Returns(Path.GetTempPath());

        var settings = Options.Create(new StorageSettings {
            Root = "media" ,
            BaseUrl = "http://localhost",
        });
        var storage = new LocalFileStorageService(env, settings);

        // Act
        Func<Task> act = async () => await storage.DeleteFileAsync("non_existing.txt");

        // Assert
        await act.Should().NotThrowAsync();
    }
}
