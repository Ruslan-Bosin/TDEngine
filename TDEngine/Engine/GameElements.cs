using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.System;

namespace TDEngine {

    public interface GECollider {

        public CGPoint center { get; set; }
        public CGSize halfSize { get; set; }
        public CGPoint offset { get; set; }

        public bool isIntersectsWith(GERectCollider collider);
        public bool isIntersectsWith(GECircleCollider collider);

        public void update();

    }

    public class GETransform {

        public CGPoint position;

        public CGSize scale;

        public float rotation;

        public GETransform(CGPoint position, CGSize scale, float rotation) {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
        }

    }

    public class GERectCollider : GECollider {

        public CGPoint center { get; set; }
        public CGSize halfSize { get; set; }
        public CGPoint offset { get; set; }

        private GETransform transform;

        public GERectCollider(GETransform transform) {
            this.transform = transform;
            halfSize = new CGSize(width: transform.scale.width / 2, height: transform.scale.height / 2);
            center = new CGPoint(x: transform.position.x + halfSize.width, y: transform.position.y + halfSize.height);
            offset = new CGPoint(x: 0, y: 0);
        }

        public GERectCollider(GETransform transform, CGSize size) {
            this.transform = transform;
            halfSize = new CGSize(width: size.width / 2, height: size.height / 2);
            center = new CGPoint(x: transform.position.x + halfSize.width, y: transform.position.y + halfSize.height);
            offset = new CGPoint(x: 0, y: 0);
        }

        public GERectCollider(GETransform transform, CGPoint offset) {
            this.transform = transform;
            halfSize = new CGSize(width: transform.scale.width / 2, height: transform.scale.height / 2);
            center = new CGPoint(x: transform.position.x + halfSize.width, y: transform.position.y + halfSize.height);
            this.offset = offset;
        }

        public GERectCollider(GETransform transform, CGSize size, CGPoint offset) {
            this.transform = transform;
            halfSize = new CGSize(width: size.width / 2, height: size.height / 2);
            center = new CGPoint(x: transform.position.x + halfSize.width, y: transform.position.y + halfSize.height);
            this.offset = offset;
        }

        public bool isIntersectsWith(GERectCollider collider) {
            if (Math.Abs(center.x - collider.center.x) > halfSize.width + collider.halfSize.width) return false;
            if (Math.Abs(center.y - collider.center.y) > halfSize.height + collider.halfSize.height) return false;
            return true;
        }

        public bool isIntersectsWith(GECircleCollider collider) {

            CGPoint topLeft = new CGPoint(x: center.x - halfSize.width, y: center.y - halfSize.height);
            CGPoint topRight = new CGPoint(x: center.x + halfSize.width, y: center.y - halfSize.height);
            CGPoint bottomLeft = new CGPoint(x: center.x - halfSize.width, y: center.y + halfSize.height);
            CGPoint bottomRight = new CGPoint(x: center.x + halfSize.width, y: center.y + halfSize.height);

            HashSet<CGPoint> points = new HashSet<CGPoint>();

            for (int x = (int)topLeft.x; x <= (int)topRight.x; x++) {
                points.Add(new CGPoint(x: x, y: topLeft.y));
                points.Add(new CGPoint(x: x, y: bottomLeft.y));
            }
            
            for (int y = (int)topLeft.y; y <= (int)bottomLeft.y; y++) {
                points.Add(new CGPoint(x: topLeft.x, y: y));
                points.Add(new CGPoint(x: topRight.x, y: y));
            }

            // Requires more resources
            //for (int x = (int)topLeft.x; x <= (int)topRight.x; x++) {
            //    for (int y = (int)topLeft.y; y <= (int)bottomLeft.y; y++) {
            //        points.Add(new CGPoint(x, y));
            //    }
            //}

            if (center.getDistanceTo(collider.center) <= halfSize.width) return true;
            if (center.getDistanceTo(collider.center) <= halfSize.height) return true;

            foreach (CGPoint point in points) {
                if (point.getDistanceTo(collider.center) <= collider.halfSize.width) return true;
            }

            return false;
        }

        public void update() {
            center = transform.position + halfSize.toPoint() + offset;
        }

    }

    public class GECircleCollider : GECollider {

        public CGPoint center { get; set; }
        public CGSize halfSize { get; set; }
        public CGPoint offset { get; set; }

        private GETransform transform;

        public GECircleCollider(GETransform transform) {
            this.transform = transform;
            halfSize = new CGSize(width: transform.scale.width / 2, height: transform.scale.width / 2);
            center = new CGPoint(x: transform.position.x + halfSize.width, y: transform.position.y + halfSize.height);
            offset = new CGPoint(x: 0, y: 0);
        }

        public GECircleCollider(GETransform transform, float diameter) {
            this.transform = transform;
            halfSize = new CGSize(width: diameter / 2, height: diameter / 2);
            center = new CGPoint(x: transform.position.x + halfSize.width, y: transform.position.y + halfSize.height);
            offset = new CGPoint(x: 0, y: 0);
        }

