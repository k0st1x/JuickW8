using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Juick.Api;
using Juick.Common;
using Windows.Storage;

namespace Juick.Client.Services {
    public class LocalStorageService : ILocalStorageService {
        static string GetFileName<T>(T value) {
            return value + ".xml";
        }

        StorageFolder folder;

        #region ILocalStorageService Members
        public async Task SaveGroup(GroupKind groupKind, Message[] messages) {
            var fileName = GetFileName(groupKind);
            await SaveCore(fileName, messages);
        }

        public async Task<Message[]> LoadGroup(GroupKind groupKind) {
            var fileName = GetFileName(groupKind);
            return await LoadCore<Message>(fileName);
        }

        public async Task SaveReplies(int mid, Reply[] replies) {
            var fileName = GetFileName(mid);
            await SaveCore(fileName, replies);
        }

        public Task<Reply[]> LoadReplies(int mid) {
            var fileName = GetFileName(mid);
            return LoadCore<Reply>(fileName);
        }
        #endregion

        async Task SaveCore<T>(string fileName, T[] items) {
            await ForceCreateFolder();
            if(items == null) {
                try {
                    var fileToDelete = await folder.GetFileAsync(fileName);
                    await fileToDelete.DeleteAsync();
                } catch(FileNotFoundException) {
                }
                return;
            }

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using(var transaction = await file.OpenTransactedWriteAsync())
            using(var stream = transaction.Stream.AsStreamForWrite()) {
                var serializer = new DataContractSerializer(typeof(T[]));
                serializer.WriteObject(stream, items);
                stream.Flush();
                await transaction.CommitAsync();
            }
        }

        async Task<T[]> LoadCore<T>(string fileName) {
            await ForceCreateFolder();
            try {
                using(var stream = await folder.OpenStreamForReadAsync(fileName)) {
                    var serializer = new DataContractSerializer(typeof(T[]));
                    return serializer.ReadObject(stream) as T[];
                }
            } catch {
                return new T[0];
            }
        }

        async Task ForceCreateFolder() {
            if(folder != null) {
                return;
            }
            const string FolderName = "SavedContent";
            var localFolder = ApplicationData.Current.LocalFolder;
            folder = await localFolder.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);
        }
    }
}
