using MudBlazor;
using ReenbitMessenger.Infrastructure.Models.DTO;
using ReenbitMessenger.Infrastructure.Models.Requests;
using ReenbitMessenger.Maui.Components.Utils;

namespace ReenbitMessenger.Maui.Components.Pages
{
    public partial class ChatsList
    {
        private class GroupChatsFilterModel
        {
            public int NumberOfGroupChats { get; set; } = 5;
            public int Page { get; set; } = 0;
            public string ValueContains { get; set; } = "";
            public bool Ascending { get; set; } = true;
            public string OrderBy { get; set; } = "Name";
        }

        private string newGroupChatName { get; set; } = string.Empty;

        private string groupChatId { get; set; } = string.Empty;

        GroupChatsFilterModel filterModel = new GroupChatsFilterModel();

        IEnumerable<GroupChat> groupChats { get; set; } = new List<GroupChat>();

        protected override async Task OnInitializedAsync()
        {
            chatService.Initialize();
            await UpdateChatsList();
        }

        protected async Task UpdateChatsList()
        {
            groupChats = await chatService.GetUserGroupChatsAsync(new GetGroupChatsRequest { 
                NumberOfGroupChats = filterModel.NumberOfGroupChats,
                Page = filterModel.Page,
                ValueContains = filterModel.ValueContains,
                Ascending = filterModel.Ascending,
                OrderBy = filterModel.OrderBy
            });
        }

        protected async Task CreateGroupChat()
        {
            if (!string.IsNullOrEmpty(newGroupChatName))
            {
                await chatService.CreateGroupChatAsync(new CreateGroupChatRequest { Name = newGroupChatName });
            }

            newGroupChatName = string.Empty;
            await UpdateChatsList();
        }

        protected async Task OnGroupClicked(Guid chatId)
        {
            navManager.NavigateTo($"/groupChat/{Convert.ToString(chatId)}", true);
        }

        protected async Task OpenJoinGroupChatDialog()
        {
            var parameters = new DialogParameters { ["Value"] = groupChatId };
            DialogOptions closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };

            var dialog = DialogService.Show<EnterValueDialog>("Enter id of a group chat you want to join", options: closeOnEscapeKey, parameters: parameters);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                string groupChatToJoinId = Convert.ToString(result.Data);

                await chatService.JoinGroupChatAsync(groupChatToJoinId);
                await UpdateChatsList();
            }
        }

        private async Task OnValueChanged(int newValue)
        {
            filterModel.NumberOfGroupChats = newValue;

            await UpdateChatsList();
        }

        private async Task MoveForward()
        {
            filterModel.Page++;
            await UpdateChatsList();
        }

        private async Task MoveBackward()
        {
            filterModel.Page--;
            await UpdateChatsList();
        }
    }
}
