using System;
using System.Collections.Generic;
using System.Text;

namespace TDEngine {

    class GEObject {

        public int id;
        public string title;
        public List<GEComponent> components;

        public GEObject(int id, string title, List<GEComponent> components) {

            this.id = id;
            this.title = title;
            this.components = new List<GEComponent> { new GCPosition( new CGRect(x: 0, y: 0, width: 0, height: 0, rotation: 0) ) };
            this.components.AddRange(components);

        }

    }

    class GEComponent {

        public string title { get; set; }
        public virtual void main() {}

    }

    interface GEGameScene {

        public void didLoad();
        public void update();

    }

    class GEScene {

        public string title;
        public GEGameScene gameScene;

        public GEScene(string title, GEGameScene gameScene) {
            this.title = title;
            this.gameScene = gameScene;
        }

    }

}
