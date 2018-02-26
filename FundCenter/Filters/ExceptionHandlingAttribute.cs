using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FundCenter.Filters
{
    public class ExceptionHandlingAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var caption = "Error";
                var serviceException = filterContext.Exception;
                if (serviceException.GetType() == typeof(NullReferenceException) ||
                    serviceException.GetType() == typeof(InvalidOperationException) ||
                     serviceException.GetType() == typeof(JsonReaderException) ||
                     serviceException.Message.StartsWith("<!DOCTYPE html>"))
                {
                    var message = "Internal Server Error";
                    filterContext.Result = new JsonResult { Data = new { Caption = caption, Message = message, Priority = 2, ReturnCode = 10 }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    filterContext.ExceptionHandled = true;
                    return;
                }
                //else
                //{
                    

                //    filterContext.Result = new JsonResult { Data = new { Caption = caption, Message = serviceException.Message, Priority = 2 }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //    filterContext.ExceptionHandled = true;
                //}
            }
            else
            {
                filterContext.ExceptionHandled = true;
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "Login");
                redirectTargetDictionary.Add("controller", "Account");
                redirectTargetDictionary.Add("system_exception", "unknown");

                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }
        }
    }
}