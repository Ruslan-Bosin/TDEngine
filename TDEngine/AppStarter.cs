using System;
using System.Collections.Generic;
using System.Text;

namespace TDEngine {

    class AppStarter {

        static CGWindow window = new CGWindow();
        static GEScene[] scenes = {
            new GEScene(title: "Scene1", gameScene: new Scene()),
            new GEScene(title: "Scene2", gameScene: new Scene2())
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
