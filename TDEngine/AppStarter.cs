using System;
using System.Collections.Generic;
using System.Text;

using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace TDEngine {

    class AppStarter {

        static CGScreen screen = new CGScreen();
        static CGWindow window = new CGWindow("Game", new CGSize(((int)screen.width).percentageRatio(80), ((int)screen.height).percentageRatio(80)));

        static void Main(string[] args) {

            CGPoint[] verst = new CGPoint[] { new CGPoint(0, 0), new CGPoint(100, 0), new CGPoint(50, 80) };
            CGPoint[] verst2 = new CGPoint[] { new CGPoint(0, 100), new CGPoint(100, 100), new CGPoint(50, 0) };

            GEObject mob = new GEObject();
            mob.transform = new GETransform(new CGPoint(0, 0), new CGSize(100, 150), 0);
            mob.rendering = new GERendering(mob.transform, window);
            mob.polygonCollider = new GEPolygonCollider(verst);
            mob.rendering.defineShape(verst);

            GEObject mob2 = new GEObject();
            mob2.transform = new GETransform(new CGPoint(0, 0), new CGSize(100, 150), 0);
            mob2.rendering = new GERendering(mob2.transform, window);
            mob2.polygonCollider = new GEPolygonCollider(verst2);
            mob2.rendering.defineShape(verst2);


            while (window.isOpen) {
                window.dispatchEvents();

                mob.update();
                mob2.update();

                if (mob.polygonCollider.isIntersectsWith(mob2.polygonCollider)) {
                    mob.rendering.backgroundColor = new CGColor(CGColors.Red);
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.D)) {
                    mob.transform.position += new CGPoint(5, 0);
                } else if (Keyboard.IsKeyPressed(Keyboard.Key.A)) {
                    mob.transform.position -= new CGPoint(5, 0);
                } 
                if (Keyboard.IsKeyPressed(Keyboard.Key.W)) {
                    mob.transform.position -= new CGPoint(0, 5);
                } else if (Keyboard.IsKeyPressed(Keyboard.Key.S)) {
                    mob.transform.position += new CGPoint(0, 5);
                }

                window.display(CGColors.Indigo);
            }

        }

    }

}
