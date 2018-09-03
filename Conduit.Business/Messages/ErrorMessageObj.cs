using System;
using System.Collections.Generic;
using System.Text;
using Conduit.Business.Helpers;

namespace Conduit.Business.Messages
{
    public class ErrorMessageObj
    {
        public ErrorMessageObj()
        {

        }
        public ErrorMessageObj(ErrorMessageCode code,string message)
        {
            Code = code;
            Message = message;
        }
        public ErrorMessageObj(ErrorMessageCode code, string message, bool isValidModel, UnprocessableEntityObjectResult unprocessableEntityObjectResult) :this(code,message)
        {
            Code = code;
            Message = message;
            IsValidModel = isValidModel;
            UnprocessableEntityObjectResult = unprocessableEntityObjectResult;
        }
        public ErrorMessageObj(bool isValidModel, UnprocessableEntityObjectResult unprocessableEntityObjectResult)
        {
            Code = ErrorMessageCode.IsInValidForm;
            Message = "Giá trị của form không họp lệ";
            IsValidModel = isValidModel;
            UnprocessableEntityObjectResult = unprocessableEntityObjectResult;
        }
        public ErrorMessageCode Code { get; set; }
        public string Message { get; set; }
        public bool IsValidModel { get; set; }
        public UnprocessableEntityObjectResult UnprocessableEntityObjectResult { get; set; }
    }
}
