// Author:Sourav Kayal
// Date:December 19, 2013
// Source: http://www.c-sharpcorner.com/UploadFile/dacca2/centralize-exception-handling-in-web-api/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace MemberDemo.CustomAttribute
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        //Populate Exception message within constructor
        public ExceptionAttribute()
        {
            this.Mappings = new Dictionary<Type, HttpStatusCode>();
            this.Mappings.Add(typeof(ArgumentNullException), HttpStatusCode.BadRequest);
            this.Mappings.Add(typeof(ArgumentException), HttpStatusCode.BadRequest);
            this.Mappings.Add(typeof(DivideByZeroException), HttpStatusCode.BadRequest);
        }
        public IDictionary<Type, HttpStatusCode> Mappings
        {
            get;
            //Set is private to make it singleton
            private set;
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                var exception = actionExecutedContext.Exception;
                string type = exception.GetType().ToString();
                if (actionExecutedContext != null)
                {
                    // LookUp Mapping Dictionary to get exception type
                    if (this.Mappings.ContainsKey(exception.GetType()))
                    {
                        //Get Status code from Dictionary
                        var httpStatusCode = this.Mappings[exception.GetType()];
                        // Create Message Body with information
                        throw new HttpResponseException(new HttpResponseMessage(httpStatusCode)
                        {
                            Content = new StringContent("Method Access Exception" + actionExecutedContext.Exception.Message),
                            ReasonPhrase = actionExecutedContext.Exception.Message
                        });
                    }
                    else
                    {
                        //Else part executes means there is not information in repository so it is some kind of anonymous exception
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent("System is failure to process request"),
                            ReasonPhrase = "System is failure to process request"
                        });
                    }
                }
            }
        }
    }
}