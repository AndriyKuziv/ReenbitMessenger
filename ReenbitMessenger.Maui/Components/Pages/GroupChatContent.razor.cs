using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using MudBlazor;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using ReenbitMessenger.Maui.Clients;
using ReenbitMessenger.Maui.Components.Utils;

namespace ReenbitMessenger.Maui.Components.Pages
{
    public partial class GroupChatContent
    {
        private int Page = 0;
        private GroupChat groupChat { get; set; } = new GroupChat();
        private string messageText { get; set; } = string.Empty;

        public string ChosenUserId { get; set; } = string.Empty;

        public bool IsUsed = true;

        private IJSObjectReference? module;

        private async Task Setup()
        {
            await chatHubService.SubscribeAsync<GroupChatMessage>("ReceiveMessage", OnMessageReceived);
            await chatHubService.SubscribeAsync<IEnumerable<GroupChatMember>>("ReceiveMembers", OnMembersReceived);
            await chatHubService.SubscribeAsync<GroupChat>("ReceiveFullGroupChat", OnChatReceived);
            await chatHubService.SubscribeAsync<IEnumerable<string>>("RemoveMembers", OnMembersRemoved);
            await chatHubService.SubscribeAsync<IEnumerable<GroupChatMessage>>("ReceiveGroupChatMessages", OnMessageListReceived);
            await chatHubService.SubscribeAsync<GroupChatMessage>("DeleteMessage", OnMessageDeleted);

            await chatHubService.StartAsync();
        }

        private async Task OpenAddUserDialog()
        {
            var parameters = new DialogParameters { ["Value"] = ChosenUserId };
            DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };

            var dialog = DialogService.Show<EnterValueDialog>("Enter id of a user to add", options: closeOnEscapeKey, parameters: parameters);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                string userToAddId = Convert.ToString(result.Data);

                await AddUserToChat(userToAddId);
            }
        }

        private async Task OpenRemoveUserDialog()
        {
            var parameters = new DialogParameters { ["Value"] = ChosenUserId };
            DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };

            var dialog = DialogService.Show<EnterValueDialog>("Enter id of a user to delete", options: closeOnEscapeKey, parameters: parameters);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                string userToDeleteId = Convert.ToString(result.Data);

                await RemoveUserFromChat(userToDeleteId);
            }
        }

        private async Task OpenLeaveChatDialog()
        {
            DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
            var dialog = DialogService.Show<ConfirmDialog>("Are you sure you want to leave this group chat?", options: closeOnEscapeKey);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await httpClient.LeaveGroupChatAsync(ChatId);
                navManager.NavigateTo("/chatsList", true);
            }
        }

        private async Task OpenEditChatMessageDialog(long messageId, string messageText)
        {
            var parameters = new DialogParameters { ["Value"] = messageText };
            DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
            var dialog = DialogService.Show<EnterValueDialog>("Edit this message", options: closeOnEscapeKey, parameters: parameters);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                
            }
        }

        private async Task OpenDeleteChatMessageDialog()
        {
            DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
            var dialog = DialogService.Show<EnterValueDialog>("Enter id of a message to delete", options: closeOnEscapeKey);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                if(long.TryParse(Convert.ToString(result.Data), out long messageId))
                {
                    await chatHubService.DeleteMessageAsync(ChatId, new DeleteMessageFromGroupChatRequest { MessageId = messageId });
                }
            }
        }

        private async Task ConnectToGroupChat()
        {
            await chatHubService.ConnectToGroupChatAsync(ChatId);
        }

        private async Task GetFullGroupChat()
        {
            await chatHubService.GetGroupChatInfoAsync(ChatId);
            await chatHubService.GetGroupChatMessages(ChatId);
        }

        [JSInvokable]
        public async Task GetGroupChatMessages()
        {
            await chatHubService.GetGroupChatMessages(ChatId, Page);
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

        private void OnMessageListReceived(IEnumerable<GroupChatMessage> groupChatMessages)
        {
            InvokeAsync(() =>
            {
                if(groupChat is null || groupChatMessages.IsNullOrEmpty())
                {
                    StopInfiniteScrolling();
                    return;
                }

                if (groupChat.GroupChatMessages is null)
                {
                    groupChat.GroupChatMessages = groupChatMessages.ToList();
                }
                else
                {
                    groupChat.GroupChatMessages.InsertRange(0, groupChatMessages);
                }
                Page++;

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

        private void OnMessageDeleted(GroupChatMessage message)
        {
            InvokeAsync(() =>
            {
                if (groupChat != null && groupChat.GroupChatMessages != null)
                {
                    groupChat.GroupChatMessages.RemoveAll(msg => msg.Id == message.Id);
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

        private async Task AddToClipboard(string value)
        {
            await Js.InvokeVoidAsync("clipboardCopy.copyText", value);
        }

        private async Task OnScrolled()
        {
            await module.InvokeVoidAsync("checkLastElement", DotNetObjectReference.Create(this));
        }

        private void StopInfiniteScrolling()
        {
            IsUsed = false;
        }

        public async ValueTask DisposeAsync()
        {
            await chatHubService.UnsubscribeAllAsync();

            chatHubService.Dispose();
        }
    }
}
