using System;
using System.Threading.Tasks;
using Juick.Api;
using Juick.Client.Data;
using Juick.Common;

namespace Juick.Client.Services {
    public class LocalStorageService : ILocalStorageService {
        #region ILocalStorageService Members
        public async Task SaveGroup(GroupKind groupKind, Message[] messages) {
            throw new NotImplementedException();
        }

        public Task<Message[]> LoadGroup(GroupKind groupKind) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
