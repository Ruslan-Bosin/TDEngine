using System;
using System.Collections.Generic;
using System.Text;

using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace TDEngine {

    class AppStarter {

        static CGWindow window = new CGWindow();
        static GEScene[] scenes = {
            new GEScene(title: "Scene1", gameScene: new Scene(window: window)),
            new GEScene(title: "Scene2", gameScene: new Scene2(window: window))
        };
        static GEScene scene = scenes[0];

        static void Main(string[] args) {

            scene.gameScene.didLoad();

            while (window.isOpen) {
                window.dispatchEvents();

                scene.gameScene.update();

                window.display();
            }

        }

    }

}
