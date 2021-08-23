using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.System;

namespace TDEngine {


    public interface GECollider {


        //public CGPoint offset { get; set; }

        public CGPoint farthestPointInDirection(CGVector direction);
        public bool isIntersectsWith(GEPolygonCollider collider);

        //public void update();

    }

    public class GETransform {

        public CGPoint position;

        public CGSize size;

        public float rotation;

        public GETransform(CGPoint position, CGSize scale, float rotation) {
            this.position = position;
            this.size = scale;
            this.rotation = rotation;
        }

    }

    public class GEPolygonCollider : GECollider {
        private CGPoint[] points;

        public GEPolygonCollider(CGPoint[] points) {
            this.points = points;
        }

        private class GESimplex {
            private List<CGPoint> points = new List<CGPoint>();

            public GESimplex() {}

            public void append(CGPoint point) {
                points.Add(point);
            }

            public object calculateDirection() {
                CGPoint a = points[points.Count - 1];

                CGVector ao = a.cgVector().inverted();

                if (points.Count == 3) {
                    CGPoint b = points[1];
                    CGPoint c = points[0];


                    CGPoint ab = b - a;
                    CGPoint ac = c - a;

                    CGVector abPerp = new CGVector(ab.y, -ab.x);

                    if (abPerp.getRelativeValueTo(c) >= 0) {
                        abPerp = abPerp.inverted();
                    }

                    if (abPerp.getRelativeValueTo(ao.cgPoint()) > 0) {
                        points.RemoveAt(0);
                    }

                    CGVector acPerp = new CGVector(ac.y, -ac.x);

                    if (acPerp.getRelativeValueTo(b) >= 0) {
                        acPerp = acPerp.inverted();
                    }

                    if (acPerp.getRelativeValueTo(ao.cgPoint()) > 0) {
                        points.RemoveAt(1);
                        return acPerp;
                    }

                    return null;
                }

                CGPoint _b = points[0];
                CGPoint _ab = _b - a;
                CGVector _abPerp = new CGVector(_ab.y, -_ab.x);

                if (_abPerp.getRelativeValueTo(ao.cgPoint()) <= 0) {
                    _abPerp = _abPerp.inverted();
                }

                return _abPerp;
            }
        
        }

        public CGPoint farthestPointInDirection(CGVector direction) {
            float farthestDistance = 0;
            CGPoint farthestPoint = new CGPoint(0, 0);

            foreach (CGPoint point in points) {
                float distanceInDirection = point.cgVector().getRelativeValueTo(direction.cgPoint());

                if (distanceInDirection > farthestDistance) {
                    farthestPoint = point;
                    farthestDistance = distanceInDirection;
                }
            }

            return farthestPoint;
        }

        public CGPoint support(GEPolygonCollider collider, CGVector direction) {
            CGPoint aFar = farthestPointInDirection(direction);
            CGPoint bFar = collider.farthestPointInDirection(direction);
            return aFar - bFar;
        }

        public bool isIntersectsWith(GEPolygonCollider collider) {
            GESimplex simplex = new GESimplex();

            CGVector direction = new CGVector(0, 1);

            CGPoint initSupportPoint = support(collider, direction);
            simplex.append(initSupportPoint);

            direction.invert();

            while (true) {
                CGPoint supportPoint = support(collider, direction);

                if (supportPoint.cgVector().getRelativeValueTo(direction.cgPoint()) <= 0) {
                    return false;
                }

                simplex.append(supportPoint);
                direction = (CGVector)simplex.calculateDirection();
            }

            return true;
        }

        public void update() {

        }
    }

    /*public class GERectCollider : GECollider {

        public CGPoint center { get; set; }
        public CGSize halfSize { get; set; }
        public CGPoint offset { get; set; }

        public bool isPreciseMode;

        private GETransform transform;

        public GERectCollider(GETransform transform) {
            this.transform = transform;
            halfSize = new CGSize(width: transform.size.width / 2, height: transform.size.height / 2);
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
            halfSize = new CGSize(width: transform.size.width / 2, height: transform.size.height / 2);
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
            if (isPreciseMode) {
                for (int x = (int)topLeft.x; x <= (int)topRight.x; x++) {
                    for (int y = (int)topLeft.y; y <= (int)bottomLeft.y; y++) {
                        points.Add(new CGPoint(x, y));
                    }
                }
            }

            foreach (CGPoint point in points) {
                if (point.getDistanceTo(collider.center) <= collider.halfSize.width) return true;
            }

            return false;
        }

        public void update() {
            center = transform.position + halfSize.toPoint() + offset;
        }

    }
    */

