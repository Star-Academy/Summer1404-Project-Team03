using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Files;

public class DeleteStagedFileRequest 
{
    [BindFrom("id")]
    [FromRoute]
    public Guid Id { get; set; }
}