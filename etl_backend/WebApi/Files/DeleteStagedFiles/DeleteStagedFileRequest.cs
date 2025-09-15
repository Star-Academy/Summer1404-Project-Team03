using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Files;

public class DeleteStagedFileRequest 
{
    [BindFrom("id")]
    [FromRoute]
    public int Id { get; set; }

}