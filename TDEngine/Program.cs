// System imports
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// SFML 
using SFML.Graphics;
using SFML.Window;

namespace TDEngine {

    class Program {

        static RenderWindow window;

        static void Main(string[] args) {

            window = new RenderWindow(new VideoMode(1080, 920), "Window Name");

            window.Closed += (obj, e) => { window.Close(); };
            window.Resized += (obj, e) => { window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height))); };

            while (window.IsOpen) {
                window.DispatchEvents();

                window.Display();
            }

        }

    }

}
