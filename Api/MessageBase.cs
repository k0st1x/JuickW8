using System;

namespace Juick.Api {
    public abstract class MessageBase {
        public abstract int Id { get; }
        public User User { get; set; }
        public string Body { get; set; }
        public DateTime TimeStamp { get; set; }
        public Photo Photo { get; set; }
    }
}
