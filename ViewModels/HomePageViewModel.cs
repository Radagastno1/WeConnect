using Microsoft.AspNetCore.Mvc.Rendering;

namespace WeConnect.ViewModels;
[Serializable]
public class HomePageViewModel
{
    public int Id{get; set;}
    public string FirstName{get;set;}
    public string LastName{get;set;}
    public string Email{get;set;}
    public string password{get;set;}
}