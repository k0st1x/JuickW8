using System.Net;
using System.Net.Http;
using System.Windows.Input;
using Juick.Api;
using Juick.Api.Extensions;
using Juick.Client.Common;
using Juick.Client.Services;
using Juick.Common.Services;
using Juick.Common.UI;

namespace Juick.Client.ViewModels {
    public class LoginViewModel : BindableBase {
        readonly IJuickClient client;
        readonly ICredentialStorage credentialStorage;
        readonly INavigationManager navigationManager;

        string login;
        string password;
        string message;
        bool isLoading;

        public string Login {
            get { return login; }
            set { SetProperty(ref login, value); }
        }

        public string Password {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public string Message {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public bool IsLoading {
            get { return isLoading; }
            set {
                SetProperty(ref isLoading, value);
                OnPropertyChanged(() => IsEnabled);
            }
        }

        public bool IsEnabled {
            get { return !IsLoading; }
        }

        public ICommand LoginCommand { get; private set; }

        public LoginViewModel(IJuickClient juickClient, ICredentialStorage credentialStorage, INavigationManager navigationManager) {
            this.client = juickClient;
            this.credentialStorage = credentialStorage;
            this.navigationManager = navigationManager;

            LoginCommand = new DelegateCommand(DoLogin);

            var credential = credentialStorage.LoadCredential();
            if(credential != null) {
                login = credential.UserName;
                password = credential.Password;
            }
        }

        async void DoLogin() {
            Message = null;
            IsLoading = true;

            var credential = new NetworkCredential(login, password);
            client.SetCredential(credential);
            try {
                var code = await client.CheckStatusCode();

                if(code.IsAuthenticated()) {
                    credentialStorage.SaveCredential(credential);
                    navigationManager.OpenMain();
                } else {
                    Message = string.Concat((int)code, ": ", code);
                    Password = string.Empty;
                }
            } catch(HttpRequestException e) {
                Message = e.InnerException != null
                    ? e.InnerException.Message
                    : e.Message;
            }
            IsLoading = false;
        }
    }
}
