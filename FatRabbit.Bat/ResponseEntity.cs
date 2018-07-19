using System;
using System.Collections.Generic;
using System.Text;

namespace FatRabbit.Bat
{
    public class ResponseEntity<T>:BaseResponse
    {
        public T Body { set; get; }

    }
}
