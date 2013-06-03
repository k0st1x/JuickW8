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
        static readonly Uri BaseUri = new Uri("ms-appx:///");

        public SampleDataCommon(string title, string subtitle, string imagePath, string smallImagePath, string description) {
            this.title = title;
            this.subtitle = subtitle;
            this.description = description;
            this.imagePath = imagePath;
            this.smallImagePath = smallImagePath;
        }

        string smallImagePath;
        public string SmallImagePath {
            get { return smallImagePath; }
            set { SetProperty(ref smallImagePath, value); }
        }

        ImageSource smallImage;
        public ImageSource SmallImage {
            get {
                if(smallImage == null && smallImagePath != null) {
                    smallImage = new BitmapImage(new Uri(BaseUri, smallImagePath));
                }
                return smallImage;
            }
        }

        private string title = string.Empty;
        public string Title {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        private string subtitle = string.Empty;
        public string Subtitle {
            get { return this.subtitle; }
            set { this.SetProperty(ref this.subtitle, value); }
        }

        private string description = string.Empty;
        public string Description {
            get { return this.description; }
            set { this.SetProperty(ref this.description, value); }
        }

        protected string ImagePath {
            get { return imagePath; }
        }

        private ImageSource _image = null;
        private string imagePath = null;
        public ImageSource Image {
            get {
                if(this._image == null && this.imagePath != null) {
                    this._image = new BitmapImage(new Uri(SampleDataCommon.BaseUri, this.imagePath));
                }
                return this._image;
            }

            set {
                this.imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path) {
            this._image = null;
            this.imagePath = path;
            this.OnPropertyChanged(() => Image);
        }

        public override string ToString() {
            return this.Title;
        }

        //private string content = string.Empty;
        //public string Content {
        //    get { return this.content; }
        //    set { this.SetProperty(ref this.content, value); }
        //}
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon {
        public SampleDataItem(int mid, string title, string subtitle, string imagePath, string description, string tags, string photoUrl, SampleDataGroup group)
            : base(title, subtitle, imagePath, imagePath, description) {
            MId = mid;
            this.group = group;
            this.photoUrl = photoUrl;
        }

        string tags;
        public string Tags {
            get { return tags; }
            set { SetProperty(ref tags, value); }
        }

        string photoUrl;
        public string PhotoUrl {
            get { return photoUrl; }
            set { SetProperty(ref photoUrl, value); }
        }

        int mid;
        public int MId {
            get { return mid; }
            set { SetProperty(ref mid, value); }
        }

        private SampleDataGroup group;
        public SampleDataGroup Group {
            get { return this.group; }
            set { this.SetProperty(ref this.group, value); }
        }

        ObservableCollection<SampleDataReplyItem> comments;
        public ObservableCollection<SampleDataReplyItem> Replies {
            get {
                if(comments == null) {
                    comments = new ObservableCollection<SampleDataReplyItem>();
                    if(CommentsRequested != null) {
                        CommentsRequested(this, EventArgs.Empty);
                    }
                }
                return comments;
            }
        }

        public event EventHandler CommentsRequested;

        public SampleDataReplyItem ToTopReplyItem() {
            return new SampleDataReplyItem(MId, Title, Subtitle, ImagePath, Description, PhotoUrl, this);
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon {
        public SampleDataGroup(GroupKind groupKind, string title, string imagePath, string smallImagePath, Brush brush)
            : base(title, string.Empty, imagePath, smallImagePath, null) {
            this.groupKind = groupKind;
            this.brush = brush;
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        Brush brush;
        public Brush Brush {
            get { return brush; }
            set { SetProperty(ref brush, value); }
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

    public class SampleDataReplyItem : SampleDataCommon {
        public int Id { get; private set; }

        private SampleDataItem messageItem;
        public SampleDataItem MessageItem {
            get { return this.messageItem; }
            set { this.SetProperty(ref messageItem, value); }
        }

        string photoUrl;
        public string PhotoUrl {
            get { return photoUrl; }
            set { SetProperty(ref photoUrl, value); }
        }

        public SampleDataReplyItem(int id, string title, string subtitle, string imagePath, string description, string photoUrl, SampleDataItem messageItem)
            : base(title, subtitle, imagePath, imagePath, description) {
            Id = id;
            MessageItem = messageItem;
            this.photoUrl = photoUrl;
        }
    }
}
