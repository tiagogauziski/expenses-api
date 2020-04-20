using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Expenses.Application.Common
{
    /// <summary>
    /// Reseponse payload
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class Response<TData>
    {
        public Response(TData data, HttpStatusCode statusCode)
        {
            Data = data;
            StatusCode = statusCode;
        }

        public Response(Error error, HttpStatusCode statusCode)
        {
            Error = error;
            StatusCode = statusCode;
        }

        public TData Data { get; private set; }

        public Error Error { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public bool Successful { get => Error == null;  }
    }
}
