using System.Linq;
using System.Net;
using Juick.Common.Services;
using Windows.Security.Credentials;

namespace Juick.Client.Services {
    public class CredentialStorage : ICredentialStorage {
        const string ResourceName = "JuickUser";
        readonly PasswordVault vault = new PasswordVault();

        #region ICredentialStorage Members
        public void SaveCredential(NetworkCredential credential) {
            var prevCredential = LoadCredentialCore();
            if(prevCredential != null) {
                vault.Remove(prevCredential);
            }
            vault.Add(new PasswordCredential(ResourceName, credential.UserName, credential.Password));
        }

        public NetworkCredential LoadCredential() {
            var credential = LoadCredentialCore();
            if(credential == null) {
                return null;
            }
            credential = vault.Retrieve(ResourceName, credential.UserName);
            return new NetworkCredential(credential.UserName, credential.Password);
        }
        #endregion

        PasswordCredential LoadCredentialCore() {
            try {
                return vault.FindAllByResource(ResourceName).FirstOrDefault();
            } catch {
                return null;
            }
        }
    }
}
