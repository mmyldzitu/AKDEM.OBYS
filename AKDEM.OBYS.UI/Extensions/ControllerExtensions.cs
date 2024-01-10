using AKDEM.OBYS.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Extensions
{
    public static class ControllerExtensions
    {
      


        public static IActionResult ResponseRedirectAction<T>(this Controller controller, IResponse<T> response, string actionName, string controllerName = "",int parameter=0)
        {

            if (response.ResponseType == ResponseType.NotFound)
            {
                return controller.NotFound();
            }
            if (response.ResponseType == ResponseType.ValidationError)
            {
                foreach (var item in response.ValidationErrors)
                {
                    controller.ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return controller.View(response.Data);
            }
            if (string.IsNullOrWhiteSpace(controllerName) && parameter==0)
            {
                return controller.RedirectToAction(actionName);

            }
            else if(!string.IsNullOrWhiteSpace(controllerName) && parameter == 0)
            {
                return controller.RedirectToAction(actionName, controllerName);
            }
            else if (!string.IsNullOrWhiteSpace(controllerName) && parameter == 1)
            {
                return controller.RedirectToAction(actionName, controllerName, new { classType = parameter });
            }
            else
            {
                return controller.RedirectToAction(actionName, new { classType = parameter });
            }
        }







        //örneğin getbyıd
        public static IActionResult ResponseView<T>(this Controller contoller, IResponse<T> response)
        {
            if (response.ResponseType == ResponseType.NotFound)
            {
                return contoller.NotFound();
            }
            return contoller.View(response.Data);
        }
        //örneğin remove
        public static IActionResult ResponseRedirectAction(this Controller controller, IResponse response, string actionName)
        {
            if (response.ResponseType == ResponseType.NotFound)
            {
                return controller.NotFound();
            }
            return controller.RedirectToAction(actionName);

        }
    }
    }

