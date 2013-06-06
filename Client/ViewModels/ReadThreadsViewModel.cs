using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Juick.Client.Common;
using Juick.Client.Data;
using Juick.Client.Services;
using Juick.Common;

namespace Juick.Client.ViewModels {
    public class ReadThreadsViewModel : BindableBase {
        readonly IRootItemsContainer rootItemsContainer;
        readonly IMessagesSourceService messagesSourceService;
        SampleDataGroup group;

        public ReadThreadsViewModel(IRootItemsContainer rootItemsContainer, IMessagesSourceService messagesSourceService) {
            this.rootItemsContainer = rootItemsContainer;
            this.messagesSourceService = messagesSourceService;
        }

        public SampleDataGroup Group {
            get { return group; }
            set {
                SetProperty(ref group, value);
                OnPropertyChanged(() => Items);
            }
        }

        public ObservableCollection<SampleDataItem> Items {
            get { return group != null ? group.Items : null; }
        }

        public async Task InitializeGroup(GroupKind kind) {
            Group = rootItemsContainer.Root.Single(x => x.GroupKind == kind);
            if(Group.Items.Count == 0) {
                await messagesSourceService.FillItems(Group);
            }
        }

        public Task SaveState() {
            return messagesSourceService.SaveState();
        }
    }
}
