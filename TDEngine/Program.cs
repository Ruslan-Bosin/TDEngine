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

            window = new RenderWindow(new VideoMode(800, 600), "Test");

            window.Closed += (obj, e) => { window.Close(); };
            window.Resized += (obj, e) => { window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height))); };

            while (window.IsOpen) {
                window.DispatchEvents();

                window.Display();
            }

            int number = 0;
            Console.WriteLine("Random int is - " + number.randomInt(min: 0, max: 10).ToString());
        }

    }

}
