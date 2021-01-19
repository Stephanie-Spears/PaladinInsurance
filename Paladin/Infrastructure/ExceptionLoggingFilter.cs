using Paladin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Paladin.Infrastructure
{
    public class ExceptionLoggingFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            //Send ajax response
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        Message = "An error has occured. Please try again later.",
                    }
                };
            }
            filterContext.HttpContext.Response.StatusCode = 500;
            //filterContext.ExceptionHandled = true;

            //Log the error
            var _context = DependencyResolver.Current.GetService<PaladinDbContext>();
            var error = new ErrorLog()
            {
                Message = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                ControllerName = filterContext.Controller.GetType().Name,
                TargetedResult = filterContext.Result.GetType().Name,
                SessionId = (string)filterContext.HttpContext.Request["LoanId"],
                UserAgent = filterContext.HttpContext.Request.UserAgent,
                Timestamp = DateTime.Now
            };
            _context.Errors.Add(error);
            _context.SaveChanges();

            //Send an email notification
            MailMessage email = new MailMessage();
            email.From = new MailAddress("ErrorOccured@Paladin.com");
            email.To.Add(new MailAddress(ConfigurationManager.AppSettings["ErrorEmail"]));
            email.Subject = "An error has occured";
            email.Body = filterContext.Exception.Message + Environment.NewLine
                + filterContext.Exception.StackTrace;
            SmtpClient client = new SmtpClient("localhost");
            client.Send(email);
        }
    }
}