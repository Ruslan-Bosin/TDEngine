using System;
using System.Collections.Generic;
using System.Text;

namespace TDEngine {

    class Scene : GEGameScene {

        CGWindow window;
        public Scene(CGWindow window) { this.window = window; }

        public void didLoad() {
            // Did Load

            window.title = "Hello World!";
        }

        public void update() {
            // Update

            window.addSubview();
        }
    }

}
