using FundCenter.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FundCenter.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizationFilter : FilterAttribute, IAuthorizationFilter
    {
        private readonly ICache _cache;

        public AuthorizationFilter(ICache cache)
        {
            _cache = cache;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
             || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            
            if (!skipAuthorization)
            {
                if (!CheckValidation())
                {
                    string sessionCookie = filterContext.HttpContext.Request.Headers["Cookie"];

                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest() == true)
                        {
                            AjaxRequestLogout(filterContext);
                        }
                        else
                        {
                            RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                            redirectTargetDictionary.Add("action", "Login");
                            redirectTargetDictionary.Add("controller", "Account");
                            //redirectTargetDictionary.Add("timeout", "true");

                            filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
                        }
                    }
                    else
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest() == true)
                        {
                            AjaxRequestLogout(filterContext);
                        }
                        else
                        {
                            RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                            redirectTargetDictionary.Add("action", "Login");
                            redirectTargetDictionary.Add("controller", "Account");

                            filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
                        }
                    }
                }
            }

        }

        private void AjaxRequestLogout(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            JsonResult jsonResult = new JsonResult();

            var caption = "Error";
            var message = "Session Timeout";
            jsonResult.Data = new
            {
                Priority = 1,
                Caption = caption,
                Message = message,
            };

            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            filterContext.Result = jsonResult;
        }

        private bool CheckValidation()
        {
            if (HttpContext.Current.Session.IsNewSession)
            {
                return false;
            }

            var user = _cache.Get<string>(SessionConstant.USER);
            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}