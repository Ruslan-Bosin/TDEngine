﻿using System;
using System.Collections.Generic;
using System.Text;

using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Linq;

namespace TDEngine {

    public interface GECollider {

        public CGPoint center { get; set; }
        public CGSize halfSize { get; set; }
        public CGPoint offset { get; set; }

        public bool isIntersectsWith(GERectCollider collider);
        public bool isIntersectsWith(GECircleCollider collider);

        public void update(GETransform transform);

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

        public GERectCollider(CGPoint position, CGSize size) {
            halfSize = new CGSize(width: size.width / 2, height: size.height / 2);
            center = new CGPoint(x: position.x + halfSize.width, y: position.y + halfSize.height);
            offset = new CGPoint(x: 0, y: 0);
        }

        public GERectCollider(CGPoint position, CGSize size, CGPoint offset) {
            halfSize = new CGSize(width: size.width / 2, height: size.height / 2);
            center = new CGPoint(x: position.x + halfSize.width, y: position.y + halfSize.height);
            this.offset = offset;
        }

        public bool isIntersectsWith(GERectCollider collider) {
            if (Math.Abs(center.x - collider.center.x) > halfSize.width + collider.halfSize.width) return false;
            if (Math.Abs(center.y - collider.center.y) > halfSize.height + collider.halfSize.height) return false;
            return true;
        }

        public bool isIntersectsWith(GECircleCollider collider) {

            throw new Exception("EmptyFunction");

        }

        public void update(GETransform transform) {
            center = transform.position + halfSize.toPoint() + offset;
        }

    }

    public class GECircleCollider : GECollider {

        public CGPoint center { get; set; }
        public CGSize halfSize { get; set; }
        public CGPoint offset { get; set; }

        public GECircleCollider(CGPoint position, float diameter) {
            halfSize = new CGSize(width: diameter / 2, height: diameter / 2);
            center = new CGPoint(x: position.x + halfSize.width, y: position.y + halfSize.height);
            offset = new CGPoint(x: 0, y: 0);
        }

        public GECircleCollider(CGPoint position, float diameter, CGPoint offset) {
            halfSize = new CGSize(width: diameter / 2, height: diameter / 2);
            center = new CGPoint(x: position.x + halfSize.width, y: position.y + halfSize.height);
            this.offset = offset;
        }

        public bool isIntersectsWith(GERectCollider collider) {
            throw new Exception("EmptyFunction");
        }

        public bool isIntersectsWith(GECircleCollider collider) {
            if (center.getDistanceTo(collider.center) <= (halfSize.width + collider.halfSize.width)) return true;
            return false;
        }

        public void update(GETransform transform) {
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
        //protected RectangleShape rect;

        protected CircleShape rect;

        public CGColor backgroundColor;

        public GERendering(GETransform transform, CGWindow window) {
            this.transform = transform;
            this.window = window;
            //rect = new RectangleShape(size: new Vector2f(transform.scale.width, transform.scale.height));
            rect = new CircleShape(radius: transform.scale.width / 2);
            setTransform();
            defaultsSettings();

            update();
        }

        private void defaultsSettings() {
            backgroundColor = new CGColor("FFFFFF", 255);
    }

        private void setTransform() {
            //rect.Size = new Vector2f(transform.scale.width, transform.scale.height);
            rect.Position = new Vector2f(x: transform.position.x, y: transform.position.y);
        }

        public void update() {
            setTransform();
            rect.FillColor = backgroundColor.toSfmlColor();
            window.draw(rect);
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
            ifNotNull(rectCollider, () => rectCollider.update(transform));
            ifNotNull(circleCollider, () => circleCollider.update(transform));
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