        public GECircleCollider(GETransform transform, CGPoint offset) {
            this.transform = transform;
            halfSize = new CGSize(width: transform.scale.width / 2, height: transform.scale.width / 2);
            center = new CGPoint(x: transform.position.x + halfSize.width, y: transform.position.y + halfSize.height);
            this.offset = offset;
        }

        public GECircleCollider(GETransform transform, float diameter, CGPoint offset) {
            this.transform = transform;
            halfSize = new CGSize(width: diameter / 2, height: diameter / 2);
            center = new CGPoint(x: transform.position.x + halfSize.width, y: transform.position.y + halfSize.height);
            this.offset = offset;
        }

        public bool isIntersectsWith(GERectCollider collider) {

            CGPoint topLeft = new CGPoint(x: collider.center.x - collider.halfSize.width, y: collider.center.y - collider.halfSize.height);
            CGPoint topRight = new CGPoint(x: collider.center.x + collider.halfSize.width, y: collider.center.y - collider.halfSize.height);
            CGPoint bottomLeft = new CGPoint(x: collider.center.x - collider.halfSize.width, y: collider.center.y + collider.halfSize.height);
            CGPoint bottomRight = new CGPoint(x: collider.center.x + collider.halfSize.width, y: collider.center.y + collider.halfSize.height);

            HashSet<CGPoint> points = new HashSet<CGPoint>();

            for (int x = (int)topLeft.x; x <= (int)topRight.x; x++) {
                points.Add(new CGPoint(x: x, y: topLeft.y));
                points.Add(new CGPoint(x: x, y: bottomLeft.y));
            }
            for (int y = (int)topLeft.y; y <= (int)bottomLeft.y; y++) {
                points.Add(new CGPoint(x: topLeft.x, y: y));
                points.Add(new CGPoint(x: topRight.x, y: y));
            }


            if (collider.center.getDistanceTo(center) <= collider.halfSize.width) return true;
            if (collider.center.getDistanceTo(center) <= collider.halfSize.height) return true;

            foreach (CGPoint point in points) {
                if (point.getDistanceTo(center) <= halfSize.width) return true;
            }

            return false;
        }

        public bool isIntersectsWith(GECircleCollider collider) {
            if (center.getDistanceTo(collider.center) <= (halfSize.width + collider.halfSize.width)) return true;
            return false;
        }

        public void update() {
            center = transform.position + halfSize.toPoint() + offset;
        }

    }

    public class GEBody {

        private GETransform transform;

        public CGPoint oldPosition;

        public CGVector oldSpeed;
        public CGVector speed;
        
        public CGSize scale;

        public bool isContactRightWall;
        public bool wasContactRightWall;

        public bool isContactLeftWall;
        public bool wasContactLeftWall;

        public bool isContactFloor;
        public bool wasContactFloor;

        public bool isContactCeiling;
        public bool wasContactCeiling;

        public GEBody(GETransform transform, CGVector speed) {
            this.transform = transform;
            this.speed = speed;

            isContactCeiling = false;
            isContactFloor = false;
            isContactLeftWall = false;
            isContactRightWall = false;

            update();
        }

        public void update() {

            oldPosition = transform.position;
            oldSpeed = speed;

            wasContactRightWall = isContactRightWall;
            wasContactLeftWall = isContactLeftWall;
            wasContactFloor = isContactFloor;
            wasContactCeiling = isContactCeiling;

            transform.position += speed;

    }

    }

    public class GERendering {

        protected GETransform transform;
        protected CGWindow window;
        protected RectangleShape rect;

        public int obj = 0;

        protected CircleShape circle;

        public CGColor backgroundColor;

        public GERendering(GETransform transform, CGWindow window) {
            this.transform = transform;
            this.window = window;

            circle = new CircleShape(radius: transform.scale.width / 2);
            rect = new RectangleShape(size: new Vector2f(transform.scale.width, transform.scale.height));

            setTransform();
            defaultsSettings();

            update();
        }

        private void defaultsSettings() {
            backgroundColor = new CGColor("FFFFFF", 255);
        }

        private void setTransform() {
            if (obj == 1) {
                circle.Position = new Vector2f(transform.position.x, transform.position.y);
            } else {
                rect.Size = new Vector2f(transform.scale.width, transform.scale.height);
                rect.Position = new Vector2f(x: transform.position.x, y: transform.position.y);
            }
        }

        public void update() {
            setTransform();
            rect.FillColor = backgroundColor.toSfmlColor();

            if (obj == 1) {
                window.draw(circle);
            } else {
                window.draw(rect);
            }
        }

    }

    public class GEObject {

        public GETransform transform = new GETransform(
            position: new CGPoint(x: 0, y: 0), 
            scale: new CGSize(width: 0, height: 0), 
            rotation: 0);

        public GERectCollider rectCollider;
        public GECircleCollider circleCollider;
        public GEBody body;
        public GERendering rendering;

        public GEObject() {}

        public void update() {
            ifNotNull(rectCollider, () => rectCollider.update());
            ifNotNull(circleCollider, () => circleCollider.update());
            ifNotNull(body, () => body.update());
            ifNotNull(rendering, () => rendering.update());
        }

        private void ifNotNull(object obj, Action func) {
            if (obj != null) {
                func();
            }
        }

    }

}
