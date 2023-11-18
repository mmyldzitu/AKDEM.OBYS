using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Common
{
    public class Response<T>:Response, IResponse<T>
    {
        //ilgili datam gelmedi mesela
        public T Data { get; set; }
        public List<CustomValidationError> ValidationErrors { get; set; }
        public Response(ResponseType responseType, string message):base(responseType,message)
        {

        }
        //özellikle success durumunda kullanılacak
        public Response(ResponseType responseType, T data):base(responseType)
        {
            Data = data;
        }

        //Modelde fluent validationdan gelecek bir validation error söz konusu olduğunda
        public Response( T data, List<CustomValidationError> errors):base(ResponseType.ValidationError)
        {
            ValidationErrors = errors;
            Data = data;
        }

    }
}
