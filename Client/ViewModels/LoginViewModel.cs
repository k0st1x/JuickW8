using System.Net;
using System.Windows.Input;
using Juick.Api;
using Juick.Api.Extensions;
using Juick.Client.Common;
using Juick.Common.Services;
using Juick.Common.Windows;

namespace Juick.Client.ViewModels {
    public class LoginViewModel : BindableBase {
        readonly ICredentialStorage credentialStorage;

        string login;
        string password;
        HttpStatusCode? httpStatusCode;
        bool isLoading;

        public string Login {
            get { return login; }
            set { SetProperty(ref login, value); }
        }

        public string Password {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public HttpStatusCode? HttpStatusCode {
            get { return httpStatusCode; }
            set { SetProperty(ref httpStatusCode, value); }
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

        public LoginViewModel(ICredentialStorage credentialStorage) {
            this.credentialStorage = credentialStorage;

            LoginCommand = new DelegateCommand(DoLogin);

            var credential = credentialStorage.LoadCredential();
            if(credential != null) {
                login = credential.UserName;
                password = credential.Password;
            }
        }

        async void DoLogin() {
            HttpStatusCode = null;
            IsLoading = true;

            var client = new JuickClient();
            client.SetCredential(new NetworkCredential(login, password));
            var code = await client.CheckStatusCode();
            HttpStatusCode = code;
            IsLoading = false;
            if(code.IsAuthenticated()) {
                credentialStorage.SaveCredential(new NetworkCredential(login, password));
                // redirect
            } else {
                Password = string.Empty;
            }
        }
    }
}
