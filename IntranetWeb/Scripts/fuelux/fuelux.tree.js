﻿! function(a) {
    "function" == typeof define && define.amd ? define(["jquery"], a) : a(jQuery)
}(function(a) {
    var b = a.fn.tree,
        c = function(b, c) {
            this.$element = a(b), this.options = a.extend({}, a.fn.tree.defaults, c), this.$element.on("click.fu.tree", ".tree-item", a.proxy(function(a) {
                this.selectItem(a.currentTarget)
            }, this)), this.$element.on("click.fu.tree", ".tree-branch-header", a.proxy(function(a) {
                this.openFolder(a.currentTarget)
            }, this)), this.render()
        };
    c.prototype = {
        constructor: c,
        destroy: function() {
            return this.$element.find("li:not([data-template])").remove(), this.$element.remove(), this.$element[0].outerHTML
        },
        render: function() {
            this.populate(this.$element)
        },
        populate: function(b) {
            var c = this,
                d = b.hasClass("tree") ? b : b.parent(),
                e = d.find(".tree-loader:eq(0)"),
                f = d.data();
            e.removeClass("hide"), this.options.dataSource(f ? f : {}, function(f) {
                e.addClass("hide"), a.each(f.data, function(e, f) {
                    var g;
                    if ("folder" === f.type) {
                        g = c.$element.find("[data-template=treebranch]:eq(0)").clone().removeClass("hide").removeAttr("data-template"), g.data(f), g.find(".tree-branch-name > .tree-label").html(f.text || f.name);
                        var h = g.find(".tree-branch-header");
                        "icon-class" in f && h.find("i").addClass(f["icon-class"]), "additionalParameters" in f && "item-selected" in f.additionalParameters && 1 == f.additionalParameters["item-selected"] && setTimeout(function() {
                            h.trigger("click")
                        }, 0)
                    } else "item" === f.type && (g = c.$element.find("[data-template=treeitem]:eq(0)").clone().removeClass("hide").removeAttr("data-template"), g.find(".tree-item-name > .tree-label").html(f.text || f.name), g.data(f), "additionalParameters" in f && "item-selected" in f.additionalParameters && 1 == f.additionalParameters["item-selected"] && (g.addClass("tree-selected"), g.find("i").removeClass(c.options["unselected-icon"]).addClass(c.options["selected-icon"])));
                    var i = f.attr || f.dataAttributes || [];
                    a.each(i, function(a, b) {
                        switch (a) {
                            case "cssClass":
                            case "class":
                            case "className":
                                g.addClass(b);
                                break;
                            case "data-icon":
                                g.find(".icon-item").removeClass().addClass("icon-item " + b), g.attr(a, b);
                                break;
                            case "id":
                                g.attr(a, b), g.attr("aria-labelledby", b + "-label"), g.find(".tree-branch-name > .tree-label").attr("id", b + "-label");
                                break;
                            default:
                                g.attr(a, b)
                        }
                    }), b.hasClass("tree-branch-header") ? d.find(".tree-branch-children:eq(0)").append(g) : b.append(g)
                }), c.$element.trigger("loaded.fu.tree", d)
            })
        },
        selectItem: function(b) {
            if (0 != this.options.selectable) {
                var c = a(b),
                    d = c.data(),
                    e = this.$element.find(".tree-selected"),
                    f = [],
                    g = c.find(".icon-item");
                this.options.multiSelect ? a.each(e, function(b, d) {
                    var e = a(d);
                    e[0] !== c[0] && f.push(a(d).data())
                }) : e[0] !== c[0] && (e.removeClass("tree-selected").find("i").removeClass(this.options["selected-icon"]).addClass(this.options["unselected-icon"]), f.push(d));
                var h = "selected";
                c.hasClass("tree-selected") ? (h = "deselected", c.removeClass("tree-selected"), (g.hasClass(this.options["selected-icon"]) || g.hasClass(this.options["unselected-icon"])) && g.removeClass(this.options["selected-icon"]).addClass(this.options["unselected-icon"])) : (c.addClass("tree-selected"), (g.hasClass(this.options["selected-icon"]) || g.hasClass(this.options["unselected-icon"])) && g.removeClass(this.options["unselected-icon"]).addClass(this.options["selected-icon"]), this.options.multiSelect && f.push(d)), this.$element.trigger(h + ".fu.tree", {
                    target: d,
                    selected: f
                }), c.trigger("updated.fu.tree", {
                    selected: f,
                    item: c,
                    eventType: h
                })
            }
        },
        openFolder: function(b) {
            var c, d, e, f = a(b);
            c = f.closest(".tree-branch"), d = c.find(".tree-branch-children"), e = d.eq(0);
            var g, h, i;
            f.find("." + a.trim(this.options["close-icon"].replace(/(\s+)/g, "."))).length ? (g = "opened", h = this.options["close-icon"], i = this.options["open-icon"], c.addClass("tree-open"), c.attr("aria-expanded", "true"), e.removeClass("hide"), d.children().length || this.populate(d)) : f.find("." + a.trim(this.options["open-icon"].replace(/(\s+)/g, "."))) && (g = "closed", h = this.options["open-icon"], i = this.options["close-icon"], c.removeClass("tree-open"), c.attr("aria-expanded", "false"), e.addClass("hide"), this.options.cacheItems || e.empty()), c.find("> .tree-branch-header .icon-folder").eq(0).removeClass(h).addClass(i), this.$element.trigger(g + ".fu.tree", c.data())
        },
        selectFolder: function(b) {
            var c = a(b),
                d = c.closest(".tree-branch"),
                e = this.$element.find(".tree-branch.tree-selected"),
                f = d.data(),
                g = [],
                h = "selected";
            d.hasClass("tree-selected") ? (h = "deselected", d.removeClass("tree-selected")) : d.addClass("tree-selected"), this.options.multiSelect ? (e = this.$element.find(".tree-branch.tree-selected"), a.each(e, function(b, d) {
                var e = a(d);
                e[0] !== c[0] && g.push(a(d).data())
            })) : e[0] !== c[0] && (e.removeClass("tree-selected"), g.push(f)), this.$element.trigger(h + ".fu.tree", {
                target: f,
                selected: g
            }), c.trigger("updated.fu.tree", {
                selected: g,
                item: c,
                eventType: h
            })
        },
        selectedItems: function() {
            var b = this.$element.find(".tree-selected"),
                c = [];
            return a.each(b, function(b, d) {
                c.push(a(d).data())
            }), c
        },
        collapse: function() {
            var b = this.options.cacheItems;
            this.$element.find("." + a.trim(this.options["open-icon"].replace(/(\s+)/g, "."))).each(function() {
                var c = a(this).removeClass(this.options["open-icon"] + " " + this.options["close-icon"]).addClass(this.options["close-icon"]),
                    d = c.parent().parent(),
                    e = d.children(".tree-branch-children");
                e.addClass("hide"), b || e.empty()
            })
        }
    }, a.fn.tree = function(b) {
        var d, e = Array.prototype.slice.call(arguments, 1),
            f = this.each(function() {
                var f = a(this),
                    g = f.data("fu.tree"),
                    h = "object" == typeof b && b;
                g || f.data("fu.tree", g = new c(this, h)), "string" == typeof b && (d = g[b].apply(g, e))
            });
        return void 0 === d ? f : d
    }, a.fn.tree.defaults = {
        dataSource: function() {},
        multiSelect: !1,
        cacheItems: !0,
        folderSelect: !1
    }, a.fn.tree.Constructor = c, a.fn.tree.noConflict = function() {
        return a.fn.tree = b, this
    }
});