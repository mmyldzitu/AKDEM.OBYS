﻿using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Dto.AppAccountDtos;
using AKDEM.OBYS.UI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IAppSessionService _appSessionService;
        private readonly IValidator<UpdatePasswordModel> _updatePasswordModelValidator;
        public AccountController(IAppUserService appUserService, IAppSessionService appSessionService, IValidator<UpdatePasswordModel> updatePasswordModelValidator)
        {
            _appUserService = appUserService;
            _appSessionService = appSessionService;
            _updatePasswordModelValidator = updatePasswordModelValidator;
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(AppUserLoginDto dto)
        {
            int sessionId = await _appSessionService.GetActiveSessionId();
            var result = await _appUserService.CheckUserAsync(dto);
            if (result.ResponseType == Common.ResponseType.Success)
            {
                var roleResult = await _appUserService.GetRolesByUserIdAsync(result.Data.Id);
                var claims = new List<Claim>();
                if (roleResult.ResponseType == Common.ResponseType.Success)
                {
                    foreach (var role in roleResult.Data)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Definition)
                        );
                    }
                }

                claims.Add(new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString()));
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {

                    IsPersistent = true,

                };


                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                


                // "Admin" rolüne sahip olup olmadığını kontrol et
                if (claims[0].Value=="Admin")
                {
                    return RedirectToAction("Index", "Session");
                }
                else if(claims[0].Value == "Teacher")
                {
                    return RedirectToAction("Index", "Teacher", new { sessionId=sessionId});
                }
                else
                {
                    return RedirectToAction("Index", "Student", new {sessionId=sessionId });

                }
            }
                ModelState.AddModelError("", result.Message);

                return View(dto);


            
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult UpdatePassword(int userId)
        {
            
            return View(new UpdatePasswordModel { UserId=userId});
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel model)
        {
            var result = await _updatePasswordModelValidator.ValidateAsync(model);
            if (result.IsValid)
            {
                await _appUserService.UserPasswordUpdate(model.UserId, model.NewPassword);
                return RedirectToAction("LogOut", "Account");
            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(model);
        }
        
    }
}
