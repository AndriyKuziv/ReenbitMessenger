using Microsoft.AspNetCore.Components.Forms;
using ReenbitMessenger.Infrastructure.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Maui.Components.Pages
{
    public partial class UserProfile
    {
        private class UserInfo
        {
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }

        private UserInfo userInfoModel = new UserInfo();
        private string avatarUrl = string.Empty;

        private IBrowserFile? imageFile { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await userProfileService.Initialize();

            await RefreshUserProfile();
        }

        private async Task OnFileChanged(IBrowserFile file)
        {
            imageFile = file;

            var newUrl = await userProfileService.UpdateUserAvatarAsync(imageFile);

            avatarUrl = newUrl is null ? string.Empty : newUrl;
        }

        private async Task DeleteUserAvatar()
        {
            var success = await userProfileService.DeleteUserAvatarAsync();
        }

        private async Task RefreshUserProfile()
        {
            var user = await userProfileService.GetUserProfileAsync();

            if (user is null)
            {
                return;
            }

            userInfoModel.Username = user.UserName;
            userInfoModel.Email = user.Email;

            avatarUrl = user.AvatarUrl;
        }

        private async Task EditUserProfile()
        {
            var user = await userProfileService.EditUserInfoAsync(new EditUserInfoRequest()
            {
                Username = userInfoModel.Username,
                Email = userInfoModel.Email
            });

            userInfoModel.Username = user.UserName;
            userInfoModel.Email = user.Email;
        }
    }
}
