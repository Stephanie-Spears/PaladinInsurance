using Paladin.Models;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Web.Mvc;

namespace Paladin.Infrastructure
{
	/* Inherits from the filter attribute class, and also implements the IExceptionFilter interface */
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
                        Message = "An error has occurred. Please try again later.",
                    }
                };
            }
            filterContext.HttpContext.Response.StatusCode = 500;
	        /* This line serves multiple purposes.
	         * First it will prevent our application error fallback code from running in addition to our filter, since the exception is already marked as handled. We do want to provide global application error code to catch any errors that aren't handled in this ExceptionFilter scope, but we don't want both to run, so we can manage that with this flag.
	         * Secondly, it's a way of communicating between multiple Exception Filters. 
	         */
			filterContext.ExceptionHandled = true;

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
            email.Subject = "An error has occurred";
            email.Body = filterContext.Exception.Message + Environment.NewLine
                + filterContext.Exception.StackTrace;
            SmtpClient client = new SmtpClient("localhost");
            client.Send(email);
        }
    }
}