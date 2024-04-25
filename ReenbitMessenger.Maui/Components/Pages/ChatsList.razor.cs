using MudBlazor;
using ReenbitMessenger.Infrastructure.Models.Requests;

namespace ReenbitMessenger.Maui.Components.Pages
{
    public partial class ChatsList
    {
        protected async Task UpdateChatsList()
        {
            groupChats = await httpClient.GetUserGroupChatsAsync();
        }

        protected async Task CreateGroupChat()
        {
            if (!string.IsNullOrEmpty(newGroupChatName))
            {
                await httpClient.CreateGroupChatAsync(new CreateGroupChatRequest { Name = newGroupChatName });
            }

            newGroupChatName = string.Empty;
            await UpdateChatsList();
        }

        protected async Task DeleteGroupChat(string groupChatId)
        {
            await httpClient.DeleteGroupChatAsync(groupChatId);
            await UpdateChatsList();
        }

        protected async Task OnGroupClicked(Guid chatId)
        {
            navManager.NavigateTo($"/groupChat/{Convert.ToString(chatId)}", true);
        }

        protected async Task OpenDeleteGroupChatDialog()
        {
            var parameters = new DialogParameters { ["Id"] = groupChatId };
            DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };

            var dialog = DialogService.Show<EnterIdDialog>("Enter id of a group chat to delete", options: closeOnEscapeKey, parameters: parameters);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                string groupChatToDeleteId = Convert.ToString(result.Data);

                await DeleteGroupChat(groupChatToDeleteId);
            }
        }
    }
}
