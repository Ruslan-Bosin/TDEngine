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

        static int width = resolution.Width;
        static int height = resolution.Height;

        static void Main(string[] args) {

            uint reducedWidth = (uint)width.percentageRatio(percentages: 80);
            uint reducedHeight = (uint)height.percentageRatio(percentages: 80);

            window = new RenderWindow(new VideoMode(reducedWidth, reducedHeight), "Window Name");

            window.Closed += (obj, e) => { window.Close(); };
            window.Resized += (obj, e) => { window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height))); };

            while (window.IsOpen) {
                window.DispatchEvents();

                window.Display();
            }

        }

    }

}
