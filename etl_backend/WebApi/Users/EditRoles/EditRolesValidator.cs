using FastEndpoints;
using FluentValidation;

namespace WebApi.Users.EditRoles;

public class EditUserRolesValidator : Validator<EditUserRolesRequest>
{
    public EditUserRolesValidator()
    {
        
    }
}