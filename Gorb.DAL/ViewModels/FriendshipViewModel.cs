using Gorb.DAL.DataContracts.FriendshipData;
using Gorb.DAL.Helpers;
using Gorb.DAL.Services;
using Gorb.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gorb.DAL.ViewModels
{
    public class FriendshipsDetailsViewModel : BaseViewModel
    {
        public readonly ApiClientService _apiClientService;
        public readonly IAuthenticationService _authenticationService;

        public FriendshipsDetailsViewModel(
            ApiClientService apiClientService,
            IAuthenticationService authenticationService)
        {
            _apiClientService = apiClientService;
            _authenticationService = authenticationService;

            AddFriendCommand = new RelayCommand(HandleAddFriendCommandAsync);
        }

        public int FriendId { get; set; }
        public FriendsResponse FriendsData { get; private set; }

        public IEnumerable<IndexedListItem> FriendList { get; private set; }

        public ICommand AddFriendCommand { get; }

        public async Task InitializeAsync(
            CancellationToken cancellationToken = default)
        {
            var friendsResponse = await _apiClientService
                .GetMyFriendsAsync(cancellationToken);

            Update(friendsResponse);
        }

        private void Update(FriendsResponse friendsResponse)
        {
            FriendsData = friendsResponse;

            FriendList = FriendsData.Friends.Select((friend, index) => new IndexedListItem()
            {
                Index = index + 1,
                Name = friend.Nickname,
                CurrentBackLvl = friend.CurrentBackLvl
            });

            OnPropertyChanged(nameof(FriendsData));
            OnPropertyChanged(nameof(FriendList));
        }

        private async Task HandleAddFriendCommandAsync()
        {
            
            var updatedFriendsResponse = await _apiClientService.AddFriendAsync(FriendId);

            Update(updatedFriendsResponse);


        }
    }
}