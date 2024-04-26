using ReenbitMessenger.Infrastructure.Models.Requests;

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
                NumberOfUsers = filterModel.NumberOfUsers,
                Page = filterModel.Page,
                ValueContains = filterModel.ValueContains,
                Ascending = filterModel.Ascending,
                OrderBy = filterModel.OrderBy
            });
        }

        private async Task Refresh()
        {
            filterModel.Page = 0;

            await UpdateUsersList();
        }

        private async Task OnValueChanged(int newValue)
        {
            filterModel.NumberOfUsers = newValue;

            await UpdateUsersList();
        }

        private async Task MoveForward()
        {
            filterModel.Page++;
            await UpdateUsersList();
        }

        private async Task MoveBackward()
        {
            filterModel.Page--;
            await UpdateUsersList();
        }
    }
}
