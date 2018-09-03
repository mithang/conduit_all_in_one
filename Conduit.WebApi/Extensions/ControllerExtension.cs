using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Business.Helpers;
using Conduit.Business.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.WebApi.Extensions
{
    public static class ControllerExtension
    {
        
        public static ResultMessage<T> BadRequestOrderByExtention<T>(this Controller controller, string nameOrderBy) where T:class
        {
            ResultMessage<T> resultMessage=new ResultMessage<T>();
            resultMessage.Errors = new ErrorMessageObj(ErrorMessageCode.NotFoundNameOrderBy,
                $"Không tìm thấy {nameOrderBy} để sắp xếp !");
            return resultMessage;
        }
        public static ResultMessage<T> BadRequestNotFindFieldExtention<T>(this Controller controller) where T : class
        {
            ResultMessage<T> resultMessage = new ResultMessage<T>();
            resultMessage.Errors =
                new ErrorMessageObj(ErrorMessageCode.NotFoundNameField, "Không tìm thấy fields để lấy ra !");
            return resultMessage;
        }
        public static ResultMessage<T> BadRequestInvalidFormExtention<T>(this Controller controller, UnprocessableEntityObjectResult unprocessableEntityObjectResult) where T : class
        {
            ResultMessage<T> resultMessage = new ResultMessage<T>();
            resultMessage.Errors =
                new ErrorMessageObj(ErrorMessageCode.IsInValidForm, "Giá trị nhập vào không họp lệ !");
            return resultMessage;
        }
        public static ResultMessage<T> BadRequestDefaultExtention<T>(this Controller controller) where T : class
        {
            ResultMessage<T> resultMessage = new ResultMessage<T>();
            resultMessage.Errors =
                new ErrorMessageObj(ErrorMessageCode.BadRequestDefault, "Đã có lỗi trong quá trình xử lí !");
            return resultMessage;
        }
        public static ResultMessage<T> BadRequestNotDataExtention<T>(this Controller controller, dynamic id) where T : class
        {
            ResultMessage<T> resultMessage = new ResultMessage<T>();
            resultMessage.Errors =
                new ErrorMessageObj(ErrorMessageCode.BadRequestDefault, $"Không tìm thấy dữ liệu với tham số {id} !");
            return resultMessage;
        }
        public static ResultMessage<T> OkDefaultExtention<T>(this Controller controller, dynamic data) where T : class
        {
            ResultMessage<T> resultMessage = new ResultMessage<T>();
            resultMessage.Errors = new ErrorMessageObj();
            resultMessage.Result = data;
            return resultMessage;
        }
        public static ResultMessage<T> OkAndErrorExtention<T>(this Controller controller, ErrorMessageObj error, dynamic data) where T : class
        {
            ResultMessage<T> resultMessage = new ResultMessage<T>();
            resultMessage.Errors = error;
            resultMessage.Result = data;
            return resultMessage;
        }
    }
}
