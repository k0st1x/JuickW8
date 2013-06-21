namespace Juick.Api {
    public class Message : MessageBase {
        public override int Id {
            get { return MId; }
        }

        public int MId { get; set; }
        public string[] Tags { get; set; }
        public int Replies { get; set; }
        public Video Video { get; set; }
    }
}
