using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using AuthenticationService.Persistence;
using AuthenticationService.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationService.Controllers;

public class UsersController : ODataController
{
    private readonly authContext _context;

    public UsersController(authContext context)
    {
        _context = context;
    }

    [EnableQuery]
    [Authorize(Policy = "RequireAdminRole")]
    public IQueryable<User> Get()
    {
        return _context.Users;
    }



}

