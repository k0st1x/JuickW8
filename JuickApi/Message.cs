using System;

namespace JuickApi {
    public class Message {
        public int MId { get; set; }
        public User User { get; set; }
        public string Body { get; set; }
        public string[] Tags { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Replies { get; set; }
    }
}
