using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
   public class ResponseModel<T>
    {
        public T Data { get; set; }
        public string Message {  get; set; }

        public int StatusCode {  get; set; }

        public bool Success { get; set; }

        public string Token { get; set; }
    }
}
