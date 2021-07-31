﻿using System;
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

            var a = new GEObject();
            a.transform.scale = new CGSize(100, 100);
            a.circleCollider = new GECircleCollider(a.transform.position, a.transform.scale.width);
            a.rectCollider = new GERectCollider(a.transform.position, a.transform.scale);
            a.body = new GEBody(a.transform, new CGVector(0, 0));
            a.rendering = new GERendering(a.transform, window); 

            var b = new GEObject();
            b.transform.scale = new CGSize(50, 50);
            b.transform.position = new CGPoint(300, 300);
            b.circleCollider = new GECircleCollider(b.transform.position, b.transform.scale.width);
            b.rectCollider = new GERectCollider(b.transform.position, b.transform.scale);
            b.body = new GEBody(b.transform, new CGVector(0, 0));
            b.rendering = new GERendering(b.transform, window);

            while (window.isOpen) {
                window.dispatchEvents();

                a.update();
                b.update();

                if (Keyboard.IsKeyPressed(Keyboard.Key.W)) {
                    a.transform.position += new CGVector(0, -3);
                } else if (Keyboard.IsKeyPressed(Keyboard.Key.A)) {
                    a.transform.position += new CGVector(-3, 0);
                } else if (Keyboard.IsKeyPressed(Keyboard.Key.S)) {
                    a.transform.position += new CGVector(0, 3);
                } else if (Keyboard.IsKeyPressed(Keyboard.Key.D)) {
                    a.transform.position += new CGVector(3, 0);
                }

                if (a.circleCollider.isIntersectsWith(b.circleCollider)) {
                    a.rendering.backgroundColor = new CGColor("000000", 100);
                } else {
                    a.rendering.backgroundColor = new CGColor("FFFFFF", 255);
                }

                window.display(CGColors.Aqua);
            }

        }

    }

}