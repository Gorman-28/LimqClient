
using LimqClient.Models;
using Microsoft.AspNetCore.Mvc;
using MyNamespace;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace LimqClient.Controllers;

public class SettingsController : Controller
{
    private readonly IClient client;

    public SettingsController(IClient client)
    {
        this.client = client;
    }

    public async Task<IActionResult> ChangeUserName(string newUserName)
    {
        var request = new MyNamespace.ChangeUserNameRequest()
        {
            Id = LimqClient.Settings.SettingArray.MyUser.Id,
            NewUserName = newUserName
        };
        LimqClient.Settings.SettingArray.MyUser.UserName = newUserName;
        Response.Cookies.Append("UserName", $"{newUserName}");
        await client.ChangeUserNameAsync(request);
        return Ok();
    }

    public async Task<IActionResult> ChangeFirstName(string newFirstName)
    {
        var request = new MyNamespace.ChangeFirstNameRequest()
        {
            Id = LimqClient.Settings.SettingArray.MyUser.Id,
            NewFirstName = newFirstName
        };
        LimqClient.Settings.SettingArray.MyUser.FirstName = newFirstName;
        await client.ChangeFirstNameAsync(request);
        return Ok();
    }

    public async Task<IActionResult> ChangeLastName(string newLastName)
    {
        var request = new MyNamespace.ChangeLastNameRequest()
        {
            Id = LimqClient.Settings.SettingArray.MyUser.Id,
            NewLastName = newLastName
        };
        LimqClient.Settings.SettingArray.MyUser.LastName = newLastName;
        await client.ChangeLastNameAsync(request);
        return Ok();
    }

    public async Task<IActionResult> ChangeAvatar(IFormFile newAvatar)
    {
       
        await client.ChangeAvatarAsync(LimqClient.Settings.SettingArray.MyUser.Id, (FileParameter)newAvatar);
        return Ok();
    }

    public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
    {
        var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(oldPassword));
        var hashPasword = Convert.ToBase64String(hash);

        if(hashPasword == Request.Cookies["Password"])
        {
            var md5_ = MD5.Create();
            var hash_ = md5_.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            var hashPasword_ = Convert.ToBase64String(hash_);
            Response.Cookies.Append("Password", $"{hashPasword_}");

            var request = new MyNamespace.ChangePasswordRequest() {
                Id = LimqClient.Settings.SettingArray.MyUser.Id,
                NewPassword = hashPasword_
            };
            await client.ChangePasswordAsync(request);
            return RedirectToAction("Chats", "Menu");
        }


        return Ok();
    }
    public async Task<IActionResult> ChangeTheme(bool theme)
    {
        if (!theme)
        {
            await client.ChangeThemeAsync(Settings.SettingArray.MyUser.Id, theme);
            Settings.SettingArray.MyUser.Theme = theme;
            Response.Cookies.Delete("blackTheme");
        }

        else
        {
            await client.ChangeThemeAsync(Settings.SettingArray.MyUser.Id, theme);
            Settings.SettingArray.MyUser.Theme = theme;
            Response.Cookies.Append("blackTheme", $"{theme}");
        }

        return Ok();
    }
    
    public async Task<IActionResult> Delete()
    {
        Response.Cookies.Delete("UserName");
        Response.Cookies.Delete("Password");
        await client.RemoveUserAsync(LimqClient.Settings.SettingArray.MyUser.Id);
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> LogOut()
    {  
        Response.Cookies.Delete("UserName");
        Response.Cookies.Delete("Password");

        return RedirectToAction("Index", "Home");
    }
    
}


