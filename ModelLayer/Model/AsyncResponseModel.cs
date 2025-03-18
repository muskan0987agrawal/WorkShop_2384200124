using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{

    public class AsyncResponseModel<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Error { get; set; }

        public AsyncResponseModel(T data, string message, int statusCode, bool success, string token = null)
        {
            Data = data;
            Message = message;
            StatusCode = statusCode;
            Success = success;
            Token = token;
        }
    }

}