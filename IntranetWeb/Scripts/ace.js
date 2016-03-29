﻿/*!
 * Ace v1.3.3
 */
if ("undefined" == typeof jQuery) throw new Error("Ace's JavaScript requires jQuery");
! function () {
    "ace" in window || (window.ace = {}), "helper" in window.ace || (window.ace.helper = {}), "vars" in window.ace || (window.ace.vars = {}), window.ace.vars.icon = " ace-icon ", window.ace.vars[".icon"] = ".ace-icon", ace.vars.touch = "ontouchstart" in window;
    var a = navigator.userAgent;
    ace.vars.webkit = !!a.match(/AppleWebKit/i), ace.vars.safari = !!a.match(/Safari/i) && !a.match(/Chrome/i), ace.vars.android = ace.vars.safari && !!a.match(/Android/i), ace.vars.ios_safari = !!a.match(/OS ([4-9])(_\d)+ like Mac OS X/i) && !a.match(/CriOS/i), ace.vars.ie = window.navigator.msPointerEnabled || document.all && document.querySelector, ace.vars.old_ie = document.all && !document.addEventListener, ace.vars.very_old_ie = document.all && !document.querySelector, ace.vars.firefox = "MozAppearance" in document.documentElement.style, ace.vars.non_auto_fixed = ace.vars.android || ace.vars.ios_safari
}(),
function (a) {
    ace.click_event = ace.vars.touch && a.fn.tap ? "tap" : "click"
}(jQuery), jQuery(function (a) {
    function b() {
        ace.vars.non_auto_fixed && a("body").addClass("mob-safari"), ace.vars.transition = !!a.support.transition.end
    }

    function c() {
        var b = a(".sidebar");
        a.fn.ace_sidebar && b.ace_sidebar(), a.fn.ace_sidebar_scroll && b.ace_sidebar_scroll({
            include_toggle: !1 || ace.vars.safari || ace.vars.ios_safari
        }), a.fn.ace_sidebar_hover && b.ace_sidebar_hover({
            sub_hover_delay: 750,
            sub_scroll_style: "no-track scroll-thin scroll-margin scroll-visible"
        })
    }

    function d() {
        if (a.fn.ace_ajax) {
            window.Pace && (window.paceOptions = {
                ajax: !0,
                document: !0,
                eventLag: !1
            });
            var b = {
                close_active: !0,
                default_url: "page/index",
                content_url: function (a) {
                    if (!a.match(/^page\//)) return !1;
                    var b = document.location.pathname;
                    return b.match(/(\/ajax\/)(index\.html)?/) ? b.replace(/(\/ajax\/)(index\.html)?/, "/ajax/content/" + a.replace(/^page\//, "") + ".html") : b + "?" + a.replace(/\//, "=")
                }
            };
            window.Pace && (b.loading_overlay = "body"), a("[data-ajax-content=true]").ace_ajax(b), a(window).on("error.ace_ajax", function () {
                a("[data-ajax-content=true]").each(function () {
                    var b = a(this);
                    b.ace_ajax("working") && (window.Pace && Pace.running && Pace.stop(), b.ace_ajax("stopLoading", !0))
                })
            })
        }
    }

    function e() {
        var b = !!a.fn.ace_scroll;
        b && a(".dropdown-content").ace_scroll({
            reset: !1,
            mouseWheelLock: !0
        }), b && !ace.vars.old_ie && (a(window).on("resize.reset_scroll", function () {
            a(".ace-scroll:not(.scroll-disabled)").not(":hidden").ace_scroll("reset")
        }), b && a(document).on("settings.ace.reset_scroll", function (b, c) {
            "sidebar_collapsed" == c && a(".ace-scroll:not(.scroll-disabled)").not(":hidden").ace_scroll("reset")
        }))
    }

    function f() {
        a(document).on("click.dropdown.pos", '.dropdown-toggle[data-position="auto"]', function () {
            var b = a(this).offset(),
                c = a(this.parentNode);
            parseInt(b.top + a(this).height()) + 50 > ace.helper.scrollTop() + ace.helper.winHeight() - c.find(".dropdown-menu").eq(0).height() ? c.addClass("dropup") : c.removeClass("dropup")
        })
    }

    function g() {
        a('.ace-nav [class*="icon-animated-"]').closest("a").one("click", function () {
            var b = a(this).find('[class*="icon-animated-"]').eq(0),
                c = b.attr("class").match(/icon\-animated\-([\d\w]+)/);
            b.removeClass(c[0])
        }), a(document).on("click", ".dropdown-navbar .nav-tabs", function (b) {
            b.stopPropagation(); {
                var c;
                b.target
            } (c = a(b.target).closest("[data-toggle=tab]")) && c.length > 0 && (c.tab("show"), b.preventDefault(), a(window).triggerHandler("resize.navbar.dropdown"))
        })
    }

    function h() {
        a(".sidebar .nav-list .badge[title],.sidebar .nav-list .badge[title]").each(function () {
            var b = a(this).attr("class").match(/tooltip\-(?:\w+)/);
            b = b ? b[0] : "tooltip-error", a(this).tooltip({
                placement: function (b, c) {
                    var d = a(c).offset();
                    return parseInt(d.left) < parseInt(document.body.scrollWidth / 2) ? "right" : "left"
                },
                container: "body",
                template: '<div class="tooltip ' + b + '"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>'
            })
        })
    }

    function i() {
        var b = a(".btn-scroll-up");
        if (b.length > 0) {
            var c = !1;
            a(window).on("scroll.scroll_btn", function () {
                var a = ace.helper.scrollTop(),
                    d = ace.helper.winHeight(),
                    e = document.body.scrollHeight;
                a > parseInt(d / 4) || a > 0 && e >= d && d + a >= e - 1 ? c || (b.addClass("display"), c = !0) : c && (b.removeClass("display"), c = !1)
            }).triggerHandler("scroll.scroll_btn"), b.on(ace.click_event, function () {
                var b = Math.min(500, Math.max(100, parseInt(ace.helper.scrollTop() / 3)));
                return a("html,body").animate({
                    scrollTop: 0
                }, b), !1
            })
        }
    }

    function j() {
        if (ace.vars.webkit) {
            var b = a(".ace-nav").get(0);
            b && a(window).on("resize.webkit_fix", function () {
                ace.helper.redraw(b)
            })
        }
        ace.vars.ios_safari && a(document).on("ace.settings.ios_fix", function (b, c, d) {
            "navbar_fixed" == c && (a(document).off("focus.ios_fix blur.ios_fix", "input,textarea,.wysiwyg-editor"), 1 == d && a(document).on("focus.ios_fix", "input,textarea,.wysiwyg-editor", function () {
                a(window).on("scroll.ios_fix", function () {
                    var b = a("#navbar").get(0);
                    b && ace.helper.redraw(b)
                })
            }).on("blur.ios_fix", "input,textarea,.wysiwyg-editor", function () {
                a(window).off("scroll.ios_fix")
            }))
        }).triggerHandler("ace.settings.ios_fix", ["navbar_fixed", "fixed" == a("#navbar").css("position")])
    }

    function k() {
        a(document).on("hide.bs.collapse show.bs.collapse", function (b) {
            var c = b.target.getAttribute("id"),
                d = a('a[href*="#' + c + '"]');
            0 == d.length && (d = a('a[data-target*="#' + c + '"]')), 0 != d.length && d.find(ace.vars[".icon"]).each(function () {
                var c, d = a(this),
                    e = null,
                    f = null;
                return (e = d.attr("data-icon-show")) ? f = d.attr("data-icon-hide") : (c = d.attr("class").match(/fa\-(.*)\-(up|down)/)) && (e = "fa-" + c[1] + "-down", f = "fa-" + c[1] + "-up"), e ? ("show" == b.type ? d.removeClass(e).addClass(f) : d.removeClass(f).addClass(e), !1) : void 0
            })
        })
    }

    function l() {
        function b() {
            var b = a(this).find("> .dropdown-menu");
            if ("fixed" == b.css("position")) {
                var d = parseInt(a(window).width()),
                    e = d > 320 ? 60 : d > 240 ? 40 : 30,
                    f = parseInt(d) - e,
                    g = parseInt(a(window).height()) - 30,
                    h = parseInt(Math.min(f, 320));
                b.css("width", h);
                var i = !1,
                    j = 0,
                    k = b.find(".tab-pane.active .dropdown-content.ace-scroll");
                0 == k.length ? k = b.find(".dropdown-content.ace-scroll") : i = !0;
                var l = k.closest(".dropdown-menu"),
                    m = b[0].scrollHeight;
                if (1 == k.length) {
                    var n = k.find(".scroll-content")[0];
                    n && (m = n.scrollHeight), j += l.find(".dropdown-header").outerHeight(), j += l.find(".dropdown-footer").outerHeight();
                    var o = l.closest(".tab-content");
                    0 != o.length && (j += o.siblings(".nav-tabs").eq(0).height())
                }
                var p = parseInt(Math.min(g, 480, m + j)),
                    q = parseInt(Math.abs((f + e - h) / 2)),
                    r = parseInt(Math.abs((g + 30 - p) / 2)),
                    s = parseInt(b.css("z-index")) || 0;
                if (b.css({
                    height: p,
                    left: q,
                    right: "auto",
                    top: r - (i ? 3 : 1)
                }), 1 == k.length && (ace.vars.touch ? k.ace_scroll("disable").css("max-height", p - j).addClass("overflow-scroll") : k.ace_scroll("update", {
                    size: p - j
                }).ace_scroll("enable").ace_scroll("reset")), b.css("height", p + (i ? 7 : 2)), b.hasClass("user-menu")) {
                    b.css("height", "");
                    var t = a(this).find(".user-info");
                    t.css(1 == t.length && "fixed" == t.css("position") ? {
                        left: q,
                        right: "auto",
                        top: r,
                        width: h - 2,
                        "max-width": h - 2,
                        "z-index": s + 1
                    } : {
                        left: "",
                        right: "",
                        top: "",
                        width: "",
                        "max-width": "",
                        "z-index": ""
                    })
                }
                a(this).closest(".navbar.navbar-fixed-top").css("z-index", s)
            } else 0 != b.length && c.call(this, b);
            var u = this;
            a(window).off("resize.navbar.dropdown").one("resize.navbar.dropdown", function () {
                a(u).triggerHandler("shown.bs.dropdown.navbar")
            })
        }

        function c(b) {
            if (b = b || a(this).find("> .dropdown-menu"), b.length > 0 && (b.css({
                width: "",
                height: "",
                left: "",
                right: "",
                top: ""
            }).find(".dropdown-content").each(function () {
                ace.vars.touch && a(this).css("max-height", "").removeClass("overflow-scroll");
                var b = parseInt(a(this).attr("data-size") || 0) || a.fn.ace_scroll.defaults.size;
                a(this).ace_scroll("update", {
                size: b
            }).ace_scroll("enable").ace_scroll("reset")
            }), b.hasClass("user-menu"))) {
                a(this).find(".user-info").css({
                    left: "",
                    right: "",
                    top: "",
                    width: "",
                    "max-width": "",
                    "z-index": ""
                })
            }
            a(this).closest(".navbar").css("z-index", "")
        }
        ace.vars.old_ie || a(".ace-nav > li").on("shown.bs.dropdown.navbar", function () {
            b.call(this)
        }).on("hidden.bs.dropdown.navbar", function () {
            a(window).off("resize.navbar.dropdown"), c.call(this)
        })
    }
    b(), c(), d(), e(), f(), g(), h(), i(), j(), k(), l()
}),
function (a) {
    a.unCamelCase = function (a) {
        return a.replace(/([a-z])([A-Z])/g, function (a, b, c) {
            return b + "-" + c.toLowerCase()
        })
    }, a.strToVal = function (a) {
        var b = a.match(/^(?:(true)|(false)|(null)|(\-?[\d]+(?:\.[\d]+)?)|(\[.*\]|\{.*\}))$/i),
            c = a;
        if (b)
            if (b[1]) c = !0;
            else if (b[2]) c = !1;
            else if (b[3]) c = null;
            else if (b[4]) c = parseFloat(a);
            else if (b[5]) try {
                c = JSON.parse(a)
            } catch (d) { }
        return c
    }, a.getAttrSettings = function (b, c, d) {
        var e = c instanceof Array ? 1 : 2,
            d = d ? d.replace(/([^\-])$/, "$1-") : "";
        d = "data-" + d;
        var f = {};
        for (var g in c)
            if (c.hasOwnProperty(g)) {
                var h, i = 1 == e ? c[g] : g,
                    j = a.unCamelCase(i.replace(/[^A-Za-z0-9]{1,}/g, "-")).toLowerCase();
                if (!(h = b.getAttribute(d + j))) continue;
                f[i] = a.strToVal(h)
            }
        return f
    }, a.scrollTop = function () {
        return document.scrollTop || document.documentElement.scrollTop || document.body.scrollTop
    }, a.winHeight = function () {
        return window.innerHeight || document.documentElement.clientHeight
    }, a.redraw = function (a, b) {
        var c = a.style.display;
        a.style.display = "none", a.offsetHeight, b !== !0 ? a.style.display = c : setTimeout(function () {
            a.style.display = c
        }, 10)
    }
}(ace.helper),
function (a, b) {
    function c(b, c) {
        function e(b) {
            var c = "",
                d = a(".breadcrumb");
            if (d.length > 0 && d.is(":visible")) {
                d.find("> li:not(:first-child)").remove();
                var e = 0;
                b.parents(".nav li").each(function () {
                    var b = a(this).find("> a"),
                        f = b.clone();
                    f.find("i,.fa,.glyphicon,.ace-icon,.menu-icon,.badge,.label").remove();
                    var g = f.text();
                    f.remove();
                    var h = b.attr("href");
                    if (0 == e) {
                        var i = a('<li class="active"></li>').appendTo(d);
                        i.text(g), c = g
                    } else {
                        var i = a("<li><a /></li>").insertAfter(d.find("> li:first-child"));
                        i.find("a").attr("href", h).text(g)
                    }
                    e++
                })
            }
            return c
        }

        function f(b) {
            var c = g.find(".ajax-append-title");
            if (c.length > 0) document.title = c.text(), c.remove();
            else if (b.length > 0) {
                var d = a.trim(String(document.title).replace(/^(.*)[\-]/, ""));
                d && (d = " - " + d), b = a.trim(b) + d
            }
        }
        var g = a(b),
            h = this;
        g.attr("data-ajax-content", "true");
        var i = ace.helper.getAttrSettings(b, a.fn.ace_ajax.defaults);
        this.settings = a.extend({}, a.fn.ace_ajax.defaults, c, i);
        var j = !1,
            k = a();
        this.force_reload = !1, this.loadUrl = function (a, b) {
            var c = !1;
            a = a.replace(/^(\#\!)?\#/, ""), this.force_reload = b === !1, "function" == typeof this.settings.content_url && (c = this.settings.content_url(a)), "string" == typeof c && this.getUrl(c, a, !1)
        }, this.loadAddr = function (a, b, c) {
            this.force_reload = c === !1, this.getUrl(a, b, !1)
        }, this.getUrl = function (b, c, d) {
            if (!j) {
                var i;
                g.trigger(i = a.Event("ajaxloadstart"), {
                    url: b,
                    hash: c
                }), i.isDefaultPrevented() || (h.startLoading(), a.ajax({
                    url: b,
                    cache: !this.force_reload
                }).error(function () {
                    g.trigger("ajaxloaderror", {
                        url: b,
                        hash: c
                    }), h.stopLoading(!0)
                }).done(function (i) {
                    g.trigger("ajaxloaddone", {
                        url: b,
                        hash: c
                    });
                    var j = null,
                        l = "";
                    if ("function" == typeof h.settings.update_active) j = h.settings.update_active.call(null, c, b);
                    else if (h.settings.update_active === !0 && c && (j = a('a[data-url="' + c + '"]'), j.length > 0)) {
                        var m = j.closest(".nav");
                        if (m.length > 0) {
                            m.find(".active").each(function () {
                                var b = "active";
                                (a(this).hasClass("hover") || h.settings.close_active) && (b += " open"), a(this).removeClass(b), h.settings.close_active && a(this).find(" > .submenu").css("display", "")
                            }); {
                                j.closest("li").addClass("active").parents(".nav li").addClass("active open")
                            }
                            m.closest(".sidebar[data-sidebar-scroll=true]").each(function () {
                                var b = a(this);
                                b.ace_sidebar_scroll("reset"), d && b.ace_sidebar_scroll("scroll_to_active")
                            })
                        }
                    }
                    "function" == typeof h.settings.update_breadcrumbs ? l = h.settings.update_breadcrumbs.call(null, c, b, j) : h.settings.update_breadcrumbs === !0 && null != j && j.length > 0 && (l = e(j)), i = String(i).replace(/<(title|link)([\s\>])/gi, '<div class="hidden ajax-append-$1"$2').replace(/<\/(title|link)\>/gi, "</div>"), k.addClass("content-loaded").detach(), g.empty().html(i), a(h.settings.loading_overlay || g).append(k), setTimeout(function () {
                        a("head").find("link.ace-ajax-stylesheet").remove();
                        for (var b = ["link.ace-main-stylesheet", "link#main-ace-style", 'link[href*="/ace.min.css"]', 'link[href*="/ace.css"]'], c = [], d = 0; d < b.length && (c = a("head").find(b[d]).first(), !(c.length > 0)) ; d++);
                        g.find(".ajax-append-link").each(function () {
                            var b = a(this);
                            if (b.attr("href")) {
                                var d = jQuery("<link />", {
                                    type: "text/css",
                                    rel: "stylesheet",
                                    "class": "ace-ajax-stylesheet"
                                });
                                c.length > 0 ? d.insertBefore(c) : d.appendTo("head"), d.attr("href", b.attr("href"))
                            }
                            b.remove()
                        })
                    }, 10), "function" == typeof h.settings.update_title ? h.settings.update_title.call(null, c, b, l) : h.settings.update_title === !0 && f(l), d || a("html,body").animate({
                        scrollTop: 0
                    }, 250), g.trigger("ajaxloadcomplete", {
                        url: b,
                        hash: c
                    }), h.stopLoading()
                }))
            }
        };
        var l = !1,
            m = null;
        this.startLoading = function () {
            j || (j = !0, this.settings.loading_overlay || "static" != g.css("position") || (g.css("position", "relative"), l = !0), k.remove(), k = a('<div class="ajax-loading-overlay"><i class="ajax-loading-icon ' + (this.settings.loading_icon || "") + '"></i> ' + this.settings.loading_text + "</div>"), "body" == this.settings.loading_overlay ? a("body").append(k.addClass("ajax-overlay-body")) : this.settings.loading_overlay ? a(this.settings.loading_overlay).append(k) : g.append(k), this.settings.max_load_wait !== !1 && (m = setTimeout(function () {
                if (m = null, j) {
                    var b;
                    g.trigger(b = a.Event("ajaxloadlong")), b.isDefaultPrevented() || h.stopLoading(!0)
                }
            }, 1e3 * this.settings.max_load_wait)))
        }, this.stopLoading = function (a) {
            a === !0 ? (j = !1, k.remove(), l && (g.css("position", ""), l = !1), null != m && (clearTimeout(m), m = null)) : (k.addClass("almost-loaded"), g.one("ajaxscriptsloaded.inner_call", function () {
                h.stopLoading(!0)
            }))
        }, this.working = function () {
            return j
        }, this.loadScripts = function (b, c) {
            a.ajaxPrefilter("script", function (a) {
                a.cache = !0
            }), setTimeout(function () {
                function e() {
                    "function" == typeof c && c(), a('.btn-group[data-toggle="buttons"] > .btn').button(), g.trigger("ajaxscriptsloaded")
                }

                function f(a) {
                    a += 1, a < b.length ? h(a) : e()
                }

                function h(c) {
                    if (c = c || 0, !b[c]) return f(c);
                    var g = "js-" + b[c].replace(/[^\w\d\-]/g, "-").replace(/\-\-/g, "-");
                    d[g] !== !0 ? a.getScript(b[c]).done(function () {
                        d[g] = !0
                    }).complete(function () {
                        k++, k >= i && j ? e() : f(c)
                    }) : f(c)
                }
                for (var i = 0, k = 0, l = 0; l < b.length; l++) b[l] && ! function () {
                    var a = "js-" + b[l].replace(/[^\w\d\-]/g, "-").replace(/\-\-/g, "-");
                    d[a] !== !0 && i++
                }();
                i > 0 ? h() : e()
            }, 10)
        }, a(window).off("hashchange.ace_ajax").on("hashchange.ace_ajax", function () {
            var b = a.trim(window.location.hash);
            b && 0 != b.length && h.loadUrl(b)
        }).trigger("hashchange.ace_ajax", [!0]);
        var n = a.trim(window.location.hash);
        !n && this.settings.default_url && (window.location.hash = this.settings.default_url)
    }
    var d = {};
    a.fn.aceAjax = a.fn.ace_ajax = function (d, e, f, g) {
        var h, i = this.each(function () {
            var i = a(this),
                j = i.data("ace_ajax"),
                k = "object" == typeof d && d;
            j || i.data("ace_ajax", j = new c(this, k)), "string" == typeof d && "function" == typeof j[d] && (h = g != b ? j[d](e, f, g) : f != b ? j[d](e, f) : j[d](e))
        });
        return h === b ? i : h
    }, a.fn.aceAjax.defaults = a.fn.ace_ajax.defaults = {
        content_url: !1,
        default_url: !1,
        loading_icon: "fa fa-spin fa-spinner fa-2x orange",
        loading_text: "",
        loading_overlay: null,
        update_breadcrumbs: !0,
        update_title: !0,
        update_active: !0,
        close_active: !1,
        max_load_wait: !1
    }
}(window.jQuery),
function (a, b) {
    if (ace.vars.touch) {
        var c = "touchstart MSPointerDown pointerdown",
            d = "touchend touchcancel MSPointerUp MSPointerCancel pointerup pointercancel",
            e = "touchmove MSPointerMove MSPointerHover pointermove";
        a.event.special.ace_drag = {
            setup: function () {
                var f = 0,
                    g = a(this);
                g.on(c, function (c) {
                    function h(a) {
                        if (k) {
                            var b = a.originalEvent.touches ? a.originalEvent.touches[0] : a;
                            if (i = {
                                coords: [b.pageX, b.pageY]
                            }, k && i && (m = 0, n = 0, l = Math.abs(n = k.coords[1] - i.coords[1]) > f && Math.abs(m = k.coords[0] - i.coords[0]) <= Math.abs(n) ? n > 0 ? "up" : "down" : Math.abs(m = k.coords[0] - i.coords[0]) > f && Math.abs(n) <= Math.abs(m) ? m > 0 ? "left" : "right" : !1, l !== !1)) {
                                var c = {
                                    cancel: !1
                                };
                                k.origin.trigger({
                                    type: "ace_drag",
                                    direction: l,
                                    dx: m,
                                    dy: n,
                                    retval: c
                                }), 0 == c.cancel && a.preventDefault()
                            }
                            k.coords[0] = i.coords[0], k.coords[1] = i.coords[1]
                        }
                    }
                    var i, j = c.originalEvent.touches ? c.originalEvent.touches[0] : c,
                        k = {
                            coords: [j.pageX, j.pageY],
                            origin: a(c.target)
                        }, l = !1,
                        m = 0,
                        n = 0;
                    g.on(e, h).one(d, function () {
                        g.off(e, h), k = i = b
                    })
                })
            }
        }
    }
}(window.jQuery),
function (a, b) {
    function c(c, e) {
        function f() {
            this.mobile_view = this.mobile_style < 4 && this.is_mobile_view(), this.collapsible = !this.mobile_view && this.is_collapsible(), this.minimized = !this.collapsible && this.$sidebar.hasClass(l) || 3 == this.mobile_style && this.mobile_view && this.$sidebar.hasClass(m), this.horizontal = !(this.mobile_view || this.collapsible) && this.$sidebar.hasClass(n)
        }
        var g = this;
        this.$sidebar = a(c), this.$sidebar.attr("data-sidebar", "true"), this.$sidebar.attr("id") || this.$sidebar.attr("id", "id-sidebar-" + ++d);
        var h = ace.helper.getAttrSettings(c, a.fn.ace_sidebar.defaults, "sidebar-");
        this.settings = a.extend({}, a.fn.ace_sidebar.defaults, e, h), this.minimized = !1, this.collapsible = !1, this.horizontal = !1, this.mobile_view = !1, this.vars = function () {
            return {
                minimized: this.minimized,
                collapsible: this.collapsible,
                horizontal: this.horizontal,
                mobile_view: this.mobile_view
            }
        }, this.get = function (a) {
            return this.hasOwnProperty(a) ? this[a] : void 0
        }, this.set = function (a, b) {
            this.hasOwnProperty(a) && (this[a] = b)
        }, this.ref = function () {
            return this
        };
        var i = function (c) {
            var d, e, f = a(this).find(ace.vars[".icon"]);
            f.length > 0 && (d = f.attr("data-icon1"), e = f.attr("data-icon2"), c !== b ? c ? f.removeClass(d).addClass(e) : f.removeClass(e).addClass(d) : f.toggleClass(d).toggleClass(e))
        }, j = function () {
            var b = g.$sidebar.find(".sidebar-collapse");
            return 0 == b.length && (b = a('.sidebar-collapse[data-target="#' + (g.$sidebar.attr("id") || "") + '"]')), b = 0 != b.length ? b[0] : null
        };
        this.toggleMenu = function (a, b) {
            if (!this.collapsible) {
                this.minimized = !this.minimized;
                try {
                    ace.settings.sidebar_collapsed(c, this.minimized, !(a === !1 || b === !1))
                } catch (d) {
                    this.minimized ? this.$sidebar.addClass("menu-min") : this.$sidebar.removeClass("menu-min")
                }
                a || (a = j()), a && i.call(a, this.minimized), ace.vars.old_ie && ace.helper.redraw(c)
            }
        }, this.collapse = function (a, b) {
            this.collapsible || (this.minimized = !1, this.toggleMenu(a, b))
        }, this.expand = function (a, b) {
            this.collapsible || (this.minimized = !0, this.toggleMenu(a, b))
        }, this.toggleResponsive = function (b) {
            if (this.mobile_view && 3 == this.mobile_style) {
                if (this.$sidebar.hasClass("menu-min")) {
                    this.$sidebar.removeClass("menu-min");
                    var c = j();
                    c && i.call(c)
                }
                if (this.minimized = !this.$sidebar.hasClass("responsive-min"), this.$sidebar.toggleClass("responsive-min responsive-max"), b || (b = this.$sidebar.find(".sidebar-expand"), 0 == b.length && (b = a('.sidebar-expand[data-target="#' + (this.$sidebar.attr("id") || "") + '"]')), b = 0 != b.length ? b[0] : null), b) {
                    var d, e, f = a(b).find(ace.vars[".icon"]);
                    f.length > 0 && (d = f.attr("data-icon1"), e = f.attr("data-icon2"), f.toggleClass(d).toggleClass(e))
                }
                a(document).triggerHandler("settings.ace", ["sidebar_collapsed", this.minimized])
            }
        }, this.is_collapsible = function () {
            var b;
            return this.$sidebar.hasClass("navbar-collapse") && null != (b = a('.navbar-toggle[data-target="#' + (this.$sidebar.attr("id") || "") + '"]').get(0)) && b.scrollHeight > 0
        }, this.is_mobile_view = function () {
            var b;
            return null != (b = a('.menu-toggler[data-target="#' + (this.$sidebar.attr("id") || "") + '"]').get(0)) && b.scrollHeight > 0
        }, this.$sidebar.on(ace.click_event + ".ace.submenu", ".nav-list", function (b) {
            var c = this,
                d = a(b.target).closest("a");
            if (d && 0 != d.length) {
                var e = g.minimized && !g.collapsible;
                if (d.hasClass("dropdown-toggle")) {
                    b.preventDefault();
                    var f = d.siblings(".submenu").get(0);
                    if (!f) return !1;
                    var h = a(f),
                        i = 0,
                        j = f.parentNode.parentNode;
                    if (e && j == c || h.parent().hasClass("hover") && "absolute" == h.css("position") && !g.collapsible) return !1;
                    var k = 0 == f.scrollHeight;
                    return k && a(j).find("> .open > .submenu").each(function () {
                        this == f || a(this.parentNode).hasClass("active") || (i -= this.scrollHeight, g.hide(this, g.settings.duration, !1))
                    }), k ? (g.show(f, g.settings.duration), 0 != i && (i += f.scrollHeight)) : (g.hide(f, g.settings.duration), i -= f.scrollHeight), 0 != i && ("true" != g.$sidebar.attr("data-sidebar-scroll") || g.minimized || g.$sidebar.ace_sidebar_scroll("prehide", i)), !1
                }
                if ("tap" == ace.click_event && e && d.get(0).parentNode.parentNode == c) {
                    var l = d.find(".menu-text").get(0);
                    if (null != l && b.target != l && !a.contains(l, b.target)) return b.preventDefault(), !1
                }
                if (ace.vars.ios_safari && "false" !== d.attr("data-link")) return document.location = d.attr("href"), b.preventDefault(), !1
            }
        });
        var k = !1;
        this.show = function (b, c, d) {
            if (d = d !== !1, d && k) return !1;
            var e, f = a(b);
            if (f.trigger(e = a.Event("show.ace.submenu")), e.isDefaultPrevented()) return !1;
            d && (k = !0), c = c || this.settings.duration, f.css({
                height: 0,
                overflow: "hidden",
                display: "block"
            }).removeClass("nav-hide").addClass("nav-show").parent().addClass("open"), b.scrollTop = 0, c > 0 && f.css({
                height: b.scrollHeight,
                "transition-property": "height",
                "transition-duration": c / 1e3 + "s"
            });
            var g = function (b, c) {
                b && b.stopPropagation(), f.css({
                    "transition-property": "",
                    "transition-duration": "",
                    overflow: "",
                    height: ""
                }), c !== !1 && f.trigger(a.Event("shown.ace.submenu")), d && (k = !1)
            };
            return c > 0 && a.support.transition.end ? f.one(a.support.transition.end, g) : g(), ace.vars.android && setTimeout(function () {
                g(null, !1), ace.helper.redraw(b)
            }, c + 20), !0
        }, this.hide = function (b, c, d) {
            if (d = d !== !1, d && k) return !1;
            var e, f = a(b);
            if (f.trigger(e = a.Event("hide.ace.submenu")), e.isDefaultPrevented()) return !1;
            d && (k = !0), c = c || this.settings.duration, f.css({
                height: b.scrollHeight,
                overflow: "hidden",
                display: "block"
            }).parent().removeClass("open"), b.offsetHeight, c > 0 && f.css({
                height: 0,
                "transition-property": "height",
                "transition-duration": c / 1e3 + "s"
            });
            var g = function (b, c) {
                b && b.stopPropagation(), f.css({
                    display: "none",
                    overflow: "",
                    height: "",
                    "transition-property": "",
                    "transition-duration": ""
                }).removeClass("nav-show").addClass("nav-hide"), c !== !1 && f.trigger(a.Event("hidden.ace.submenu")), d && (k = !1)
            };
            return c > 0 && a.support.transition.end ? f.one(a.support.transition.end, g) : g(), ace.vars.android && setTimeout(function () {
                g(null, !1), ace.helper.redraw(b)
            }, c + 20), !0
        }, this.toggle = function (a, b) {
            if (b = b || g.settings.duration, 0 == a.scrollHeight) {
                if (this.show(a, b)) return 1
            } else if (this.hide(a, b)) return -1;
            return 0
        };
        var l = "menu-min",
            m = "responsive-min",
            n = "h-sidebar",
            o = function () {
                this.mobile_style = 1, this.$sidebar.hasClass("responsive") && !a('.menu-toggler[data-target="#' + this.$sidebar.attr("id") + '"]').hasClass("navbar-toggle") ? this.mobile_style = 2 : this.$sidebar.hasClass(m) ? this.mobile_style = 3 : this.$sidebar.hasClass("navbar-collapse") && (this.mobile_style = 4)
            };
        o.call(g), a(window).on("resize.sidebar.vars", function () {
            f.call(g)
        }).triggerHandler("resize.sidebar.vars")
    }
    var d = 0;
    a(document).on(ace.click_event + ".ace.menu", ".menu-toggler", function (b) {
        var c = a(this),
            d = a(c.attr("data-target"));
        if (0 != d.length) {
            b.preventDefault(), d.toggleClass("display"), c.toggleClass("display");
            var e = ace.click_event + ".ace.autohide",
                f = "true" === d.attr("data-auto-hide");
            return c.hasClass("display") ? (f && a(document).on(e, function (b) {
                return d.get(0) == b.target || a.contains(d.get(0), b.target) ? void b.stopPropagation() : (d.removeClass("display"), c.removeClass("display"), void a(document).off(e))
            }), "true" == d.attr("data-sidebar-scroll") && d.ace_sidebar_scroll("reset")) : f && a(document).off(e), !1
        }
    }).on(ace.click_event + ".ace.menu", ".sidebar-collapse", function (b) {
        var c = a(this).attr("data-target"),
            d = null;
        c && (d = a(c)), (null == d || 0 == d.length) && (d = a(this).closest(".sidebar")), 0 != d.length && (b.preventDefault(), d.ace_sidebar("toggleMenu", this))
    }).on(ace.click_event + ".ace.menu", ".sidebar-expand", function (b) {
        var c = a(this).attr("data-target"),
            d = null;
        if (c && (d = a(c)), (null == d || 0 == d.length) && (d = a(this).closest(".sidebar")), 0 != d.length) {
            var e = this;
            b.preventDefault(), d.ace_sidebar("toggleResponsive", this);
            var f = ace.click_event + ".ace.autohide";
            "true" === d.attr("data-auto-hide") && (d.hasClass("responsive-max") ? a(document).on(f, function (b) {
                return d.get(0) == b.target || a.contains(d.get(0), b.target) ? void b.stopPropagation() : (d.ace_sidebar("toggleResponsive", e), void a(document).off(f))
            }) : a(document).off(f))
        }
    }), a.fn.ace_sidebar = function (d, e) {
        var f, g = this.each(function () {
            var b = a(this),
                g = b.data("ace_sidebar"),
                h = "object" == typeof d && d;
            g || b.data("ace_sidebar", g = new c(this, h)), "string" == typeof d && "function" == typeof g[d] && (f = e instanceof Array ? g[d].apply(g, e) : g[d](e))
        });
        return f === b ? g : f
    }, a.fn.ace_sidebar.defaults = {
        duration: 300
    }
}(window.jQuery),
function (a, b) {
    function c(b, c) {
        var f = this,
            g = a(window),
            h = a(b),
            i = h.find(".nav-list"),
            j = h.find(".sidebar-toggle").eq(0),
            k = h.find(".sidebar-shortcuts").eq(0),
            l = i.get(0);
        if (l) {
            var m = ace.helper.getAttrSettings(b, a.fn.ace_sidebar_scroll.defaults);
            this.settings = a.extend({}, a.fn.ace_sidebar_scroll.defaults, c, m);
            var n = f.settings.scroll_to_active,
                o = h.ace_sidebar("ref");
            h.attr("data-sidebar-scroll", "true");
            var p = null,
                q = null,
                r = null,
                s = null,
                t = null,
                u = null;
            this.is_scrolling = !1;
            var v = !1;
            this.sidebar_fixed = e(b, "fixed");
            var w, x, y = function () {
                var a = i.parent().offset();
                return f.sidebar_fixed && (a.top -= ace.helper.scrollTop()), g.innerHeight() - a.top - (f.settings.include_toggle ? 0 : j.outerHeight())
            }, z = function () {
                return l.clientHeight
            }, A = function (b) {
                if (!v && f.sidebar_fixed) {
                    i.wrap('<div class="nav-wrap-up pos-rel" />'), i.after("<div><div></div></div>"), i.wrap('<div class="nav-wrap" />'), f.settings.include_toggle || j.css({
                        "z-index": 1
                    }), f.settings.include_shortcuts || k.css({
                        "z-index": 99
                    }), p = i.parent().next().ace_scroll({
                        size: y(),
                        mouseWheelLock: !0,
                        hoverReset: !1,
                        dragEvent: !0,
                        styleClass: f.settings.scroll_style,
                        touchDrag: !1
                    }).closest(".ace-scroll").addClass("nav-scroll"), u = p.data("ace_scroll"), q = p.find(".scroll-content").eq(0), r = q.find(" > div").eq(0), t = a(u.get_track()), s = t.find(".scroll-bar").eq(0), f.settings.include_shortcuts && 0 != k.length && (i.parent().prepend(k).wrapInner("<div />"), i = i.parent()), f.settings.include_toggle && 0 != j.length && (i.append(j), i.closest(".nav-wrap").addClass("nav-wrap-t")), i.css({
                        position: "relative"
                    }), 1 == f.settings.scroll_outside && p.addClass("scrollout"), l = i.get(0), l.style.top = 0, q.on("scroll.nav", function () {
                        l.style.top = -1 * this.scrollTop + "px"
                    }), i.on(a.event.special.mousewheel ? "mousewheel.ace_scroll" : "mousewheel.ace_scroll DOMMouseScroll.ace_scroll", function (a) {
                        return f.is_scrolling && u.is_active() ? p.trigger(a) : !f.settings.lock_anyway
                    }), i.on("mouseenter.ace_scroll", function () {
                        t.addClass("scroll-hover")
                    }).on("mouseleave.ace_scroll", function () {
                        t.removeClass("scroll-hover")
                    });
                    var c = q.get(0);
                    if (i.on("ace_drag.nav", function (b) {
                        if (!f.is_scrolling || !u.is_active()) return void (b.retval.cancel = !0);
                        if (0 != a(b.target).closest(".can-scroll").length) return void (b.retval.cancel = !0);
                        if ("up" == b.direction || "down" == b.direction) {
                            u.move_bar(!0);
                            var d = b.dy;
                            d = parseInt(Math.min(w, d)), Math.abs(d) > 2 && (d = 2 * d), 0 != d && (c.scrollTop = c.scrollTop + d, l.style.top = -1 * c.scrollTop + "px")
                    }
                    }), f.settings.smooth_scroll && i.on("touchstart.nav MSPointerDown.nav pointerdown.nav", function () {
                        i.css("transition-property", "none"), s.css("transition-property", "none")
                    }).on("touchend.nav touchcancel.nav MSPointerUp.nav MSPointerCancel.nav pointerup.nav pointercancel.nav", function () {
                        i.css("transition-property", "top"), s.css("transition-property", "top")
                    }), d && !f.settings.include_toggle) {
                        var e = j.get(0);
                        e && q.on("scroll.safari", function () {
                            ace.helper.redraw(e)
                        })
                    }
                    if (v = !0, 1 == b && (f.reset(), n && f.scroll_to_active(), n = !1), "number" == typeof f.settings.smooth_scroll && f.settings.smooth_scroll > 0 && (i.css({
                        "transition-property": "top",
                        "transition-duration": (f.settings.smooth_scroll / 1e3).toFixed(2) + "s"
                    }), s.css({
                        "transition-property": "top",
                        "transition-duration": (f.settings.smooth_scroll / 1500).toFixed(2) + "s"
                    }), p.on("drag.start", function (a) {
                        a.stopPropagation(), i.css("transition-property", "none")
                    }).on("drag.end", function (a) {
                        a.stopPropagation(), i.css("transition-property", "top")
                    })), ace.vars.android) {
                        var g = ace.helper.scrollTop();
                        2 > g && (window.scrollTo(g, 0), setTimeout(function () {
                            f.reset()
                        }, 20));
                        var h, m = ace.helper.winHeight();
                        a(window).on("scroll.ace_scroll", function () {
                            f.is_scrolling && u.is_active() && (h = ace.helper.winHeight(), h != m && (m = h, f.reset()))
                        })
                    }
                }
            };
            this.scroll_to_active = function () {
                if (u && u.is_active()) try {
                    var a, b = o.vars(),
                        c = h.find(".nav-list");
                    b.minimized && !b.collapsible ? a = c.find("> .active") : (a = i.find("> .active.hover"), 0 == a.length && (a = i.find(".active:not(.open)")));
                    var d = a.outerHeight();
                    c = c.get(0);
                    for (var e = a.get(0) ; e != c;) d += e.offsetTop, e = e.parentNode;
                    var f = d - p.height();
                    f > 0 && (l.style.top = -f + "px", q.scrollTop(f))
                } catch (g) { }
            }, this.reset = function (a) {
                if (a === !0 && (this.sidebar_fixed = e(b, "fixed")), !this.sidebar_fixed) return void this.disable();
                v || A();
                var c = o.vars(),
                    d = !c.collapsible && !c.horizontal && (w = y()) < (x = l.clientHeight);
                this.is_scrolling = !0, d && (r.css({
                    height: x,
                    width: 8
                }), p.prev().css({
                    "max-height": w
                }), u.update({
                    size: w
                }), u.enable(), u.reset()), d && u.is_active() ? h.addClass("sidebar-scroll") : this.is_scrolling && this.disable()
            }, this.disable = function () {
                this.is_scrolling = !1, p && (p.css({
                    height: "",
                    "max-height": ""
                }), r.css({
                    height: "",
                    width: ""
                }), p.prev().css({
                    "max-height": ""
                }), u.disable()), parseInt(l.style.top) < 0 && f.settings.smooth_scroll && a.support.transition.end ? i.one(a.support.transition.end, function () {
                    h.removeClass("sidebar-scroll"), i.off(".trans")
                }) : h.removeClass("sidebar-scroll"), l.style.top = 0
            }, this.prehide = function (a) {
                if (this.is_scrolling && !o.get("minimized"))
                    if (z() + a < y()) this.disable();
                    else if (0 > a) {
                        var b = q.scrollTop() + a;
                        if (0 > b) return;
                        l.style.top = -1 * b + "px"
                    }
            }, this._reset = function (a) {
                a === !0 && (this.sidebar_fixed = e(b, "fixed")), ace.vars.webkit ? setTimeout(function () {
                    f.reset()
                }, 0) : this.reset()
            }, this.set_hover = function () {
                t && t.addClass("scroll-hover")
            }, this.get = function (a) {
                return this.hasOwnProperty(a) ? this[a] : void 0
            }, this.set = function (a, b) {
                this.hasOwnProperty(a) && (this[a] = b)
            }, this.ref = function () {
                return this
            }, this.updateStyle = function (a) {
                null != u && u.update({
                    styleClass: a
                })
            }, h.on("hidden.ace.submenu.sidebar_scroll shown.ace.submenu.sidebar_scroll", ".submenu", function (a) {
                a.stopPropagation(), o.get("minimized") || (f._reset(), "shown" == a.type && f.set_hover())
            }), A(!0)
        }
    }
    var d = ace.vars.safari && navigator.userAgent.match(/version\/[1-5]/i),
        e = "getComputedStyle" in window ? function (a, b) {
            return a.offsetHeight, window.getComputedStyle(a).position == b
        } : function (b, c) {
            return b.offsetHeight, a(b).css("position") == c
        };
    a(document).on("settings.ace.sidebar_scroll", function (b, c) {
        a(".sidebar[data-sidebar-scroll=true]").each(function () {
            var b = a(this),
                d = b.ace_sidebar_scroll("ref");
            if ("sidebar_collapsed" == c && e(this, "fixed")) "true" == b.attr("data-sidebar-hover") && b.ace_sidebar_hover("reset"), d._reset();
            else if ("sidebar_fixed" === c || "navbar_fixed" === c) {
                var f = d.get("is_scrolling"),
                    g = e(this, "fixed");
                d.set("sidebar_fixed", g), g && !f ? d._reset() : g || d.disable()
            }
        })
    }), a(window).on("resize.ace.sidebar_scroll", function () {
        a(".sidebar[data-sidebar-scroll=true]").each(function () {
            var b = a(this);
            "true" == b.attr("data-sidebar-hover") && b.ace_sidebar_hover("reset");
            var c = a(this).ace_sidebar_scroll("ref"),
                d = e(this, "fixed");
            c.set("sidebar_fixed", d), c._reset()
        })
    }), a.fn.ace_sidebar_scroll || (a.fn.ace_sidebar_scroll = function (d, e) {
        var f, g = this.each(function () {
            var b = a(this),
                g = b.data("ace_sidebar_scroll"),
                h = "object" == typeof d && d;
            g || b.data("ace_sidebar_scroll", g = new c(this, h)), "string" == typeof d && "function" == typeof g[d] && (f = g[d](e))
        });
        return f === b ? g : f
    }, a.fn.ace_sidebar_scroll.defaults = {
        scroll_to_active: !0,
        include_shortcuts: !0,
        include_toggle: !1,
        smooth_scroll: 150,
        scroll_outside: !1,
        scroll_style: "",
        lock_anyway: !1
    })
}(window.jQuery),
function (a, b) {
    function c(b, c) {
        function h(b) {
            var c = b,
                d = a(c),
                e = null,
                f = !1;
            this.show = function () {
                null != e && clearTimeout(e), e = null, d.addClass("hover-show hover-shown"), f = !0;
                for (var a = 0; a < g.length; a++) g[a].find(".hover-show").not(".hover-shown").each(function () {
                    i(this).hide()
                })
            }, this.hide = function () {
                f = !1, d.removeClass("hover-show hover-shown hover-flip"), null != e && clearTimeout(e), e = null;
                var a = d.find("> .submenu").get(0);
                a && j(a, "hide")
            }, this.hideDelay = function (a) {
                null != e && clearTimeout(e), d.removeClass("hover-shown"), e = setTimeout(function () {
                    f = !1, d.removeClass("hover-show hover-flip"), e = null;
                    var b = d.find("> .submenu").get(0);
                    b && j(b, "hide"), "function" == typeof a && a.call(this)
                }, m.settings.sub_hover_delay)
            }, this.is_visible = function () {
                return f
            }
        }

        function i(b) {
            var c = a(b).data("subHide");
            return c || a(b).data("subHide", c = new h(b)), c
        }

        function j(b, c) {
            var d = a(b).data("ace_scroll");
            return d ? "string" == typeof c ? (d[c](), !0) : d : !1
        }

        function k(c) {
            var d = a(this),
                f = a(c);
            c.style.top = "", c.style.bottom = "";
            var g = null;
            q.minimized && (g = d.find(".menu-text").get(0)) && (g.style.marginTop = "");
            var h = ace.helper.scrollTop(),
                i = 0,
                k = h;
            v && (i = b.offsetTop, k += i + 1);
            var m = d.offset();
            m.top = parseInt(m.top);
            var n, o = 0;
            c.style.maxHeight = "";
            var r = c.scrollHeight,
                n = d.height();
            g && (o = n, m.top += o);
            var u = parseInt(m.top + r),
                x = 0,
                y = t.height(),
                z = parseInt(m.top - k - o),
                A = y,
                B = q.horizontal,
                C = !1;
            B && this.parentNode == p && (x = 0, m.top += d.height(), C = !0), !C && (x = u - (y + h)) >= 0 && (x = z > x ? x : z, 0 == x && (x = 20), z - x > 10 && (x += parseInt(Math.min(25, z - x))), m.top + (n - o) > u - x && (x -= m.top + (n - o) - (u - x)), x > 0 && (c.style.top = -x + "px", g && (g.style.marginTop = -x + "px"))), 0 > x && (x = 0);
            var D = x > 0 && x > n - 20;
            if (D ? d.addClass("pull_up") : d.removeClass("pull_up"), B)
                if (d.parent().parent().hasClass("hover-flip")) d.addClass("hover-flip");
                else {
                    var E = f.offset(),
                        F = f.width(),
                        G = t.width();
                    E.left + F > G && d.addClass("hover-flip")
                }
            var H = d.hasClass("hover") && !q.mobile_view;
            if (!(H && f.find("> li > .submenu").length > 0)) {
                var I = A - (m.top - h) + x,
                    J = x - I;
                if (J > 0 && n > J && (I += parseInt(Math.max(n, n - J))), I -= 5, !(90 > I)) {
                    var K = !1;
                    if (e) f.addClass("sub-scroll").css("max-height", I + "px");
                    else {
                        if (K = j(c), 0 == K) {
                            f.ace_scroll({
                                observeContent: !0,
                                detached: !0,
                                updatePos: !1,
                                reset: !0,
                                mouseWheelLock: !0,
                                styleClass: l.settings.sub_scroll_style
                            }), K = j(c);
                            var L = K.get_track();
                            L && f.after(L)
                        }
                        K.update({
                            size: I
                        })
                    } if (w = I, !e && K) {
                        I > 14 && r - I > 4 ? (K.enable(), K.reset()) : K.disable();
                        var L = K.get_track();
                        if (L) {
                            L.style.top = -(x - o - 1) + "px";
                            var m = f.position(),
                                M = m.left;
                            M += s ? 2 : f.outerWidth() - K.track_size(), L.style.left = parseInt(M) + "px", C && (L.style.left = parseInt(M - 2) + "px", L.style.top = parseInt(m.top) + (g ? o - 2 : 0) + "px")
                        }
                    }
                    ace.vars.safari && ace.helper.redraw(c)
                }
            }
        }
        var l = this,
            m = this,
            n = ace.helper.getAttrSettings(b, a.fn.ace_sidebar_hover.defaults);
        this.settings = a.extend({}, a.fn.ace_sidebar_hover.defaults, c, n);
        var o = a(b),
            p = o.find(".nav-list").get(0);
        o.attr("data-sidebar-hover", "true"), g.push(o);
        var q = {}, r = ace.vars.old_ie,
            s = !1;
        d && (l.settings.sub_hover_delay = parseInt(Math.max(l.settings.sub_hover_delay, 2500)));
        var t = a(window),
            u = a(".navbar").eq(0),
            v = "fixed" == u.css("position");
        this.update_vars = function () {
            v = "fixed" == u.css("position")
        }, l.dirty = !1, this.reset = function () {
            0 != l.dirty && (l.dirty = !1, o.find(".submenu").each(function () {
                var b = a(this),
                    c = b.parent();
                b.css({
                    top: "",
                    bottom: "",
                    "max-height": ""
                }), b.hasClass("ace-scroll") ? b.ace_scroll("disable") : b.removeClass("sub-scroll"), f(this, "absolute") ? b.addClass("can-scroll") : b.removeClass("can-scroll"), c.removeClass("pull_up").find(".menu-text:first").css("margin-top", "")
            }), o.find(".hover-show").removeClass("hover-show hover-shown hover-flip"))
        }, this.updateStyle = function (a) {
            sub_scroll_style = a, o.find(".submenu.ace-scroll").ace_scroll("update", {
                styleClass: a
            })
        }, this.changeDir = function (a) {
            s = "right" === a
        };
        var w = -1;
        e || o.on("hide.ace.submenu.sidebar_hover", ".submenu", function (b) {
            if (!(1 > w)) {
                b.stopPropagation();
                var c = a(this).closest(".ace-scroll.can-scroll");
                0 != c.length && f(c[0], "absolute") && c[0].scrollHeight - this.scrollHeight < w && c.ace_scroll("disable")
            }
        }), e || o.on("shown.ace.submenu.sidebar_hover hidden.ace.submenu.sidebar_hover", ".submenu", function () {
            if (!(1 > w)) {
                var b = a(this).closest(".ace-scroll.can-scroll");
                if (0 != b.length && f(b[0], "absolute")) {
                    var c = b[0].scrollHeight;
                    w > 14 && c - w > 4 ? b.ace_scroll("enable").ace_scroll("reset") : b.ace_scroll("disable")
                }
            }
        });
        var x = -1,
            y = d ? "touchstart.sub_hover" : "mouseenter.sub_hover",
            z = d ? "touchend.sub_hover touchcancel.sub_hover" : "mouseleave.sub_hover";
        o.on(y, ".nav-list li, .sidebar-shortcuts", function () {
            if (q = o.ace_sidebar("vars"), !q.collapsible) {
                var b = a(this),
                    c = !1,
                    e = b.hasClass("hover"),
                    g = b.find("> .submenu").get(0);
                if (!(g || this.parentNode == p || e || (c = b.hasClass("sidebar-shortcuts")))) return void (g && a(g).removeClass("can-scroll"));
                var h = g,
                    j = !1;
                if (h || this.parentNode != p || (h = b.find("> a > .menu-text").get(0)), !h && c && (h = b.find(".sidebar-shortcuts-large").get(0)), !(h && (j = f(h, "absolute")) || e)) return void (g && a(g).removeClass("can-scroll"));
                var m = i(this);
                if (g)
                    if (j) {
                        l.dirty = !0;
                        var n = ace.helper.scrollTop();
                        if (!m.is_visible() || !d && n != x || r)
                            if (a(g).addClass("can-scroll"), r || d) {
                                var s = this;
                                setTimeout(function () {
                                    k.call(s, g)
                                }, 0)
                            } else k.call(this, g);
                        x = n
                    } else a(g).removeClass("can-scroll");
                m.show()
            }
        }).on(z, ".nav-list li, .sidebar-shortcuts", function () {
            q = o.ace_sidebar("vars"), q.collapsible || a(this).hasClass("hover-show") && i(this).hideDelay()
        })
    }
    if (!ace.vars.very_old_ie) {
        var d = ace.vars.touch,
            e = ace.vars.old_ie || d,
            f = "getComputedStyle" in window ? function (a, b) {
                return a.offsetHeight, window.getComputedStyle(a).position == b
            } : function (b, c) {
                return b.offsetHeight, a(b).css("position") == c
            };
        a(window).on("resize.sidebar.ace_hover", function () {
            a(".sidebar[data-sidebar-hover=true]").ace_sidebar_hover("update_vars").ace_sidebar_hover("reset")
        }), a(document).on("settings.ace.ace_hover", function (b, c) {
            "sidebar_collapsed" == c ? a(".sidebar[data-sidebar-hover=true]").ace_sidebar_hover("reset") : "navbar_fixed" == c && a(".sidebar[data-sidebar-hover=true]").ace_sidebar_hover("update_vars")
        });
        var g = [];
        a.fn.ace_sidebar_hover = function (d, e) {
            var f, g = this.each(function () {
                var b = a(this),
                    g = b.data("ace_sidebar_hover"),
                    h = "object" == typeof d && d;
                g || b.data("ace_sidebar_hover", g = new c(this, h)), "string" == typeof d && "function" == typeof g[d] && (f = g[d](e))
            });
            return f === b ? g : f
        }, a.fn.ace_sidebar_hover.defaults = {
            sub_sub_hover_delay: 750,
            sub_scroll_style: "no-track scroll-thin"
        }
    }
}(window.jQuery),
function (a, b) {
    function c(b, c) {
        var d = b.find(".widget-main").eq(0);
        a(window).off("resize.widget.scroll");
        var e = ace.vars.old_ie || ace.vars.touch;
        if (c) {
            var f = d.data("ace_scroll");
            f && d.data("save_scroll", {
                size: f.size,
                lock: f.lock,
                lock_anyway: f.lock_anyway
            });
            var g = b.height() - b.find(".widget-header").height() - 10;
            g = parseInt(g), d.css("min-height", g), e ? (f && d.ace_scroll("disable"), d.css("max-height", g).addClass("overflow-scroll")) : (f ? d.ace_scroll("update", {
                size: g,
                mouseWheelLock: !0,
                lockAnyway: !0
            }) : d.ace_scroll({
                size: g,
                mouseWheelLock: !0,
                lockAnyway: !0
            }), d.ace_scroll("enable").ace_scroll("reset")), a(window).on("resize.widget.scroll", function () {
                var a = b.height() - b.find(".widget-header").height() - 10;
                a = parseInt(a), d.css("min-height", a), e ? d.css("max-height", a).addClass("overflow-scroll") : d.ace_scroll("update", {
                    size: a
                }).ace_scroll("reset")
            })
        } else {
            d.css("min-height", "");
            var h = d.data("save_scroll");
            h && d.ace_scroll("update", {
                size: h.size,
                mouseWheelLock: h.lock,
                lockAnyway: h.lock_anyway
            }).ace_scroll("enable").ace_scroll("reset"), e ? d.css("max-height", "").removeClass("overflow-scroll") : h || d.ace_scroll("disable")
        }
    }
    var d = function (b) {
        this.$box = a(b);
        this.reload = function () {
            var a = this.$box,
                b = !1;
            "static" == a.css("position") && (b = !0, a.addClass("position-relative")), a.append('<div class="widget-box-overlay"><i class="' + ace.vars.icon + 'loading-icon fa fa-spinner fa-spin fa-2x white"></i></div>'), a.one("reloaded.ace.widget", function () {
               a.find(".widget-box-overlay").remove(), b && a.removeClass("position-relative")
            })
        }, this.close = function () {
            var a = this.$box,
                b = 300;
            a.fadeOut(b, function () {
                a.trigger("closed.ace.widget"), a.remove()
            })
        }, this.toggle = function (a, b) {
            var c = this.$box,
                d = c.find(".widget-body").eq(0),
                e = null,
                f = "undefined" != typeof a ? a : c.hasClass("collapsed") ? "show" : "hide",
                g = "show" == f ? "shown" : "hidden";
            if ("undefined" == typeof b && (b = c.find("> .widget-header a[data-action=collapse]").eq(0), 0 == b.length && (b = null)), b) {
                e = b.find(ace.vars[".icon"]).eq(0);
                var h, i = null,
                    j = null;
                (i = e.attr("data-icon-show")) ? j = e.attr("data-icon-hide") : (h = e.attr("class").match(/fa\-(.*)\-(up|down)/)) && (i = "fa-" + h[1] + "-down", j = "fa-" + h[1] + "-up")
            }
            var k = 250,
                l = 200;
            "show" == f ? (e && e.removeClass(i).addClass(j), d.hide(), c.removeClass("collapsed"), d.slideDown(k, function () {
                c.trigger(g + ".ace.widget")
            })) : (e && e.removeClass(j).addClass(i), d.slideUp(l, function () {
                c.addClass("collapsed"), c.trigger(g + ".ace.widget")
            }))
        }, this.hide = function () {
            this.toggle("hide")
        }, this.show = function () {
            this.toggle("show")
        }, this.fullscreen = function () {
            var a = this.$box.find("> .widget-header a[data-action=fullscreen]").find(ace.vars[".icon"]).eq(0),
                b = null,
                d = null;
            (b = a.attr("data-icon1")) ? d = a.attr("data-icon2") : (b = "fa-expand", d = "fa-compress"), this.$box.hasClass("fullscreen") ? (a.addClass(b).removeClass(d), this.$box.removeClass("fullscreen"), c(this.$box, !1)) : (a.removeClass(b).addClass(d), this.$box.addClass("fullscreen"), c(this.$box, !0)), this.$box.trigger("fullscreened.ace.widget")
        }
    };
    a.fn.widget_box = function (c, e) {
        var f, g = this.each(function () {
            var b = a(this),
                g = b.data("widget_box"),
                h = "object" == typeof c && c;
            g || b.data("widget_box", g = new d(this, h)), "string" == typeof c && (f = g[c](e))
        });
        return f === b ? g : f
    }, a(document).on("click.ace.widget", ".widget-header a[data-action]", function (b) {
        b.preventDefault();
        var c = a(this),
            e = c.closest(".widget-box");
        if (0 != e.length && !e.hasClass("ui-sortable-helper")) {
            var f = e.data("widget_box");
            f || e.data("widget_box", f = new d(e.get(0)));
            var g = c.data("action");
            if ("collapse" == g) {
                var h, i = e.hasClass("collapsed") ? "show" : "hide";
                if (e.trigger(h = a.Event(i + ".ace.widget")), h.isDefaultPrevented()) return;
                f.toggle(i, c)
            } else if ("close" == g) {
                var h;
                if (e.trigger(h = a.Event("close.ace.widget")), h.isDefaultPrevented()) return;
                f.close()
            } else if ("reload" == g) {
                c.blur();
                var h;
                if (e.trigger(h = a.Event("reload.ace.widget")), h.isDefaultPrevented()) return;
                f.reload()
            } else if ("fullscreen" == g) {
                var h;
                if (e.trigger(h = a.Event("fullscreen.ace.widget")), h.isDefaultPrevented()) return;
                f.fullscreen()
            } else "settings" == g && e.trigger("setting.ace.widget")
        }
    })
}(window.jQuery),
function (a) {
    a("#ace-settings-btn").on(ace.click_event, function (b) {
        b.preventDefault(), a(this).toggleClass("open"), a("#ace-settings-box").toggleClass("open")
    }), a("#ace-settings-navbar").on("click", function () {
        ace.settings.navbar_fixed(null, this.checked)
    }).each(function () {
        this.checked = ace.settings.is("navbar", "fixed")
    }), a("#ace-settings-sidebar").on("click", function () {
        ace.settings.sidebar_fixed(null, this.checked)
    }).each(function () {
        this.checked = ace.settings.is("sidebar", "fixed")
    }), a("#ace-settings-breadcrumbs").on("click", function () {
        ace.settings.breadcrumbs_fixed(null, this.checked)
    }).each(function () {
        this.checked = ace.settings.is("breadcrumbs", "fixed")
    }), a("#ace-settings-add-container").on("click", function () {
        ace.settings.main_container_fixed(null, this.checked)
    }).each(function () {
        this.checked = ace.settings.is("main-container", "fixed")
    }), a("#ace-settings-compact").on("click", function () {
        if (this.checked) {
            a("#sidebar").addClass("compact");
            var b = a("#ace-settings-hover");
            b.length > 0 && b.removeAttr("checked").trigger("click")
        } else a("#sidebar").removeClass("compact"), a("#sidebar[data-sidebar-scroll=true]").ace_sidebar_scroll("reset");
        ace.vars.old_ie && ace.helper.redraw(a("#sidebar")[0], !0)
    }), a("#ace-settings-highlight").on("click", function () {
        this.checked ? a("#sidebar .nav-list > li").addClass("highlight") : a("#sidebar .nav-list > li").removeClass("highlight"), ace.vars.old_ie && ace.helper.redraw(a("#sidebar")[0])
    }), a("#ace-settings-hover").on("click", function () {
        if (!a("#sidebar").hasClass("h-sidebar")) {
            if (this.checked) a("#sidebar li").addClass("hover").filter(".open").removeClass("open").find("> .submenu").css("display", "none");
            else {
                a("#sidebar li.hover").removeClass("hover");
                var b = a("#ace-settings-compact");
                b.length > 0 && b.get(0).checked && b.trigger("click")
            }
            a(".sidebar[data-sidebar-hover=true]").ace_sidebar_hover("reset"), a(".sidebar[data-sidebar-scroll=true]").ace_sidebar_scroll("reset"), ace.vars.old_ie && ace.helper.redraw(a("#sidebar")[0])
        }
    })
}(jQuery),
function (a) {
    a("#ace-settings-rtl").removeAttr("checked").on("click", function () {
        b()
    });
    var b = function () {
        function b(b) {
            function c(a, b) {
                e.find("." + a).removeClass(a).addClass("tmp-rtl-" + a).end().find("." + b).removeClass(b).addClass(a).end().find(".tmp-rtl-" + a).removeClass("tmp-rtl-" + a).addClass(b)
            }
            var d = a(document.body);
            b || d.toggleClass("rtl"), b = b || document.body;
            var e = a(b);
            e.find(".dropdown-menu:not(.datepicker-dropdown,.colorpicker)").toggleClass("dropdown-menu-right").end().find(".pull-right:not(.dropdown-menu,blockquote,.profile-skills .pull-right)").removeClass("pull-right").addClass("tmp-rtl-pull-right").end().find(".pull-left:not(.dropdown-submenu,.profile-skills .pull-left)").removeClass("pull-left").addClass("pull-right").end().find(".tmp-rtl-pull-right").removeClass("tmp-rtl-pull-right").addClass("pull-left").end().find(".chosen-select").toggleClass("chosen-rtl").next().toggleClass("chosen-rtl"), c("align-left", "align-right"), c("no-padding-left", "no-padding-right"), c("arrowed", "arrowed-right"), c("arrowed-in", "arrowed-in-right"), c("tabs-left", "tabs-right"), c("messagebar-item-left", "messagebar-item-right"), a(".modal.aside-vc").ace_aside("flip").ace_aside("insideContainer"), e.find(".fa").each(function () {
                if (!(this.className.match(/ui-icon/) || a(this).closest(".fc-button").length > 0))
                    for (var b = this.attributes.length, c = 0; b > c; c++) {
                        var d = this.attributes[c].value;
                        d.match(/fa\-(?:[\w\-]+)\-left/) ? this.attributes[c].value = d.replace(/fa\-([\w\-]+)\-(left)/i, "fa-$1-right") : d.match(/fa\-(?:[\w\-]+)\-right/) && (this.attributes[c].value = d.replace(/fa\-([\w\-]+)\-(right)/i, "fa-$1-left"))
                    }
            });
            var f = d.hasClass("rtl");
            f ? (e.find(".scroll-hz").addClass("make-ltr").find(".scroll-content").wrapInner('<div class="make-rtl" />'), a(".sidebar[data-sidebar-hover=true]").ace_sidebar_hover("changeDir", "right")) : (e.find(".scroll-hz").removeClass("make-ltr").find(".make-rtl").children().unwrap(), a(".sidebar[data-sidebar-hover=true]").ace_sidebar_hover("changeDir", "left")), a.fn.ace_scroll && e.find(".scroll-hz").ace_scroll("reset");
            try {
                var g = a("#piechart-placeholder");
                if (g.length > 0) {
                    var h = d.hasClass("rtl") ? "nw" : "ne";
                    g.data("draw").call(g.get(0), g, g.data("chart"), h)
                }
            } catch (i) { }
            ace.helper.redraw(b, !0)
        }
        if (0 == a("#ace-rtl-stylesheet").length) {
            var c = a("head").find("link.ace-main-stylesheet");
            0 == c.length && (c = a("head").find('link[href*="/ace.min.css"],link[href*="/ace-part2.min.css"]'), 0 == c.length && (c = a("head").find('link[href*="/ace.css"],link[href*="/ace-part2.css"]')));
            var d = a("head").find("link#ace-skins-stylesheet"),
                e = c.first().attr("href").replace(/(\.min)?\.css$/i, "-rtl%241.html");
            a.ajax({
                url: e
            }).done(function () {
                var a = jQuery("<link />", {
                    type: "text/css",
                    rel: "stylesheet",
                    id: "ace-rtl-stylesheet"
                });
                d.length > 0 ? a.insertAfter(d) : c.length > 0 ? a.insertAfter(c.last()) : a.appendTo("head"), a.attr("href", e), b(), window.Pace && Pace.running && Pace.stop()
            })
        } else b();
        a(".page-content-area[data-ajax-content=true]").on("ajaxscriptsloaded.rtl", function () {
            a("body").hasClass("rtl") && b(this)
        })
    }
}(jQuery),
function (a) {
    try {
        a("#skin-colorpicker").ace_colorpicker({
            auto_pos: !1
        })
    } catch (b) { }
    a("#skin-colorpicker").on("change", function () {
        function b(b) {
            var c = a(document.body);
            c.removeClass("no-skin skin-1 skin-2 skin-3"), c.addClass(b), ace.data.set("skin", b);
            var d = ["red", "blue", "green", ""];
            a(".ace-nav > li.grey").removeClass("dark"), a(".ace-nav > li").removeClass("no-border margin-1"), a(".ace-nav > li:not(:last-child)").removeClass("light-pink").find("> a > " + ace.vars[".icon"]).removeClass("pink").end().eq(0).find(".badge").removeClass("badge-warning"), a(".sidebar-shortcuts .btn").removeClass("btn-pink btn-white").find(ace.vars[".icon"]).removeClass("white"), a(".ace-nav > li.grey").removeClass("red").find(".badge").removeClass("badge-yellow"), a(".sidebar-shortcuts .btn").removeClass("btn-primary btn-white");
            var e = 0;
            a(".sidebar-shortcuts .btn").each(function () {
                a(this).find(ace.vars[".icon"]).removeClass(d[e++])
            });
            var f = ["btn-success", "btn-info", "btn-warning", "btn-danger"];
            if ("no-skin" == b) {
                var e = 0;
                a(".sidebar-shortcuts .btn").each(function () {
                    a(this).attr("class", "btn " + f[e++ % 4])
                }), a(".sidebar[data-sidebar-scroll=true]").ace_sidebar_scroll("updateStyle", ""), a(".sidebar[data-sidebar-hover=true]").ace_sidebar_hover("updateStyle", "no-track scroll-thin")
            } else if ("skin-1" == b) {
                a(".ace-nav > li.grey").addClass("dark");
                var e = 0;
                a(".sidebar-shortcuts").find(".btn").each(function () {
                    a(this).attr("class", "btn " + f[e++ % 4])
                }), a(".sidebar[data-sidebar-scroll=true]").ace_sidebar_scroll("updateStyle", "scroll-white no-track"), a(".sidebar[data-sidebar-hover=true]").ace_sidebar_hover("updateStyle", "no-track scroll-thin scroll-white")
            } else if ("skin-2" == b) a(".ace-nav > li").addClass("no-border margin-1"), a(".ace-nav > li:not(:last-child)").addClass("light-pink").find("> a > " + ace.vars[".icon"]).addClass("pink").end().eq(0).find(".badge").addClass("badge-warning"), a(".sidebar-shortcuts .btn").attr("class", "btn btn-white btn-pink").find(ace.vars[".icon"]).addClass("white"), a(".sidebar[data-sidebar-scroll=true]").ace_sidebar_scroll("updateStyle", "scroll-white no-track"), a(".sidebar[data-sidebar-hover=true]").ace_sidebar_hover("updateStyle", "no-track scroll-thin scroll-white");
            else if ("skin-3" == b) {
                c.addClass("no-skin"), a(".ace-nav > li.grey").addClass("red").find(".badge").addClass("badge-yellow");
                var e = 0;
                a(".sidebar-shortcuts .btn").each(function () {
                    a(this).attr("class", "btn btn-primary btn-white"), a(this).find(ace.vars[".icon"]).addClass(d[e++])
                }), a(".sidebar[data-sidebar-scroll=true]").ace_sidebar_scroll("updateStyle", "scroll-dark no-track"), a(".sidebar[data-sidebar-hover=true]").ace_sidebar_hover("updateStyle", "no-track scroll-thin")
            }
            a(".sidebar[data-sidebar-scroll=true]").ace_sidebar_scroll("reset"), ace.vars.old_ie && ace.helper.redraw(document.body, !0)
        }
        var c = a(this).find("option:selected").data("skin");
        if (0 == a("#ace-skins-stylesheet").length) {
            var d = a("head").find("link.ace-main-stylesheet");
            0 == d.length && (d = a("head").find('link[href*="/ace.min.css"],link[href*="/ace-part2.min.css"]'), 0 == d.length && (d = a("head").find('link[href*="/ace.css"],link[href*="/ace-part2.css"]')));
            var e = d.first().attr("href").replace(/(\.min)?\.css$/i, "-skins%241.html");
            a.ajax({
                url: e
            }).done(function () {
                var a = jQuery("<link />", {
                    type: "text/css",
                    rel: "stylesheet",
                    id: "ace-skins-stylesheet"
                });
                d.length > 0 ? a.insertAfter(d.last()) : a.appendTo("head"), a.attr("href", e), b(c), window.Pace && Pace.running && Pace.stop()
            })
        } else b(c)
    })
}(jQuery),
function (a) {
    a(document).on("reload.ace.widget", ".widget-box", function () {
        var b = a(this);
        setTimeout(function () {
            b.trigger("reloaded.ace.widget")
        }, parseInt(1e3 * Math.random() + 1e3))   
    
    })
}(window.jQuery),
function (a) {
    ace.vars.US_STATES = ["Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut", "Delaware", "Florida", "Georgia", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine", "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey", "New Mexico", "New York", "North Dakota", "North Carolina", "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "Rhode Island", "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington", "West Virginia", "Wisconsin", "Wyoming"];
    try {
        a("#nav-search-input").bs_typeahead({
            source: ace.vars.US_STATES,
            updater: function (b) {
                return a("#nav-search-input").focus(), b
            }
        })
    } catch (b) { }
}(window.jQuery);
