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
        public CGRect frame {
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

        private void defaultsSettings() {

            _position = new CGPoint(window.Position.X, window.Position.Y);
            _rect = new CGRect(_position, _size, 0);
            _frameLimit = 60;
            window.SetFramerateLimit(_frameLimit);
            window.Closed += (obj, e) => { window.Close(); };
            window.Resized += (obj, e) => {
                window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
                size = new CGSize(e.Width, e.Height);
            };

        }

        public CGWindow() {
            _title = "Window";
            _size = new CGSize(500, 500);
            
            window = new RenderWindow(new VideoMode((uint)_size.width, (uint)_size.height), _title);

            defaultsSettings();
        }

        public CGWindow(string title, CGSize size) {
            _title = title;
            _size = size;

            window = new RenderWindow(new VideoMode((uint)_size.width, (uint)_size.height), _title);

            defaultsSettings();
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

            defaultsSettings();
        }

        public void draw(Drawable obj) {
            window.Draw(obj);
        }

        public void dispatchEvents() {
            window.DispatchEvents();
        }

        public void display(CGColor backgroundColor) {
            window.Display();
            window.Clear(backgroundColor.toSfmlColor());
        }

        public void display(CGColors backgroundColor) {
            window.Display();
            window.Clear(CGColor.set(backgroundColor).toSfmlColor());
        }

    }

    public class CGVector {

        public float x;
        public float y;
        public float value {
            get {
                CGVector _vector = new CGVector(x, y);
                return _vector.getDistanceTo(new CGPoint(0, 0));
            }
        }

        public CGVector(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public CGVector(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public CGVector(CGPoint point) {
            x = point.x;
            y = point.y;
        }

        public void invert() {
            x = -x;
            y = -y;
        }

        public CGVector inverted() {
            return new CGVector(-x, -y);
        }

        public float getDistanceTo(CGPoint point) {
            double result = Math.Sqrt(Math.Pow((point.x - x), 2) + Math.Pow((point.y - y), 2));
            return (float)result;
        }

        public float getRelativeValueTo(CGPoint point) {
            return (x * point.x) + (y * point.y);
        }

        // +
        public static CGVector operator + (CGVector value1, CGVector value2) {
            return new CGVector(x: value1.x + value2.x, y: value1.y + value2.y);
        }

        public static CGVector operator +(CGVector value1, CGPoint value2) {
            return new CGVector(x: value1.x + value2.x, y: value1.y + value2.y);
        }

        // -
        public static CGVector operator -(CGVector value1, CGVector value2) {
            return new CGVector(x: value1.x - value2.x, y: value1.y - value2.y);
        }

        public static CGVector operator -(CGVector value1, CGPoint value2) {
            return new CGVector(x: value1.x - value2.x, y: value1.y - value2.y);
        }

        // *
        public static CGVector operator *(CGVector value1, CGVector value2) {
            return new CGVector(x: value1.x * value2.x, y: value1.y * value2.y);
        }

        public static CGVector operator *(CGVector value1, CGPoint value2) {
            return new CGVector(x: value1.x * value2.x, y: value1.y * value2.y);
        }

        // /
        public static CGVector operator /(CGVector value1, CGVector value2) {
            return new CGVector(x: value1.x / value2.x, y: value1.y / value2.y);
        }

        public static CGVector operator /(CGVector value1, CGPoint value2) {
            return new CGVector(x: value1.x / value2.x, y: value1.y / value2.y);
        }

        // ==
        public static bool operator ==(CGVector value1, CGVector value2) {
            if ((value1.x == value2.x) && (value1.y == value2.y)) return true;
            return false;
        }

        public static bool operator !=(CGVector value1, CGVector value2) {
            if ((value1.x == value2.x) && (value1.y == value2.y)) return false;
            return true;
        }

        // Casting
        public CGPoint cgPoint() {
            return new CGPoint(x: x, y: y);
        }

        public override string ToString() {
            return $"CGVector x: {x}, y: {y}";
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

        public CGPoint(CGVector vector) {
            x = vector.x;
            y = vector.y;
        }

        // +
        public static CGPoint operator +(CGPoint value1, CGPoint value2) {
            return new CGPoint(x: value1.x + value2.x, y: value1.y + value2.y);
        }

        public static CGPoint operator +(CGPoint value1, CGVector value2) {
            return new CGPoint(x: value1.x + value2.x, y: value1.y + value2.y);
        }

        // -
        public static CGPoint operator -(CGPoint value1, CGPoint value2) {
            return new CGPoint(x: value1.x - value2.x, y: value1.y - value2.y);
        }

        public static CGPoint operator -(CGPoint value1, CGVector value2) {
            return new CGPoint(x: value1.x - value2.x, y: value1.y - value2.y);
        }

        // *
        public static CGPoint operator *(CGPoint value1, CGPoint value2) {
            return new CGPoint(x: value1.x * value2.x, y: value1.y * value2.y);
        }

        public static CGPoint operator *(CGPoint value1, CGVector value2) {
            return new CGPoint(x: value1.x * value2.x, y: value1.y * value2.y);
        }

        // /
        public static CGPoint operator /(CGPoint value1, CGPoint value2) {
            return new CGPoint(x: value1.x / value2.x, y: value1.y / value2.y);
        }

        public static CGPoint operator /(CGPoint value1, CGVector value2) {
            return new CGPoint(x: value1.x / value2.x, y: value1.y / value2.y);
        }

        // ==
        public static bool operator ==(CGPoint value1, CGPoint value2) {
            if ((value1.x == value2.x) && (value1.y == value2.y)) return true;
            return false;
        }

        public static bool operator !=(CGPoint value1, CGPoint value2) {
            if ((value1.x == value2.x) && (value1.y == value2.y)) return false;
            return true;
        }

        // Casting
        public CGVector cgVector() {
            return new CGVector(x: x, y: y);
        }


        public override string ToString() {
            return $"CGPoint x: {x}, y: {y}";
        }

        public int getDistanceTo(CGPoint point) {
            double result = Math.Round(Math.Sqrt(Math.Pow((point.x - x), 2) + Math.Pow((point.y - y), 2)), MidpointRounding.AwayFromZero);
            return (int)result;
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

        public static CGSize operator +(CGSize value1, CGSize value2) {
            return new CGSize(width: value1.width + value2.width, height: value1.height + value2.height);
        }
        public static CGSize operator -(CGSize value1, CGSize value2) {
            return new CGSize(width: value1.width - value2.width, height: value1.height - value2.height);
        }
        public static CGSize operator *(CGSize value1, CGSize value2) {
            return new CGSize(width: value1.width * value2.width, height: value1.height * value2.height);
        }
        public static CGSize operator /(CGSize value1, CGSize value2) {
            return new CGSize(width: value1.width / value2.width, height: value1.height / value2.height);
        }

        // ==
        public static bool operator ==(CGSize value1, CGSize value2) {
            if ((value1.width == value2.width) && (value1.height == value2.height)) return true;
            return false;
        }

        public static bool operator !=(CGSize value1, CGSize value2) {
            if ((value1.width == value2.width) && (value1.height == value2.height)) return false;
            return true;
        }

        public override string ToString() {
            return $"CGSize width: {width}, height: {height}";
        }

        public CGPoint toPoint() {
            return new CGPoint(width, height);
        }

    }

    public class CGRect {

        private float _x;
        private float _y;
        private float _width;
        private float _height;
        private CGPoint _point;
        private CGSize _size;


        public float x {
            get {
                return _x;
            } set {
                _x = value;
                _point = new CGPoint(x: value, y: _y);
            }
        }
        public float y {
            get {
                return _y;
            } set {
                _y = value;
                _point = new CGPoint(x: _x, y: value);
            }
        }
        public float width {
            get {
                return _width;
            }
            set {
                _width = value;
                _size = new CGSize(width: value, height: _height);
            }
        }
        public float height {
            get {
                return _height;
            }
            set {
                _height = value;
                _size = new CGSize(width: _width, height: value);
            }
        }
        public float rotation;

        public CGPoint point { get {
                return _point;
            } set {
                _point = value;
                _x = value.x;
                _y = value.y;
            }
        }
        public CGSize size { get {
                return _size;
            } set {
                _size = value;
                _width = value.width;
                _height = value.height;
            }
        }

        public CGRect(float x, float y, float width, float height, float rotation) {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            this.rotation = rotation;

            _point = new CGPoint(x: _x, y: _y);
            _size = new CGSize(width: _width, height: _height);
        }

        public CGRect(CGPoint point, float width, float height, float rotation) {
            _x = point.x;
            _y = point.y;
            _width = width;
            _height = height;
            this.rotation = rotation;

            _point = new CGPoint(x: _x, y: _y);
            _size = new CGSize(width: _width, height: _height);
        }

        public CGRect(float x, float y, CGSize size, float rotation) {
            _x = x;
            _y = y;
            _width = size.width;
            _height = size.height;
            this.rotation = rotation;

            _point = new CGPoint(x: _x, y: _y);
            _size = new CGSize(width: _width, height: _height);
        }

        public CGRect(CGPoint point, CGSize size, float rotation) {
            _x = point.x;
            _y = point.y;
            _width = size.width;
            _height = size.height;
            this.rotation = rotation;

            _point = new CGPoint(x: _x, y: _y);
            _size = new CGSize(width: _width, height: _height);
        }

        public CGRect(float x, float y, float width, float height) {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            rotation = 0;

            _point = new CGPoint(x: _x, y: _y);
            _size = new CGSize(width: _width, height: _height);
        }

        public CGRect(CGPoint point, float width, float height) {
            _x = point.x;
            _y = point.y;
            _width = width;
            _height = height;
            rotation = 0;

            _point = new CGPoint(x: _x, y: _y);
            _size = new CGSize(width: _width, height: _height);
        }

        public CGRect(float x, float y, CGSize size) {
            _x = x;
            _y = y;
            _width = size.width;
            _height = size.height;
            rotation = 0;

            _point = new CGPoint(x: _x, y: _y);
            _size = new CGSize(width: _width, height: _height);
        }

        public CGRect(CGPoint point, CGSize size) {
            _x = point.x;
            _y = point.y;
            _width = size.width;
            _height = size.height;
            rotation = 0;

            _point = new CGPoint(x: _x, y: _y);
            _size = new CGSize(width: _width, height: _height);
        }

        public static CGRect operator +(CGRect value1, CGRect value2) {
            return new CGRect(x: value1.x + value2.x, y: value1.y + value2.y, width: value1.width + value2.width, height: value1.height + value2.height, rotation: value1.rotation + value2.rotation);
        }

        public static CGRect operator -(CGRect value1, CGRect value2) {
            return new CGRect(x: value1.x - value2.x, y: value1.y - value2.y, width: value1.width - value2.width, height: value1.height - value2.height, rotation: value1.rotation - value2.rotation);
        }

        public static CGRect operator *(CGRect value1, CGRect value2) {
            return new CGRect(x: value1.x * value2.x, y: value1.y * value2.y, width: value1.width * value2.width, height: value1.height * value2.height, rotation: value1.rotation * value2.rotation);
        }
        
        public static CGRect operator /(CGRect value1, CGRect value2) {
            return new CGRect(x: value1.x / value2.x, y: value1.y / value2.y, width: value1.width / value2.width, height: value1.height / value2.height, rotation: value1.rotation / value2.rotation);
        }

        public override string ToString() {
            return $"CGRect x: {x}, y: {y}, width: {width}, height: {height}";
        }

    }

    public class CGColor {

        public byte red;
        public byte green;
        public byte blue;
        public byte alpha;

        public CGColor(byte red, byte green, byte blue, byte alpha) {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }

        public CGColor(string hex, byte alpha) {
            if (hex.Length != 6) throw new Exception("Invalid hex string length, must contain six characters. The \"#\" symbol is not needed");
            this.alpha = alpha;

            try {
                red = Convert.ToByte(hex.Substring(0, 2), 16);
                green = Convert.ToByte(hex.Substring(2, 2), 16);
                blue = Convert.ToByte(hex.Substring(4, 2), 16);
            } catch (Exception e) {
                throw new Exception("Failed to parse hex and convert to rgb");
            }
            
        }

        public CGColor(CGColors color) {
            red = set(color).red;
            green = set(color).green;
            blue = set(color).blue;
            alpha = set(color).alpha;
        }

        public override string ToString() {
            string result = Convert.ToString(red, 16) + Convert.ToString(green, 16) + Convert.ToString(blue, 16);
            return result;
        }

        public static CGColor set(CGColors color) {
            switch (color) {
                case CGColors.Blue:
                    return new CGColor("0000FF", 255);
                case CGColors.Aqua:
                    return new CGColor("00FFFF", 255);
                case CGColors.Green:
                    return new CGColor("008000", 255);
                case CGColors.Yellow:
                    return new CGColor("FFFF00", 255);
                case CGColors.Red:
                    return new CGColor("FF0000", 255);
                case CGColors.Purple:
                    return new CGColor("800080", 255);
                case CGColors.White:
                    return new CGColor("FFFFFF", 255);
                case CGColors.Gray:
                    return new CGColor("808080", 255);
                case CGColors.Black:
                    return new CGColor("000000", 255);
                case CGColors.Bisque:
                    return new CGColor("FFE4C4", 255);
                case CGColors.Indigo:
                    return new CGColor("4B0082", 255);
                case CGColors.Orange:
                    return new CGColor("FFA500", 255);
                case CGColors.Pink:
                    return new CGColor("FFC0CB", 255);
                case CGColors.Lime:
                    return new CGColor("00FF00", 255);
                case CGColors.DarkGray:
                    return new CGColor("A9A9A9", 255);
                default:
                    return new CGColor("FFFFFF", 255);
            }
        }

        public SFML.Graphics.Color toSfmlColor() {
            return new SFML.Graphics.Color(red, green, blue, alpha);
        }

    }

    public class CGImage {

        private SFML.Graphics.Image img;

        private bool _isFlippedHorizontally = false;
        private bool _isFlippedVertically = false;

        public uint width {
            get {
                return img.Size.X;
            }
        }
        public uint height {
            get {
                return img.Size.Y;
            }
        }

        public readonly string path = "NONE";
        public bool isFlippedHorizontally { get { return _isFlippedHorizontally; } }
        public bool isFlippedVertically { get { return _isFlippedVertically; } }

        public CGImage(string path) {
            img = new SFML.Graphics.Image(path);
            this.path = path;
        }

        public SFML.Graphics.Image toSfmlImage() {
            return img;
        }

        public void flipHorizontally() {
            img.FlipHorizontally();
            _isFlippedHorizontally = !_isFlippedHorizontally;
        }

        public void flipVertically() {
            img.FlipVertically();
            _isFlippedVertically = !_isFlippedVertically;
        }

        public void save(string path) {
            img.SaveToFile(path);
        }

        public void crop(CGRect area) {
            Texture texture = new Texture(image: img, area: new IntRect(left: (int)area.x, top: (int)area.y, width: (int)area.width, height: (int)area.height));
            img = texture.CopyToImage();
        }

        public void createMask(CGColor color) {
            img.CreateMaskFromColor(color.toSfmlColor());
        }

        public override string ToString() {
            return $"CGImage (path: {path})";
        }

    }

    public class CGTeexture {

        private Texture texture;

        public CGTeexture(CGImage image) {
            texture = new Texture(image.toSfmlImage());
        }

        public CGTeexture(string path) {
            texture = new Texture(path);
        }

        public void crop(CGRect area) {
            Texture temp = new Texture(image: texture.CopyToImage(), area: new IntRect(left: (int)area.x, top: (int)area.y, width: (int)area.width, height: (int)area.height));
            texture = new Texture(temp);
        }

    }

    public enum CGColors {
        Blue,
        Aqua,
        Green,
        Yellow,
        Red,
        Purple,
        White,
        Gray,
        Black,
        Bisque,
        Indigo,
        Orange,
        Pink,
        Lime,
        DarkGray
    }

}
