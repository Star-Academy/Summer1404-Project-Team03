namespace Application.Tests.Common;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authorization;
using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Services.Abstractions;
using MediatR;

public class AuthorizationBehaviorTests
{
    private readonly ICurrentUserService _currentUser;
    private readonly RequestHandlerDelegate<string> _next;
    private readonly AuthorizationBehavior<TestRequest, string> _sut;
    

    public AuthorizationBehaviorTests()
    {
        _currentUser = Substitute.For<ICurrentUserService>();
        _next = Substitute.For<RequestHandlerDelegate<string>>();
        _next.Invoke().Returns("SUCCESS");
        _sut = new AuthorizationBehavior<TestRequest, string>(_currentUser);
        
    }

    [Fact]
    public async Task Handle_Should_Call_Next_When_No_AuthorizationAttribute()
    {
        var requestWithNoAuthorization = new TestRequest();

        var result = await _sut.Handle(requestWithNoAuthorization, _next, CancellationToken.None);

        result.Should().Be("SUCCESS");
        await _next.Received(1).Invoke();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_User_Not_Authenticated()
    {
        var request = new RequiresAdminRequest();
        _currentUser.IsAuthenticated.Returns(false);

        var act = async () => await _sut.Handle((TestRequest)(object)request, _next, CancellationToken.None);


        await act.Should().ThrowAsync<ForbiddenException>()
            .WithMessage("User is not authenticated.");
        await _next.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_User_Does_Not_Have_Required_Role()
    {
        var request = new RequiresAdminRequest();
        _currentUser.IsAuthenticated.Returns(true);
        _currentUser.Roles.Returns(new[] { "Operator" });


        var act = async () => await _sut.Handle((TestRequest)(object)request, _next, CancellationToken.None);


        await act.Should().ThrowAsync<ForbiddenException>()
            .WithMessage("User is not authorized. Required roles: Operator");
        await _next.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task Handle_Should_Call_Next_When_User_Has_Required_Role()
    {
        var request = new RequiresAdminRequest();
        _currentUser.IsAuthenticated.Returns(true);
        _currentUser.Roles.Returns(new[] { "Admin", "Operator" });


        var result = await _sut.Handle((TestRequest)(object)request, _next, CancellationToken.None);


        result.Should().Be("SUCCESS");
        await _next.Received(1).Invoke();
    }


    public class TestRequest : IRequest<string>
    {
    }

    [RequireRole("Admin")]
    public class RequiresAdminRequest : TestRequest
    {
    }
}