using Microsoft.AspNetCore.Mvc;
using UMS_Lab3.Infrastructure.Abstraction.EmailServiceAbstraction;

namespace UMS_Lab3.Controllers;
[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    IEmailService _emailService = null;
    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }
    [HttpPost]
    public bool SendEmail(string EmailToId, string EmailToName, string EmailSubject, string EmailBody)
    {
        return _emailService.SendEmail(EmailToId, EmailToName, EmailSubject, EmailBody);
    }

}