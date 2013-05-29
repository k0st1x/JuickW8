using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Juick.Api;
using Juick.Common;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Juick.Client.Services {
    public class LocalStorageService : ILocalStorageService {
        readonly DataContractSerializer serializer = new DataContractSerializer(typeof(Message[]));
        readonly StorageFolder folder = ApplicationData.Current.LocalFolder;

        static string GetFileName(GroupKind groupKind) {
            return groupKind + ".xml";
        }

        #region ILocalStorageService Members
        public async Task SaveGroup(GroupKind groupKind, Message[] messages) {
            var fileName = GetFileName(groupKind);
            if(messages == null) {
                try {
                    var fileToDelete = await folder.GetFileAsync(fileName);
                    await fileToDelete.DeleteAsync();
                } catch(FileNotFoundException) {
                }
                return;
            }

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using(var transaction = await file.OpenTransactedWriteAsync()) {
                serializer.WriteObject(transaction.Stream.AsStreamForWrite(), messages);
                await transaction.CommitAsync();
            }
        }

        public async Task<Message[]> LoadGroup(GroupKind groupKind) {
            var fileName = GetFileName(groupKind);
            try {
                using(var stream = await folder.OpenStreamForReadAsync(fileName)) {
                    return serializer.ReadObject(stream) as Message[];
                }
            } catch {
                return new Message[0];
            }
        }
        #endregion
    }
}