    /*public class GECircleCollider : GECollider {

        public CGPoint center { get; set; }
        public CGSize halfSize { get; set; }
        public CGPoint offset { get; set; }

        public bool isPreciseMode;

        private GETransform transform;

        public GECircleCollider(GETransform transform) {
            this.transform = transform;
            halfSize = new CGSize(width: transform.size.width / 2, height: transform.size.width / 2);
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
            halfSize = new CGSize(width: transform.size.width / 2, height: transform.size.width / 2);
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

            // Requires more resources
            if (isPreciseMode) {
                for (int x = (int)topLeft.x; x <= (int)topRight.x; x++) {
                    for (int y = (int)topLeft.y; y <= (int)bottomLeft.y; y++) {
                        points.Add(new CGPoint(x, y));
                    }
                }
            }

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
    */

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

        // Main fields
        protected GETransform transform;
        protected CGWindow window;
        public CGPoint offset = new CGPoint(0, 0);
        public GEBody body;

        // Shapes
        protected ConvexShape polygon;
        protected CircleShape circle;

        // Optional fields
        public CGColor backgroundColor;
        public CGColor outlineColor;
        public float outlineThickness;
        public CGPoint origin;

        public GERendering(GETransform transform, CGWindow window) {
            this.transform = transform;
            this.window = window;

            defineShape(GERenderingShape.Rectangle);

            defaultsSettings();
            setTransform();

            update();
        }

        public GERendering(GETransform transform, GERenderingShape initialShape, CGWindow window) {
            this.transform = transform;
            this.window = window;

            defineShape(initialShape);

            defaultsSettings();
            setTransform();

            update();
        }

        public GERendering(GETransform transform, GERenderingShape initialShape, CGPoint offset, CGWindow window) {
            this.transform = transform;
            this.offset = offset;
            this.window = window;

            defineShape(initialShape);

            defaultsSettings();
            setTransform();

            update();
        }

        public void defineShape(CGPoint[] vertexes) {
            circle = null;
            polygon = new ConvexShape((uint)vertexes.Length);
            for (uint i = 0; i < vertexes.Length; i++) {
                polygon.SetPoint(i, new Vector2f(vertexes[i].x, vertexes[i].y));
            }
        }

        public void defineShape(GERenderingShape shape) {
            if (shape == GERenderingShape.Rectangle) {
                circle = null;
                polygon = new ConvexShape(4);
                polygon.SetPoint(0, new Vector2f(0, 0));
                polygon.SetPoint(1, new Vector2f(0 + transform.size.width, 0));
                polygon.SetPoint(2, new Vector2f(0 + transform.size.width, 0 + transform.size.height));
                polygon.SetPoint(3, new Vector2f(0, 0 + transform.size.height));
            } else if (shape == GERenderingShape.Circle) {
                polygon = null;
                circle = new CircleShape(radius: transform.size.width / 2);
            }
            
        }

        private void defaultsSettings() {
            backgroundColor = new CGColor("FFFFFF", 255);
            outlineColor = new CGColor("000000", 255);
            outlineThickness = 0;
            if (circle != null) {
                origin = new CGPoint(x: transform.size.width / 2, y: transform.size.width / 2);
            }
            if (polygon != null) {
                origin = new CGPoint(x: transform.size.width / 2, y: transform.size.height / 2);
            }
        }

        private void setTransform() {

            if (circle != null) {
                circle.Radius = transform.size.width / 2;
                circle.Position = new Vector2f(transform.position.x + offset.x + origin.x, transform.position.y + offset.y + origin.y);  //  - origin.x
            }
            if (polygon != null) {
                polygon.Position = new Vector2f(transform.position.x + offset.x + origin.x, transform.position.y + offset.y + origin.y);
            }

        }

        public void update() {
            setTransform();

            if (circle != null) {
                circle.FillColor = backgroundColor.toSfmlColor();
                circle.OutlineColor = outlineColor.toSfmlColor();
                circle.OutlineThickness = outlineThickness;

                circle.Origin = new Vector2f(origin.x, origin.y);
                circle.Rotation = transform.rotation;

                window.draw(circle);
            }
            if (polygon != null) {
                polygon.FillColor = backgroundColor.toSfmlColor();
                polygon.OutlineColor = outlineColor.toSfmlColor();
                polygon.OutlineThickness = outlineThickness;

                polygon.Origin = new Vector2f(origin.x, origin.y);
                polygon.Rotation = transform.rotation;

                window.draw(polygon);
            }

        }

    }

    public class GEObject {

        public GETransform transform = new GETransform(
            position: new CGPoint(x: 0, y: 0), 
            scale: new CGSize(width: 0, height: 0), 
            rotation: 0);

        //public GERectCollider rectCollider;
        //public GECircleCollider circleCollider;
        public GEPolygonCollider polygonCollider;
        public GEBody body;
        public GERendering rendering;

        public GEObject() {}

        public void update() {
            //ifNotNull(rectCollider, () => rectCollider.update());
            //ifNotNull(circleCollider, () => circleCollider.update());
            ifNotNull(body, () => body.update());
            ifNotNull(rendering, () => rendering.update());
        }

        private void ifNotNull(object obj, Action func) {
            if (obj != null) {
                func();
            }
        }

    }

    public enum GERenderingShape {
        Rectangle,
        Circle
    }

}
