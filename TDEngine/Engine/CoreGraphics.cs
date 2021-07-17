using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using SFML.Window;
using SFML.Graphics;
using SFML.System;

using System.Runtime.InteropServices;


namespace TDEngine {

    public enum CGWindowStyles {
        None,
        Titlebar,
        Resize,
        Close,
        Default,
        Fullscreen
    }

    public class CGScreen {

        public float width = 0;
        public float height = 0;

        public CGScreen() {
            if (isOS(OSPlatform.Windows)) {
                try {
                    width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width;
                    height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height;
                } finally {}
            }
        }

        private bool isOS(OSPlatform OSPlatform) {
            bool isOs = RuntimeInformation.IsOSPlatform(OSPlatform);
            return isOs;
        }

    }

    public class CGWindow {

        private RenderWindow window;
        private string _title;
        private CGSize _size;
        private CGPoint _position;
        private CGRect _rect;
        private uint _frameLimit;

        private Styles __windowStyle;

        public string title {
            get {
                return _title;
            } set {
                _title = value;
                window.SetTitle(_title);
            }
        }
        public CGSize size {
            get {
                return _size;
            } set {
                _size = value;
                window.Size = new Vector2u((uint)_size.width, (uint)_size.height);
                _rect = new CGRect(_position, _size, 0);
            }
        }
        public CGPoint position {
            get {
                return _position;
            }
            set {
                _position = value;
                window.Position = new Vector2i((int)_position.x, (int)_position.y);
                _rect = new CGRect(_position, _size, 0);
            }
        }
        public CGRect rect {
            get {
                return _rect;
            }
            set {
                _rect = value;
                _position = new CGPoint(value.x, value.y);
                _size = new CGSize(value.width, value.height);
                window.Position = new Vector2i((int)_position.x, (int)_position.y);
                window.Size = new Vector2u((uint)_size.width, (uint)_size.height);
            }
        }
        public bool isOpen {
            get {
                return window.IsOpen;
            }
            set {
                if (!value) {
                    window.Close();
                }
            }
        }
        public uint frameLimit {
            get {
                return _frameLimit;
            }
            set {
                _frameLimit = value;
                window.SetFramerateLimit(_frameLimit);
            }
        }


        public CGWindow() {
            _title = "Window";
            _size = new CGSize(500, 500);
            
            window = new RenderWindow(new VideoMode((uint)_size.width, (uint)_size.height), _title);

            _position = new CGPoint(window.Position.X, window.Position.Y);
            _rect = new CGRect(_position, _size, 0);
            _frameLimit = 60;
            window.SetFramerateLimit(_frameLimit);
            window.Closed += (obj, e) => { window.Close(); };
        }

        public CGWindow(string title, CGSize size) {
            _title = title;
            _size = size;

            window = new RenderWindow(new VideoMode((uint)_size.width, (uint)_size.height), _title);

            _position = new CGPoint(window.Position.X, window.Position.Y);
            _rect = new CGRect(_position, _size, 0);
            _frameLimit = 60;
            window.SetFramerateLimit(_frameLimit);
            window.Closed += (obj, e) => { window.Close(); };
        }

        public CGWindow(CGWindowStyles windowSyle) {
            _title = "Window";
            _size = new CGSize(500, 500);

            switch (windowSyle) {
                case CGWindowStyles.None:
                    __windowStyle = Styles.None;
                    break;
                case CGWindowStyles.Titlebar:
                    __windowStyle = Styles.Titlebar;
                    break;
                case CGWindowStyles.Resize:
                    __windowStyle = Styles.Resize;
                    break;
                case CGWindowStyles.Close:
                    __windowStyle = Styles.Close;
                    break;
                case CGWindowStyles.Default:
                    __windowStyle = Styles.Default;
                    break;
                case CGWindowStyles.Fullscreen:
                    __windowStyle = Styles.Fullscreen;
                    break;
            }

            window = new RenderWindow(new VideoMode((uint)_size.width, (uint)_size.height), _title, __windowStyle);

            _position = new CGPoint(window.Position.X, window.Position.Y);
            _rect = new CGRect(_position, _size, 0);
            _frameLimit = 60;
            window.SetFramerateLimit(_frameLimit);
            window.Closed += (obj, e) => { window.Close(); };
        }

        public void addSubview() {
            RectangleShape rect = new RectangleShape(new Vector2f(100, 100));
            rect.FillColor = SFML.Graphics.Color.Cyan;
            window.Draw(rect);
        }


        public void dispatchEvents() {
            window.DispatchEvents();
        }

        public void display() {
            window.Display();
        }

    }

    public class CGPoint {

        public float x;
        public float y;

        public CGPoint(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public CGPoint(float x, float y) {
            this.x = x;
            this.y = y;
        }

    }

    public class CGSize {

        public float width;
        public float height;

        public CGSize(int width, int height) {
            this.width = width;
            this.height = height;
        }

        public CGSize(float width, float height) {
            this.width = width;
            this.height = height;
        }

    }

    public class CGRect {

        public float x;
        public float y;
        public float width;
        public float height;
        public float rotation;

        public CGRect(float x, float y, float width, float height, float rotation) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.rotation = rotation;
        }

        public CGRect(CGPoint point, float width, float height, float rotation) {
            x = point.x;
            y = point.y;
            this.width = width;
            this.height = height;
            this.rotation = rotation;
        }

        public CGRect(float x, float y, CGSize size, float rotation) {
            this.x = x;
            this.y = y;
            width = size.width;
            height = size.height;
            this.rotation = rotation;
        }

        public CGRect(CGPoint point, CGSize size, float rotation) {
            x = point.x;
            y = point.y;
            width = size.width;
            height = size.height;
            this.rotation = rotation;
        }
    }

}
