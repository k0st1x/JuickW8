using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Juick.Common;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace Juick.Client.Data {
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : Juick.Client.Common.BindableBase {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(string title, string subtitle, string imagePath, string description) {
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _title = string.Empty;
        public string Title {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private string _imagePath = null;
        public ImageSource Image {
            get {
                if(this._image == null && this._imagePath != null) {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path) {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public override string ToString() {
            return this.Title;
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon {
        public SampleDataItem(int mid, string title, string subtitle, string imagePath, string description, string content, SampleDataGroup group)
            : base(title, subtitle, imagePath, description) {
            MId = mid;
            this._content = content;
            this._group = group;
        }

        int mid;
        public int MId {
            get { return mid; }
            set { SetProperty(ref mid, value); }
        }

        private string _content = string.Empty;
        public string Content {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }

        ObservableCollection<SampleDataCommentItem> comments;
        public ObservableCollection<SampleDataCommentItem> Comments {
            get {
                if(comments == null) {
                    comments = new ObservableCollection<SampleDataCommentItem>();
                    if(CommentsRequested != null) {
                        CommentsRequested(this, EventArgs.Empty);
                    }
                }
                return comments;
            }
        }

        public event EventHandler CommentsRequested;
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon {
        public SampleDataGroup(GroupKind groupKind, string title, string subtitle, string imagePath, string description)
            : base(title, subtitle, imagePath, description) {
            this.groupKind = groupKind;
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        GroupKind groupKind = GroupKind.None;
        public GroupKind GroupKind {
            get { return groupKind; }
            set { this.SetProperty(ref groupKind, value); }
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed

            switch(e.Action) {
                case NotifyCollectionChangedAction.Add:
                    if(e.NewStartingIndex < 12) {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        if(TopItems.Count > 12) {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if(e.OldStartingIndex < 12 && e.NewStartingIndex < 12) {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    } else if(e.OldStartingIndex < 12) {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    } else if(e.NewStartingIndex < 12) {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if(e.OldStartingIndex < 12) {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if(Items.Count >= 12) {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if(e.OldStartingIndex < 12) {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while(TopItems.Count < Items.Count && TopItems.Count < 12) {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items {
            get { return this._items; }
        }

        private ObservableCollection<SampleDataItem> _topItem = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> TopItems {
            get { return this._topItem; }
        }
    }

    public class SampleDataCommentItem : SampleDataCommon {
        public int CId { get; private set; }

        string _content = string.Empty;
        public string Content {
            get { return this._content; }
            set { this.SetProperty(ref _content, value); }
        }

        private SampleDataItem messageItem;
        public SampleDataItem MessageItem {
            get { return this.messageItem; }
            set { this.SetProperty(ref messageItem, value); }
        }

        public SampleDataCommentItem(int cid, string title, string subtitle, string imagePath, string description, SampleDataItem messageItem)
            : base(title, subtitle, imagePath, description) {
            CId = cid;
            MessageItem = messageItem;
        }
    }
}
