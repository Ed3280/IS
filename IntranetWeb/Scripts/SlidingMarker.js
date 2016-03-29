﻿/* SlidingMarker v0.2.8 19-02-2016 (C) 2015 Terikon Apps */

! function (a, b) {
    "use strict";
    "function" == typeof define && define.amd ? define(["jquery", "marker-animate"], b.bind(null, a)) : "undefined" != typeof module && null !== module && null != module.exports ? module.exports = b(a, require("jquery"), require("marker-animate")) : a.SlidingMarker = b(a, a.jQuery)
}(this, function (a, b) {
    "use strict";
    b = b || {}, b.extend = b.extend || function m(a) {
        return Array.prototype.slice.call(arguments, 1).forEach(function (b) {
            if (b)
                for (var c in b) b[c] && b[c].constructor === Object ? a[c] && a[c].constructor !== Object ? a[c] = b[c] : (a[c] = a[c] || {}, m(a[c], b[c])) : a[c] = b[c]
        }), a
    };
    var c, d = google.maps.Marker,
        e = function (a, b, c) {
            return null === a || void 0 === a ? void (window.cancelAnimationFrame ? window.cancelAnimationFrame(this.AT_animationHandler) : clearTimeout(this.AT_animationHandler)) : void google.maps.Marker.prototype.animateTo.apply(this, arguments)
        },
        f = {
            easing: "easeInOutQuint",
            duration: 1e3,
            animateFunctionAdapter: function (a, b, d, f) {
                if (!c) {
                    if (!google.maps.Marker.prototype.animateTo) throw new Error("Please either reference markerAnimate.js, or provide an alternative animateFunctionAdapter");
                    c = e
                }
                c.call(a, b, {
                    easing: d,
                    duration: f,
                    complete: function () { }
                })
            }
        },
        g = function (a, b) {
            function c() { }
            c.prototype = b.prototype, a.superClass_ = b.prototype, a.prototype = new c, a.prototype.constructor = a
        },
        h = function (a, c) {
            g(a, c);
            var d = c.prototype;
            b.extend(a.prototype, {
                _instance: null,
                originalSet: function () {
                    return d.set.apply(this, arguments)
                },
                set: function (a, b) {
                    var c = this;
                    c.originalSet.apply(c, arguments), "position" === a && c instanceof l ? c._setInstancePositionAnimated(b) : c.originalSet.apply(c._instance, arguments)
                },
                _setInstancePositionAnimated: function (a) {
                    var b = this;
                    if (!b._constructing) return a && b._instance.getPosition() ? void b.get("animateFunctionAdapter").call(null, b._instance, a, b.get("easing"), b.get("duration")) : void (b._instance.getPosition() !== a && b._instance.setPosition(a))
                },
                originalAddListener: function () {
                    return d.addListener.apply(this, arguments)
                },
                addListener: function (a, b) {
                    var c = "map_changed" === a ? this._instance : i.call(this, a);
                    return this.originalAddListener.apply(c, arguments)
                },
                map_changed: function () { },
                position_changed: function () {
                    this._suppress_animation ? delete this._suppress_animation : this._setInstancePositionAnimated(this.getPosition())
                }
            })
        },
        i = function (a) {
            return a.endsWith("_changed") ? this : this._instance
        },
        j = google.maps.event.addListener;
    google.maps.event.addListener = function (a, b, c) {
        if (a instanceof l) {
            var d = "map_changed" === b ? a._instance : i.call(a, b);
            return j.call(this, d, b, c)
        }
        return j.apply(this, arguments)
    };
    var k = google.maps.event.trigger;
    google.maps.event.trigger = function (a, b) {
        if (a instanceof l) {
            var c = "map_changed" === b ? a : i.call(a, b),
                d = [c].concat(Array.prototype.slice.call(arguments, 1));
            return k.apply(this, d)
        }
        return k.apply(this, arguments)
    }, String.prototype.endsWith = String.prototype.endsWith || function (a) {
        return -1 !== this.indexOf(a, this.length - a.length)
    };
    var l = function (a) {
        a = b.extend({}, f, a), this._instance = new d(a), this.animationPosition = null, this._constructing = !0, d.call(this, a), delete this._constructing, this.bindTo("animationPosition", this._instance, "position"), this.bindTo("anchorPoint", this._instance, "anchorPoint"), this.bindTo("internalPosition", this._instance, "internalPosition")
    };
    return h(l, d), b.extend(l.prototype, {
        getAnimationPosition: function () {
            return this.get("animationPosition")
        },
        setPositionNotAnimated: function (a) {
            this._suppress_animation = !0, this.get("animateFunctionAdapter").call(null, this._instance, null), this.originalSet("position", a), this._instance.setPosition(a)
        },
        setDuration: function (a) {
            this.set("duration", a)
        },
        getDuration: function () {
            return this.get("duration")
        },
        setEasing: function (a) {
            this.set("easing", a)
        },
        getEasing: function () {
            return this.get("easing")
        }
    }), l.initializeGlobally = function () {
        google.maps.Marker = l
    }, l
});
//# sourceMappingURL=SlidingMarker.min.js.map