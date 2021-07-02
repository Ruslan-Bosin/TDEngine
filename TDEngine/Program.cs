// System imports
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// SFML 
using SFML.Graphics;
using SFML.Window;

// Other
using System.Drawing;

namespace TDEngine {

    class Program {

        static RenderWindow window;
        static Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;

        static void Main(string[] args) {

            uint width = (uint)resolution.Width;
            uint height = (uint)resolution.Height;

            window = new RenderWindow(new VideoMode(width, height), "Window Name");

            window.Closed += (obj, e) => { window.Close(); };
            window.Resized += (obj, e) => { window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height))); };

            while (window.IsOpen) {
                window.DispatchEvents();

                window.Display();
            }

        }

    }

}
