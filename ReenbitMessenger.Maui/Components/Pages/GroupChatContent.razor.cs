using MudBlazor;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;

namespace ReenbitMessenger.Maui.Components.Pages
{
    public partial class GroupChatContent
    {
        private async Task OpenAddUserDialog()
        {
            var parameters = new DialogParameters { ["Id"] = ChosenUserId };
            DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };

            var dialog = DialogService.Show<EnterIdDialog>("Enter id of a user to add", options: closeOnEscapeKey, parameters: parameters);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                string userToAddId = Convert.ToString(result.Data);

                await AddUserToChat(userToAddId);
            }
        }

        private async Task OpenRemoveUserDialog()
        {
            var parameters = new DialogParameters { ["Id"] = ChosenUserId };
            DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };

            var dialog = DialogService.Show<EnterIdDialog>("Enter id of a user to delete", options: closeOnEscapeKey, parameters: parameters);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                string userToDeleteId = Convert.ToString(result.Data);

                await RemoveUserFromChat(userToDeleteId);
            }
        }

        private async Task ConnectToGroupChat()
        {
            await chatHubService.ConnectToGroupChatAsync(ChatId);
        }

        private async Task SendMessage()
        {
            await chatHubService.SendMessageAsync(ChatId, new SendMessageToGroupChatRequest() { Text = messageText });
            messageText = string.Empty;
        }

        private async Task AddUserToChat(string userId)
        {
            await chatHubService.AddUsersToGroupChatAsync(ChatId, new AddUsersToGroupChatRequest { UsersIds = new List<string> { userId } });
        }

        private async Task RemoveUserFromChat(string userId)
        {
            await chatHubService.RemoveUsersFromGroupChatAsync(ChatId, new RemoveUsersFromGroupChatRequest { UsersIds = new List<string> { userId } });
        }

        private void OnChatReceived(GroupChat receivedGroupChat)
        {
            InvokeAsync(() =>
            {
                groupChat = receivedGroupChat;

                StateHasChanged();
            });
        }

        private void OnMessageReceived(GroupChatMessage message)
        {
            InvokeAsync(() =>
            {
                if (groupChat != null && groupChat.GroupChatMessages != null)
                {
                    groupChat.GroupChatMessages.Add(message);
                }
                StateHasChanged();
            });
        }

        private void OnMembersReceived(IEnumerable<GroupChatMember> members)
        {
            InvokeAsync(() =>
            {
                if (groupChat != null && groupChat.GroupChatMembers != null)
                {
                    groupChat.GroupChatMembers.AddRange(members);
                }
                StateHasChanged();
            });
        }

        private void OnMembersRemoved(IEnumerable<string> removedMembersIds)
        {
            InvokeAsync(() =>
            {
                if (groupChat != null && groupChat.GroupChatMembers != null)
                {
                    groupChat.GroupChatMembers.RemoveAll(mem => removedMembersIds.Contains(mem.UserId));
                    StateHasChanged();
                }
            });
        }
    }
}
