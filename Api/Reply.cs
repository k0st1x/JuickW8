using System;

namespace Juick.Api {
    public class Reply : MessageBase {
        public override int Id {
            get { return RId; }
        }

        public int RId { get; set; }
    }
}
