﻿! function (a) {
    "function" == typeof define && define.amd ? define(["jquery"], a) : a(jQuery)
}(function (a) {
    var b = a.fn.wizard,
        c = function (b, c) {
            var d;
            this.$element = a(b), this.options = a.extend({}, a.fn.wizard.defaults, c), this.options.disablePreviousStep = "previous" === this.$element.attr("data-restrict") ? !0 : this.options.disablePreviousStep, this.currentStep = this.options.selectedItem.step, this.numSteps = this.$element.find(".steps li").length, this.$prevBtn = this.$element.find("button.btn-prev"), this.$nextBtn = this.$element.find("button.btn-next"), d = this.$nextBtn.children().detach(), this.nextText = a.trim(this.$nextBtn.text()), this.$nextBtn.append(d), this.$prevBtn.on("click.fu.wizard", a.proxy(this.previous, this)), this.$nextBtn.on("click.fu.wizard", a.proxy(this.next, this)), this.$element.on("click.fu.wizard", "li.complete", a.proxy(this.stepclicked, this)), this.selectedItem(this.options.selectedItem), this.options.disablePreviousStep && (this.$prevBtn.attr("disabled", !0), this.$element.find(".steps").addClass("previous-disabled"))
        };
    c.prototype = {
        constructor: c,
        destroy: function () {
            return this.$element.remove(), this.$element[0].outerHTML
        },
        addSteps: function (b) {
            var c, d, e, f, g, h, i = [].slice.call(arguments).slice(1),
                j = this.$element.find(".steps"),
                k = this.$element.find(".step-content");
            for (b = -1 === b || b > this.numSteps + 1 ? this.numSteps + 1 : b, i[0] instanceof Array && (i = i[0]), g = j.find("li:nth-child(" + b + ")"), f = k.find(".step-pane:nth-child(" + b + ")"), g.length < 1 && (g = null), c = 0, d = i.length; d > c; c++) h = a('<li data-step="' + b + '"><span class="badge badge-info"></span></li>'), h.append(i[c].label || "").append('<span class="chevron"></span>'), h.find(".badge").append(i[c].badge || b), e = a('<div class="step-pane" data-step="' + b + '"></div>'), e.append(i[c].pane || ""), g ? (g.before(h), f.before(e)) : (j.append(h), k.append(e)), b++;
            this.syncSteps(), this.numSteps = j.find("li").length, this.setState()
        },
        removeSteps: function (b, c) {
            var d, e = "nextAll",
                f = 0,
                g = this.$element.find(".steps"),
                h = this.$element.find(".step-content");
            c = void 0 !== c ? c : 1, b > g.find("li").length ? d = g.find("li:last") : (d = g.find("li:nth-child(" + b + ")").prev(), d.length < 1 && (e = "children", d = g)), d[e]().each(function () {
                var b = a(this),
                    d = b.attr("data-step");
                return c > f ? (b.remove(), h.find('.step-pane[data-step="' + d + '"]:first').remove(), void f++) : !1
            }), this.syncSteps(), this.numSteps = g.find("li").length, this.setState()
        },
        setState: function () {
            var b = this.currentStep > 1,
                c = 1 === this.currentStep,
                d = this.currentStep === this.numSteps;
            this.options.disablePreviousStep || this.$prevBtn.attr("disabled", c === !0 || b === !1);
            var e = this.$nextBtn.attr("data-last");
            if (e) {
                this.lastText = e;
                var f = this.nextText;
                d === !0 ? (f = this.lastText, this.$element.addClass("complete")) : this.$element.removeClass("complete");
                var g = this.$nextBtn.children().detach();
                this.$nextBtn.text(f).append(g)
            }
            var h = this.$element.find(".steps li");
            h.removeClass("active").removeClass("complete"), h.find("span.badge").removeClass("badge-info").removeClass("badge-success");
            var i = ".steps li:lt(" + (this.currentStep - 1) + ")",
                j = this.$element.find(i);
            j.addClass("complete"), j.find("span.badge").addClass("badge-success");
            var k = ".steps li:eq(" + (this.currentStep - 1) + ")",
                l = this.$element.find(k);
            l.addClass("active"), l.find("span.badge").addClass("badge-info");
            var m = this.$element.find(".step-content"),
                n = l.attr("data-step");
            if (m.find(".step-pane").removeClass("active"), m.find('.step-pane[data-step="' + n + '"]:first').addClass("active"), "undefined" != typeof this.initialized) {
                var o = a.Event("changed.fu.wizard");
                this.$element.trigger(o, {
                    step: this.currentStep
                })
            }
            this.initialized = !0
        },
        stepclicked: function (b) {
            var c = a(b.currentTarget),
                d = this.$element.find(".steps li").index(c),
                e = !0;
            if (this.options.disablePreviousStep && d < this.currentStep && (e = !1), e) {
                var f = a.Event("stepclicked.fu.wizard");
                if (this.$element.trigger(f, {
                    step: d + 1
                }), f.isDefaultPrevented()) return;
                this.currentStep = d + 1, this.setState()
            }
        },
        syncSteps: function () {
            var b = 1,
                c = this.$element.find(".steps"),
                d = this.$element.find(".step-content");
            c.children().each(function () {
                var c = a(this),
                    e = c.find(".badge"),
                    f = c.attr("data-step");
                isNaN(parseInt(e.html(), 10)) || e.html(b), c.attr("data-step", b), d.find('.step-pane[data-step="' + f + '"]:last').attr("data-step", b), b++
            })
        },
        previous: function () {
            var b = this.currentStep > 1;
            if (this.options.disablePreviousStep && (b = !1), b) {
                var c = a.Event("actionclicked.fu.wizard");
                if (this.$element.trigger(c, {
                    step: this.currentStep,
                    direction: "previous"
                }), c.isDefaultPrevented()) return;
                this.currentStep -= 1, this.setState()
            }
            this.$prevBtn.is(":disabled") ? this.$nextBtn.focus() : this.$prevBtn.focus()
        },
        next: function () {
            var b = this.currentStep + 1 <= this.numSteps,
                c = this.currentStep === this.numSteps;
            if (b) {
                var d = a.Event("actionclicked.fu.wizard");
                if (this.$element.trigger(d, {
                    step: this.currentStep,
                    direction: "next"
                }), d.isDefaultPrevented()) return;
                this.currentStep += 1, this.setState()
            } else c && this.$element.trigger("finished.fu.wizard");
            this.$nextBtn.is(":disabled") ? this.$prevBtn.focus() : this.$nextBtn.focus()
        },
        selectedItem: function (a) {
            var b, c;
            return a ? (c = a.step || -1, c >= 1 && c <= this.numSteps ? (this.currentStep = c, this.setState()) : (c = this.$element.find(".steps li.active:first").attr("data-step"), isNaN(c) || (this.currentStep = parseInt(c, 10), this.setState())), b = this) : b = {
                step: this.currentStep
            }, b
        }
    }, a.fn.wizard = function (b) {
        var d, e = Array.prototype.slice.call(arguments, 1),
            f = this.each(function () {
                var f = a(this),
                    g = f.data("fu.wizard"),
                    h = "object" == typeof b && b;
                g || f.data("fu.wizard", g = new c(this, h)), "string" == typeof b && (d = g[b].apply(g, e))
            });
        return void 0 === d ? f : d
    }, a.fn.wizard.defaults = {
        disablePreviousStep: !1,
        selectedItem: {
            step: -1
        }
    }, a.fn.wizard.Constructor = c, a.fn.wizard.noConflict = function () {
        return a.fn.wizard = b, this
    }, a(document).on("mouseover.fu.wizard.data-api", "[data-initialize=wizard]", function (b) {
        var c = a(b.target).closest(".wizard");
        c.data("fu.wizard") || c.wizard(c.data())
    }), a(function () {
        a("[data-initialize=wizard]").each(function () {
            var b = a(this);
            b.data("fu.wizard") || b.wizard(b.data())
        })
    })
});