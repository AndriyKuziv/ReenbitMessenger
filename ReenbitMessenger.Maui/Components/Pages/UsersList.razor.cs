using ReenbitMessenger.Infrastructure.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Maui.Components.Pages
{
    public partial class UsersList
    {
        private class UsersFilterModel
        {
            public int NumberOfUsers { get; set; } = 5;
            public int Page { get; set; } = 0;
            public string ValueContains { get; set; } = "";
            public bool Ascending { get; set; } = true;
            public string OrderBy { get; set; } = "Username";
        }

        private async Task UpdateUsersList()
        {
            users = await httpClient.GetUsersAsync(new GetUsersRequest
            {
                NumberOfUsers = usersFilterModel.NumberOfUsers,
                Page = usersFilterModel.Page,
                ValueContains = usersFilterModel.ValueContains,
                Ascending = usersFilterModel.Ascending,
                OrderBy = usersFilterModel.OrderBy
            });
        }

        private async Task OnValueChanged(int newValue)
        {
            usersFilterModel.NumberOfUsers = newValue;

            await UpdateUsersList();
        }

        private async Task Refresh()
        {
            usersFilterModel.Page = 0;

            await UpdateUsersList();
        }

        private async Task MoveForward()
        {
            usersFilterModel.Page++;
            await UpdateUsersList();
        }

        private async Task MoveBackward()
        {
            usersFilterModel.Page--;
            await UpdateUsersList();
        }
    }
}
