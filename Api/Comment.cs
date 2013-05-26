using System;

namespace Juick.Api {
    public class Comment : IContainsBody {
        public int CId { get; set; }
        public User User { get; set; }
        public string Body { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
