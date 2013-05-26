using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Juick.Client.Common;
using Juick.Client.Data;
using Juick.Client.Services;
using Juick.Common;

namespace Juick.Client.ViewModels {
    public class ReadThreadsViewModel : BindableBase {
        readonly IMessagesSourceService messagesSourceService;
        SampleDataGroup group;

        public ReadThreadsViewModel(IMessagesSourceService messagesSourceService) {
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

        public async Task InitializeGroup(SampleDataGroup group) {
            Group = group;
            if(Group.Items.Count == 0) {
                await messagesSourceService.FillItems(group);
            }
        }

        public async Task SaveState() {
            messagesSourceService.SaveState();
        }
    }
}
