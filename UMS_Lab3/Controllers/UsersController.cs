using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using UMS_Lab3.Domain.Models;
using UMS_Lab3.Persistence;

namespace UMS_Lab3.Controllers;

public class UsersController : ODataController
{
    private readonly postgresContext _context;

    public UsersController(postgresContext context)
    {
        _context = context;
    }

    [EnableQuery]
    public IQueryable<User> Get()
    {
        return _context.Users;
    }


    //[HttpPost]
    //[EnableQuery]
    //public async Task<ActionResult<User>> Post([FromBody] User user)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    // Here you can validate the user data before inserting it into the database

    //    _context.Users.Add(user);
    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction("Get", new { id = user.Id }, user);
    //}

}