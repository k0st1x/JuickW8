using System.Net;
using System.Windows.Input;
using Juick.Api.Extensions;
using Juick.Client.Common;
using Juick.Common.Windows;
using JuickApi;

namespace Juick.Client.ViewModels {
    public class LoginViewModel : BindableBase {
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

        public LoginViewModel() {
            LoginCommand = new DelegateCommand(DoLogin);
        }

        async void DoLogin() {
            HttpStatusCode = null;
            IsLoading = true;

            var client = new JuickClient();
            client.SetCredential(new NetworkCredential(Login, Password));
            var code = await client.CheckStatusCode();
            HttpStatusCode = code;
            IsLoading = false;
            if(code.IsAuthenticated()) {
                // redirect
            } else {
                Password = string.Empty;
            }
        }
    }
}
