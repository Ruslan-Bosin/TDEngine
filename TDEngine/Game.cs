// System imports
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TDEngine {

    class Game {

        static CGWindow window = new CGWindow(CGWindowStyles.Fullscreen);
        // static CGScreen screen = new CGScreen();

        static void Main(string[] args) {

            // window.rect = new CGRect(0, 0, screen.width, screen.height);

            while (window.isOpen) {
                window.dispatchEvents();

                window.display();
            }

        }

    }

}
