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
    public class RegisterViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ApiClientService _apiClientService;

        private string _error;

        public RegisterViewModel(
            ApiClientService apiClientService,
            INavigationService navigationService)
        {
            _navigationService = navigationService;
            _apiClientService = apiClientService;

            RegisterCommand = new RelayCommand(HandleRegister);
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public int Avatar {  get; set; }
        public int NotificationTimeOut {  get; set; }

        public string Error
        {
            get => _error;
            set => SetField(ref _error, value);
        }

        public ICommand RegisterCommand { get; }

        public ICommand LoginCommand { get; }


        private async Task HandleRegister()
        {
            try
            {

                var isRegistered = await _apiClientService
                    .RegisterAsync(Login, Password,Avatar,NotificationTimeOut);

                if (isRegistered)
                {
                    Error = string.Empty;
                    await _navigationService.GoToAsync(Route.Login, keepHistory: false);
                }
            }
            catch (Exception e)
            {
                Error = $"Please use another username to register.\n{e.Message}";
            }
        }
    }
}
