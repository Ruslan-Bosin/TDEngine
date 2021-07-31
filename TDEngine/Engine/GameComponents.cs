using System;
using System.Collections.Generic;
using System.Text;

namespace TDEngine {

    class GCPosition : GEComponent {

        public CGRect frame { get; set; }
        public GCPosition(CGRect frame) {
            title = "Position";
            this.frame = frame;
        }

    }

}
