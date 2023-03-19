using ImgurNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using WeConnect.ViewModels;
using WeConnect.Models;

namespace WeConnect.Controllers;

public class EditController : Controller
{
    UserService _userService;
    PhotoService _photoService;
    public EditController(UserService userService, PhotoService photoService)
    {
        _userService = userService;
        _photoService = photoService;
    }

    public IActionResult Profile()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var user = _userService.GetUserById(userId);
        return View(UserToMyViewModel(user));
    }

    private MyViewModel UserToMyViewModel(User user)
    {
        return new MyViewModel
        {
            Id = user.ID,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.PassWord,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            AboutMe = user.AboutMe,
            ProfilePhoto = user.ProfilePhoto ?? new Photo()
        };
    }

    [HttpPost]
    public async Task<IActionResult> UploadProfilePhoto(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            try
            {
                string clientId = "c13c4448d07f4a7";
                var client = new RestClient("https://api.imgur.com/3");
                var request = new RestRequest("image", Method.Post);
                request.AddHeader("Authorization", $"Client-ID {clientId}");

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();

                request.AddFile("image", fileBytes, file.FileName);

                var response = await client.ExecuteAsync(request);
                var content = JsonConvert.DeserializeObject<ImgurNet.Models.Image>(response.Content);


                if (response.IsSuccessful)
                {
                    var imageUrl = content.Link;

                    // Save the image URL to the database
                    // ...
                    var userId = HttpContext.Session.GetInt32("UserId");
                    var user = _userService.GetUserById(userId);
                    _photoService.UpdateProfilePhoto(user, imageUrl);

                    return RedirectToAction("Profile", "Edit");
                }
                else
                {
                    throw new Exception(response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                // Handle the error
                // ...
                return RedirectToAction("Profile", "Edit");
            }
        }
        return RedirectToAction("Profile", "Edit");
    }
}
