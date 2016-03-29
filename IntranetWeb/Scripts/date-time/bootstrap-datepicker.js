﻿! function (a, b) {
    function c() {
        return new Date(Date.UTC.apply(Date, arguments))
    }

    function d() {
        var a = new Date;
        return c(a.getFullYear(), a.getMonth(), a.getDate())
    }

    function e(a) {
        return function () {
            return this[a].apply(this, arguments)
        }
    }

    function f(b, c) {
        function d(a, b) {
            return b.toLowerCase()
        }
        var e, f = a(b).data(),
            g = {}, h = new RegExp("^" + c.toLowerCase() + "([A-Z])");
        c = new RegExp("^" + c.toLowerCase());
        for (var i in f) c.test(i) && (e = i.replace(h, d), g[e] = f[i]);
        return g
    }

    function g(b) {
        var c = {};
        if (q[b] || (b = b.split("-")[0], q[b])) {
            var d = q[b];
            return a.each(p, function (a, b) {
                b in d && (c[b] = d[b])
            }), c
        }
    }
    var h = "&laquo;",
        i = "&raquo;",
        j = a(window),
        k = function () {
            var b = {
                get: function (a) {
                    return this.slice(a)[0]
                },
                contains: function (a) {
                    for (var b = a && a.valueOf(), c = 0, d = this.length; d > c; c++)
                        if (this[c].valueOf() === b) return c;
                    return -1
                },
                remove: function (a) {
                    this.splice(a, 1)
                },
                replace: function (b) {
                    b && (a.isArray(b) || (b = [b]), this.clear(), this.push.apply(this, b))
                },
                clear: function () {
                    this.length = 0
                },
                copy: function () {
                    var a = new k;
                    return a.replace(this), a
                }
            };
            return function () {
                var c = [];
                return c.push.apply(c, arguments), a.extend(c, b), c
            }
        }(),
        l = function (b, c) {
            this.dates = new k, this.viewDate = d(), this.focusDate = null, this._process_options(c), this.element = a(b), this.isInline = !1, this.isInput = this.element.is("input"), this.component = this.element.is(".date") ? this.element.find(".add-on, .input-group-addon, .btn") : !1, this.hasInput = this.component && this.element.find("input").length, this.component && 0 === this.component.length && (this.component = !1), this.picker = a(r.template), this._buildEvents(), this._attachEvents(), this.isInline ? this.picker.addClass("datepicker-inline").appendTo(this.element) : this.picker.addClass("datepicker-dropdown dropdown-menu"), this.o.rtl && this.picker.addClass("datepicker-rtl"), this.viewMode = this.o.startView, this.o.calendarWeeks && this.picker.find("tfoot th.today").attr("colspan", function (a, b) {
                return parseInt(b) + 1
            }), this._allow_update = !1, this.setStartDate(this._o.startDate), this.setEndDate(this._o.endDate), this.setDaysOfWeekDisabled(this.o.daysOfWeekDisabled), this.fillDow(), this.fillMonths(), this._allow_update = !0, this.update(), this.showMode(), this.isInline && this.show()
        };
    l.prototype = {
        constructor: l,
        _process_options: function (b) {
            this._o = a.extend({}, this._o, b);
            var c = this.o = a.extend({}, this._o),
                d = c.language;
            switch (q[d] || (d = d.split("-")[0], q[d] || (d = o.language)), c.language = d, c.startView) {
                case 2:
                case "decade":
                    c.startView = 2;
                    break;
                case 1:
                case "year":
                    c.startView = 1;
                    break;
                default:
                    c.startView = 0
            }
            switch (c.minViewMode) {
                case 1:
                case "months":
                    c.minViewMode = 1;
                    break;
                case 2:
                case "years":
                    c.minViewMode = 2;
                    break;
                default:
                    c.minViewMode = 0
            }
            c.startView = Math.max(c.startView, c.minViewMode), c.multidate !== !0 && (c.multidate = Number(c.multidate) || !1, c.multidate = c.multidate !== !1 ? Math.max(0, c.multidate) : 1), c.multidateSeparator = String(c.multidateSeparator), c.weekStart %= 7, c.weekEnd = (c.weekStart + 6) % 7;
            var e = r.parseFormat(c.format);
            c.startDate !== -1 / 0 && (c.startDate = c.startDate ? c.startDate instanceof Date ? this._local_to_utc(this._zero_time(c.startDate)) : r.parseDate(c.startDate, e, c.language) : -1 / 0), 1 / 0 !== c.endDate && (c.endDate = c.endDate ? c.endDate instanceof Date ? this._local_to_utc(this._zero_time(c.endDate)) : r.parseDate(c.endDate, e, c.language) : 1 / 0), c.daysOfWeekDisabled = c.daysOfWeekDisabled || [], a.isArray(c.daysOfWeekDisabled) || (c.daysOfWeekDisabled = c.daysOfWeekDisabled.split(/[,\s]*/)), c.daysOfWeekDisabled = a.map(c.daysOfWeekDisabled, function (a) {
                return parseInt(a, 10)
            });
            var f = String(c.orientation).toLowerCase().split(/\s+/g),
                g = c.orientation.toLowerCase();
            if (f = a.grep(f, function (a) {
                return /^auto|left|right|top|bottom$/.test(a)
            }), c.orientation = {
                x: "auto",
                y: "auto"
            }, g && "auto" !== g)
                if (1 === f.length) switch (f[0]) {
                    case "top":
                    case "bottom":
                        c.orientation.y = f[0];
                        break;
                    case "left":
                    case "right":
                        c.orientation.x = f[0]
                } else g = a.grep(f, function (a) {
                    return /^left|right$/.test(a)
                }), c.orientation.x = g[0] || "auto", g = a.grep(f, function (a) {
                    return /^top|bottom$/.test(a)
                }), c.orientation.y = g[0] || "auto";
            else;
        },
        _events: [],
        _secondaryEvents: [],
        _applyEvents: function (a) {
            for (var c, d, e, f = 0; f < a.length; f++) c = a[f][0], 2 === a[f].length ? (d = b, e = a[f][1]) : 3 === a[f].length && (d = a[f][1], e = a[f][2]), c.on(e, d)
        },
        _unapplyEvents: function (a) {
            for (var c, d, e, f = 0; f < a.length; f++) c = a[f][0], 2 === a[f].length ? (e = b, d = a[f][1]) : 3 === a[f].length && (e = a[f][1], d = a[f][2]), c.off(d, e)
        },
        _buildEvents: function () {
            this.isInput ? this._events = [
                [this.element, {
                    focus: a.proxy(this.show, this),
                    keyup: a.proxy(function (b) {
                        -1 === a.inArray(b.keyCode, [27, 37, 39, 38, 40, 32, 13, 9]) && this.update()
                    }, this),
                    keydown: a.proxy(this.keydown, this)
                }]
            ] : this.component && this.hasInput ? this._events = [
                [this.element.find("input"), {
                    focus: a.proxy(this.show, this),
                    keyup: a.proxy(function (b) {
                        -1 === a.inArray(b.keyCode, [27, 37, 39, 38, 40, 32, 13, 9]) && this.update()
                    }, this),
                    keydown: a.proxy(this.keydown, this)
                }],
                [this.component, {
                    click: a.proxy(this.show, this)
                }]
            ] : this.element.is("div") ? this.isInline = !0 : this._events = [
                [this.element, {
                    click: a.proxy(this.show, this)
                }]
            ], this._events.push([this.element, "*", {
                blur: a.proxy(function (a) {
                    this._focused_from = a.target
                }, this)
            }], [this.element, {
                blur: a.proxy(function (a) {
                    this._focused_from = a.target
                }, this)
            }]), this._secondaryEvents = [
                [this.picker, {
                    click: a.proxy(this.click, this)
                }],
                [a(window), {
                    resize: a.proxy(this.place, this)
                }],
                [a(document), {
                    "mousedown touchstart": a.proxy(function (a) {
                        this.element.is(a.target) || this.element.find(a.target).length || this.picker.is(a.target) || this.picker.find(a.target).length || this.hide()
                    }, this)
                }]
            ]
        },
        _attachEvents: function () {
            this._detachEvents(), this._applyEvents(this._events)
        },
        _detachEvents: function () {
            this._unapplyEvents(this._events)
        },
        _attachSecondaryEvents: function () {
            this._detachSecondaryEvents(), this._applyEvents(this._secondaryEvents)
        },
        _detachSecondaryEvents: function () {
            this._unapplyEvents(this._secondaryEvents)
        },
        _trigger: function (b, c) {
            var d = c || this.dates.get(-1),
                e = this._utc_to_local(d);
            this.element.trigger({
                type: b,
                date: e,
                dates: a.map(this.dates, this._utc_to_local),
                format: a.proxy(function (a, b) {
                    0 === arguments.length ? (a = this.dates.length - 1, b = this.o.format) : "string" == typeof a && (b = a, a = this.dates.length - 1), b = b || this.o.format;
                    var c = this.dates.get(a);
                    return r.formatDate(c, b, this.o.language)
                }, this)
            })
        },
        show: function () {
            this.isInline || this.picker.appendTo("body"), this.picker.show(), this.place(), this._attachSecondaryEvents(), this._trigger("show")
        },
        hide: function () {
            this.isInline || this.picker.is(":visible") && (this.focusDate = null, this.picker.hide().detach(), this._detachSecondaryEvents(), this.viewMode = this.o.startView, this.showMode(), this.o.forceParse && (this.isInput && this.element.val() || this.hasInput && this.element.find("input").val()) && this.setValue(), this._trigger("hide"))
        },
        remove: function () {
            this.hide(), this._detachEvents(), this._detachSecondaryEvents(), this.picker.remove(), delete this.element.data().datepicker, this.isInput || delete this.element.data().date
        },
        _utc_to_local: function (a) {
            return a && new Date(a.getTime() + 6e4 * a.getTimezoneOffset())
        },
        _local_to_utc: function (a) {
            return a && new Date(a.getTime() - 6e4 * a.getTimezoneOffset())
        },
        _zero_time: function (a) {
            return a && new Date(a.getFullYear(), a.getMonth(), a.getDate())
        },
        _zero_utc_time: function (a) {
            return a && new Date(Date.UTC(a.getUTCFullYear(), a.getUTCMonth(), a.getUTCDate()))
        },
        getDates: function () {
            return a.map(this.dates, this._utc_to_local)
        },
        getUTCDates: function () {
            return a.map(this.dates, function (a) {
                return new Date(a)
            })
        },
        getDate: function () {
            return this._utc_to_local(this.getUTCDate())
        },
        getUTCDate: function () {
            return new Date(this.dates.get(-1))
        },
        setDates: function () {
            var b = a.isArray(arguments[0]) ? arguments[0] : arguments;
            this.update.apply(this, b), this._trigger("changeDate"), this.setValue()
        },
        setUTCDates: function () {
            var b = a.isArray(arguments[0]) ? arguments[0] : arguments;
            this.update.apply(this, a.map(b, this._utc_to_local)), this._trigger("changeDate"), this.setValue()
        },
        setDate: e("setDates"),
        setUTCDate: e("setUTCDates"),
        setValue: function () {
            var a = this.getFormattedDate();
            this.isInput ? this.element.val(a).change() : this.component && this.element.find("input").val(a).change()
        },
        getFormattedDate: function (c) {
            c === b && (c = this.o.format);
            var d = this.o.language;
            return a.map(this.dates, function (a) {
                return r.formatDate(a, c, d)
            }).join(this.o.multidateSeparator)
        },
        setStartDate: function (a) {
            this._process_options({
                startDate: a
            }), this.update(), this.updateNavArrows()
        },
        setEndDate: function (a) {
            this._process_options({
                endDate: a
            }), this.update(), this.updateNavArrows()
        },
        setDaysOfWeekDisabled: function (a) {
            this._process_options({
                daysOfWeekDisabled: a
            }), this.update(), this.updateNavArrows()
        },
        place: function () {
            if (!this.isInline) {
                var b = this.picker.outerWidth(),
                    c = this.picker.outerHeight(),
                    d = 10,
                    e = j.width(),
                    f = j.height(),
                    g = j.scrollTop(),
                    h = parseInt(this.element.parents().filter(function () {
                        return "auto" !== a(this).css("z-index")
                    }).first().css("z-index")) + 10,
                    i = this.component ? this.component.parent().offset() : this.element.offset(),
                    k = this.component ? this.component.outerHeight(!0) : this.element.outerHeight(!1),
                    l = this.component ? this.component.outerWidth(!0) : this.element.outerWidth(!1),
                    m = i.left,
                    n = i.top;
                this.picker.removeClass("datepicker-orient-top datepicker-orient-bottom datepicker-orient-right datepicker-orient-left"), "auto" !== this.o.orientation.x ? (this.picker.addClass("datepicker-orient-" + this.o.orientation.x), "right" === this.o.orientation.x && (m -= b - l)) : (this.picker.addClass("datepicker-orient-left"), i.left < 0 ? m -= i.left - d : i.left + b > e && (m = e - b - d));
                var o, p, q = this.o.orientation.y;
                "auto" === q && (o = -g + i.top - c, p = g + f - (i.top + k + c), q = Math.max(o, p) === p ? "top" : "bottom"), this.picker.addClass("datepicker-orient-" + q), "top" === q ? n += k : n -= c + parseInt(this.picker.css("padding-top")), this.picker.css({
                    top: n,
                    left: m,
                    zIndex: h
                })
            }
        },
        _allow_update: !0,
        update: function () {
            if (this._allow_update) {
                var b = this.dates.copy(),
                    c = [],
                    d = !1;
                arguments.length ? (a.each(arguments, a.proxy(function (a, b) {
                    b instanceof Date && (b = this._local_to_utc(b)), c.push(b)
                }, this)), d = !0) : (c = this.isInput ? this.element.val() : this.element.data("date") || this.element.find("input").val(), c = c && this.o.multidate ? c.split(this.o.multidateSeparator) : [c], delete this.element.data().date), c = a.map(c, a.proxy(function (a) {
                    return r.parseDate(a, this.o.format, this.o.language)
                }, this)), c = a.grep(c, a.proxy(function (a) {
                    return a < this.o.startDate || a > this.o.endDate || !a
                }, this), !0), this.dates.replace(c), this.dates.length ? this.viewDate = new Date(this.dates.get(-1)) : this.viewDate < this.o.startDate ? this.viewDate = new Date(this.o.startDate) : this.viewDate > this.o.endDate && (this.viewDate = new Date(this.o.endDate)), d ? this.setValue() : c.length && String(b) !== String(this.dates) && this._trigger("changeDate"), !this.dates.length && b.length && this._trigger("clearDate"), this.fill()
            }
        },
        fillDow: function () {
            var a = this.o.weekStart,
                b = "<tr>";
            if (this.o.calendarWeeks) {
                var c = '<th class="cw">&nbsp;</th>';
                b += c, this.picker.find(".datepicker-days thead tr:first-child").prepend(c)
            }
            for (; a < this.o.weekStart + 7;) b += '<th class="dow">' + q[this.o.language].daysMin[a++ % 7] + "</th>";
            b += "</tr>", this.picker.find(".datepicker-days thead").append(b)
        },
        fillMonths: function () {
            for (var a = "", b = 0; 12 > b;) a += '<span class="month">' + q[this.o.language].monthsShort[b++] + "</span>";
            this.picker.find(".datepicker-months td").html(a)
        },
        setRange: function (b) {
            b && b.length ? this.range = a.map(b, function (a) {
                return a.valueOf()
            }) : delete this.range, this.fill()
        },
        getClassNames: function (b) {
            var c = [],
                d = this.viewDate.getUTCFullYear(),
                e = this.viewDate.getUTCMonth(),
                f = new Date;
            return b.getUTCFullYear() < d || b.getUTCFullYear() === d && b.getUTCMonth() < e ? c.push("old") : (b.getUTCFullYear() > d || b.getUTCFullYear() === d && b.getUTCMonth() > e) && c.push("new"), this.focusDate && b.valueOf() === this.focusDate.valueOf() && c.push("focused"), this.o.todayHighlight && b.getUTCFullYear() === f.getFullYear() && b.getUTCMonth() === f.getMonth() && b.getUTCDate() === f.getDate() && c.push("today"), -1 !== this.dates.contains(b) && c.push("active"), (b.valueOf() < this.o.startDate || b.valueOf() > this.o.endDate || -1 !== a.inArray(b.getUTCDay(), this.o.daysOfWeekDisabled)) && c.push("disabled"), this.range && (b > this.range[0] && b < this.range[this.range.length - 1] && c.push("range"), -1 !== a.inArray(b.valueOf(), this.range) && c.push("selected")), c
        },
        fill: function () {
            var d, e = new Date(this.viewDate),
                f = e.getUTCFullYear(),
                g = e.getUTCMonth(),
                h = this.o.startDate !== -1 / 0 ? this.o.startDate.getUTCFullYear() : -1 / 0,
                i = this.o.startDate !== -1 / 0 ? this.o.startDate.getUTCMonth() : -1 / 0,
                j = 1 / 0 !== this.o.endDate ? this.o.endDate.getUTCFullYear() : 1 / 0,
                k = 1 / 0 !== this.o.endDate ? this.o.endDate.getUTCMonth() : 1 / 0,
                l = q[this.o.language].today || q.en.today || "",
                m = q[this.o.language].clear || q.en.clear || "";
            this.picker.find(".datepicker-days thead th.datepicker-switch").text(q[this.o.language].months[g] + " " + f), this.picker.find("tfoot th.today").text(l).toggle(this.o.todayBtn !== !1), this.picker.find("tfoot th.clear").text(m).toggle(this.o.clearBtn !== !1), this.updateNavArrows(), this.fillMonths();
            var n = c(f, g - 1, 28),
                o = r.getDaysInMonth(n.getUTCFullYear(), n.getUTCMonth());
            n.setUTCDate(o), n.setUTCDate(o - (n.getUTCDay() - this.o.weekStart + 7) % 7);
            var p = new Date(n);
            p.setUTCDate(p.getUTCDate() + 42), p = p.valueOf();
            for (var s, t = []; n.valueOf() < p;) {
                if (n.getUTCDay() === this.o.weekStart && (t.push("<tr>"), this.o.calendarWeeks)) {
                    var u = new Date(+n + (this.o.weekStart - n.getUTCDay() - 7) % 7 * 864e5),
                        v = new Date(Number(u) + (11 - u.getUTCDay()) % 7 * 864e5),
                        w = new Date(Number(w = c(v.getUTCFullYear(), 0, 1)) + (11 - w.getUTCDay()) % 7 * 864e5),
                        x = (v - w) / 864e5 / 7 + 1;
                    t.push('<td class="cw">' + x + "</td>")
                }
                if (s = this.getClassNames(n), s.push("day"), this.o.beforeShowDay !== a.noop) {
                    var y = this.o.beforeShowDay(this._utc_to_local(n));
                    y === b ? y = {} : "boolean" == typeof y ? y = {
                        enabled: y
                    } : "string" == typeof y && (y = {
                        classes: y
                    }), y.enabled === !1 && s.push("disabled"), y.classes && (s = s.concat(y.classes.split(/\s+/))), y.tooltip && (d = y.tooltip)
                }
                s = a.unique(s), t.push('<td class="' + s.join(" ") + '"' + (d ? ' title="' + d + '"' : "") + ">" + n.getUTCDate() + "</td>"), n.getUTCDay() === this.o.weekEnd && t.push("</tr>"), n.setUTCDate(n.getUTCDate() + 1)
            }
            this.picker.find(".datepicker-days tbody").empty().append(t.join(""));
            var z = this.picker.find(".datepicker-months").find("th:eq(1)").text(f).end().find("span").removeClass("active");
            a.each(this.dates, function (a, b) {
                b.getUTCFullYear() === f && z.eq(b.getUTCMonth()).addClass("active")
            }), (h > f || f > j) && z.addClass("disabled"), f === h && z.slice(0, i).addClass("disabled"), f === j && z.slice(k + 1).addClass("disabled"), t = "", f = 10 * parseInt(f / 10, 10);
            var A = this.picker.find(".datepicker-years").find("th:eq(1)").text(f + "-" + (f + 9)).end().find("td");
            f -= 1;
            for (var B, C = a.map(this.dates, function (a) {
                    return a.getUTCFullYear()
            }), D = -1; 11 > D; D++) B = ["year"], -1 === D ? B.push("old") : 10 === D && B.push("new"), -1 !== a.inArray(f, C) && B.push("active"), (h > f || f > j) && B.push("disabled"), t += '<span class="' + B.join(" ") + '">' + f + "</span>", f += 1;
            A.html(t)
        },
        updateNavArrows: function () {
            if (this._allow_update) {
                var a = new Date(this.viewDate),
                    b = a.getUTCFullYear(),
                    c = a.getUTCMonth();
                switch (this.viewMode) {
                    case 0:
                        this.picker.find(".prev").css(this.o.startDate !== -1 / 0 && b <= this.o.startDate.getUTCFullYear() && c <= this.o.startDate.getUTCMonth() ? {
                            visibility: "hidden"
                        } : {
                            visibility: "visible"
                        }), this.picker.find(".next").css(1 / 0 !== this.o.endDate && b >= this.o.endDate.getUTCFullYear() && c >= this.o.endDate.getUTCMonth() ? {
                            visibility: "hidden"
                        } : {
                            visibility: "visible"
                        });
                        break;
                    case 1:
                    case 2:
                        this.picker.find(".prev").css(this.o.startDate !== -1 / 0 && b <= this.o.startDate.getUTCFullYear() ? {
                            visibility: "hidden"
                        } : {
                            visibility: "visible"
                        }), this.picker.find(".next").css(1 / 0 !== this.o.endDate && b >= this.o.endDate.getUTCFullYear() ? {
                            visibility: "hidden"
                        } : {
                            visibility: "visible"
                        })
                }
            }
        },
        click: function (b) {
            b.preventDefault();
            var d, e, f, g = a(b.target).closest("span, td, th");
            if (1 === g.length) switch (g[0].nodeName.toLowerCase()) {
                case "th":
                    switch (g[0].className) {
                        case "datepicker-switch":
                            this.showMode(1);
                            break;
                        case "prev":
                        case "next":
                            var h = r.modes[this.viewMode].navStep * ("prev" === g[0].className ? -1 : 1);
                            switch (this.viewMode) {
                                case 0:
                                    this.viewDate = this.moveMonth(this.viewDate, h), this._trigger("changeMonth", this.viewDate);
                                    break;
                                case 1:
                                case 2:
                                    this.viewDate = this.moveYear(this.viewDate, h), 1 === this.viewMode && this._trigger("changeYear", this.viewDate)
                            }
                            this.fill();
                            break;
                        case "today":
                            var i = new Date;
                            i = c(i.getFullYear(), i.getMonth(), i.getDate(), 0, 0, 0), this.showMode(-2);
                            var j = "linked" === this.o.todayBtn ? null : "view";
                            this._setDate(i, j);
                            break;
                        case "clear":
                            var k;
                            this.isInput ? k = this.element : this.component && (k = this.element.find("input")), k && k.val("").change(), this.update(), this._trigger("changeDate"), this.o.autoclose && this.hide()
                    }
                    break;
                case "span":
                    g.is(".disabled") || (this.viewDate.setUTCDate(1), g.is(".month") ? (f = 1, e = g.parent().find("span").index(g), d = this.viewDate.getUTCFullYear(), this.viewDate.setUTCMonth(e), this._trigger("changeMonth", this.viewDate), 1 === this.o.minViewMode && this._setDate(c(d, e, f))) : (f = 1, e = 0, d = parseInt(g.text(), 10) || 0, this.viewDate.setUTCFullYear(d), this._trigger("changeYear", this.viewDate), 2 === this.o.minViewMode && this._setDate(c(d, e, f))), this.showMode(-1), this.fill());
                    break;
                case "td":
                    g.is(".day") && !g.is(".disabled") && (f = parseInt(g.text(), 10) || 1, d = this.viewDate.getUTCFullYear(), e = this.viewDate.getUTCMonth(), g.is(".old") ? 0 === e ? (e = 11, d -= 1) : e -= 1 : g.is(".new") && (11 === e ? (e = 0, d += 1) : e += 1), this._setDate(c(d, e, f)))
            }
            this.picker.is(":visible") && this._focused_from && a(this._focused_from).focus(), delete this._focused_from
        },
        _toggle_multidate: function (a) {
            var b = this.dates.contains(a);
            if (a ? -1 !== b ? this.dates.remove(b) : this.dates.push(a) : this.dates.clear(), "number" == typeof this.o.multidate)
                for (; this.dates.length > this.o.multidate;) this.dates.remove(0)
        },
        _setDate: function (a, b) {
            b && "date" !== b || this._toggle_multidate(a && new Date(a)), b && "view" !== b || (this.viewDate = a && new Date(a)), this.fill(), this.setValue(), this._trigger("changeDate");
            var c;
            this.isInput ? c = this.element : this.component && (c = this.element.find("input")), c && c.change(), !this.o.autoclose || b && "date" !== b || this.hide()
        },
        moveMonth: function (a, c) {
            if (!a) return b;
            if (!c) return a;
            var d, e, f = new Date(a.valueOf()),
                g = f.getUTCDate(),
                h = f.getUTCMonth(),
                i = Math.abs(c);
            if (c = c > 0 ? 1 : -1, 1 === i) e = -1 === c ? function () {
                return f.getUTCMonth() === h
            } : function () {
                return f.getUTCMonth() !== d
            }, d = h + c, f.setUTCMonth(d), (0 > d || d > 11) && (d = (d + 12) % 12);
            else {
                for (var j = 0; i > j; j++) f = this.moveMonth(f, c);
                d = f.getUTCMonth(), f.setUTCDate(g), e = function () {
                    return d !== f.getUTCMonth()
                }
            }
            for (; e() ;) f.setUTCDate(--g), f.setUTCMonth(d);
            return f
        },
        moveYear: function (a, b) {
            return this.moveMonth(a, 12 * b)
        },
        dateWithinRange: function (a) {
            return a >= this.o.startDate && a <= this.o.endDate
        },
        keydown: function (a) {
            if (this.picker.is(":not(:visible)")) return void (27 === a.keyCode && this.show());
            var b, c, e, f = !1,
                g = this.focusDate || this.viewDate;
            switch (a.keyCode) {
                case 27:
                    this.focusDate ? (this.focusDate = null, this.viewDate = this.dates.get(-1) || this.viewDate, this.fill()) : this.hide(), a.preventDefault();
                    break;
                case 37:
                case 39:
                    if (!this.o.keyboardNavigation) break;
                    b = 37 === a.keyCode ? -1 : 1, a.ctrlKey ? (c = this.moveYear(this.dates.get(-1) || d(), b), e = this.moveYear(g, b), this._trigger("changeYear", this.viewDate)) : a.shiftKey ? (c = this.moveMonth(this.dates.get(-1) || d(), b), e = this.moveMonth(g, b), this._trigger("changeMonth", this.viewDate)) : (c = new Date(this.dates.get(-1) || d()), c.setUTCDate(c.getUTCDate() + b), e = new Date(g), e.setUTCDate(g.getUTCDate() + b)), this.dateWithinRange(c) && (this.focusDate = this.viewDate = e, this.setValue(), this.fill(), a.preventDefault());
                    break;
                case 38:
                case 40:
                    if (!this.o.keyboardNavigation) break;
                    b = 38 === a.keyCode ? -1 : 1, a.ctrlKey ? (c = this.moveYear(this.dates.get(-1) || d(), b), e = this.moveYear(g, b), this._trigger("changeYear", this.viewDate)) : a.shiftKey ? (c = this.moveMonth(this.dates.get(-1) || d(), b), e = this.moveMonth(g, b), this._trigger("changeMonth", this.viewDate)) : (c = new Date(this.dates.get(-1) || d()), c.setUTCDate(c.getUTCDate() + 7 * b), e = new Date(g), e.setUTCDate(g.getUTCDate() + 7 * b)), this.dateWithinRange(c) && (this.focusDate = this.viewDate = e, this.setValue(), this.fill(), a.preventDefault());
                    break;
                case 32:
                    break;
                case 13:
                    g = this.focusDate || this.dates.get(-1) || this.viewDate, this._toggle_multidate(g), f = !0, this.focusDate = null, this.viewDate = this.dates.get(-1) || this.viewDate, this.setValue(), this.fill(), this.picker.is(":visible") && (a.preventDefault(), this.o.autoclose && this.hide());
                    break;
                case 9:
                    this.focusDate = null, this.viewDate = this.dates.get(-1) || this.viewDate, this.fill(), this.hide()
            }
            if (f) {
                this._trigger(this.dates.length ? "changeDate" : "clearDate");
                var h;
                this.isInput ? h = this.element : this.component && (h = this.element.find("input")), h && h.change()
            }
        },
        showMode: function (a) {
            a && (this.viewMode = Math.max(this.o.minViewMode, Math.min(2, this.viewMode + a))), this.picker.find(">div").hide().filter(".datepicker-" + r.modes[this.viewMode].clsName).css("display", "block"), this.updateNavArrows()
        }
    };
    var m = function (b, c) {
        this.element = a(b), this.inputs = a.map(c.inputs, function (a) {
            return a.jquery ? a[0] : a
        }), delete c.inputs, a(this.inputs).datepicker(c).bind("changeDate", a.proxy(this.dateUpdated, this)), this.pickers = a.map(this.inputs, function (b) {
            return a(b).data("datepicker")
        }), this.updateDates()
    };
    m.prototype = {
        updateDates: function () {
            this.dates = a.map(this.pickers, function (a) {
                return a.getUTCDate()
            }), this.updateRanges()
        },
        updateRanges: function () {
            var b = a.map(this.dates, function (a) {
                return a.valueOf()
            });
            a.each(this.pickers, function (a, c) {
                c.setRange(b)
            })
        },
        dateUpdated: function (b) {
            if (!this.updating) {
                this.updating = !0;
                var c = a(b.target).data("datepicker"),
                    d = c.getUTCDate(),
                    e = a.inArray(b.target, this.inputs),
                    f = this.inputs.length;
                if (-1 !== e) {
                    if (a.each(this.pickers, function (a, b) {
                        b.getUTCDate() || b.setUTCDate(d)
                    }), d < this.dates[e])
                        for (; e >= 0 && d < this.dates[e];) this.pickers[e--].setUTCDate(d);
                    else if (d > this.dates[e])
                        for (; f > e && d > this.dates[e];) this.pickers[e++].setUTCDate(d);
                    this.updateDates(), delete this.updating
                }
            }
        },
        remove: function () {
            a.map(this.pickers, function (a) {
                a.remove()
            }), delete this.element.data().datepicker
        }
    };
    var n = a.fn.datepicker;
    a.fn.datepicker = function (c) {
        var d = Array.apply(null, arguments);
        d.shift();
        var e;
        return this.each(function () {
            var h = a(this),
                i = h.data("datepicker"),
                j = "object" == typeof c && c;
            if (!i) {
                var k = f(this, "date"),
                    n = a.extend({}, o, k, j),
                    p = g(n.language),
                    q = a.extend({}, o, p, k, j);
                if (h.is(".input-daterange") || q.inputs) {
                    var r = {
                        inputs: q.inputs || h.find("input").toArray()
                    };
                    h.data("datepicker", i = new m(this, a.extend(q, r)))
                } else h.data("datepicker", i = new l(this, q))
            }
            return "string" == typeof c && "function" == typeof i[c] && (e = i[c].apply(i, d), e !== b) ? !1 : void 0
        }), e !== b ? e : this
    };
    var o = a.fn.datepicker.defaults = {
        autoclose: !1,
        beforeShowDay: a.noop,
        calendarWeeks: !1,
        clearBtn: !1,
        daysOfWeekDisabled: [],
        endDate: 1 / 0,
        forceParse: !0,
        format: "mm/dd/yyyy",
        keyboardNavigation: !0,
        language: "en",
        minViewMode: 0,
        multidate: !1,
        multidateSeparator: ",",
        orientation: "auto",
        rtl: !1,
        startDate: -1 / 0,
        startView: 0,
        todayBtn: !1,
        todayHighlight: !1,
        weekStart: 0
    }, p = a.fn.datepicker.locale_opts = ["format", "rtl", "weekStart"];
    a.fn.datepicker.Constructor = l;
    var q = a.fn.datepicker.dates = {
        en: {
            days: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"],
            daysShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
            daysMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa", "Su"],
            months: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
            monthsShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            today: "Today",
            clear: "Clear"
        }
    }, r = {
        modes: [{
            clsName: "days",
            navFnc: "Month",
            navStep: 1
        }, {
            clsName: "months",
            navFnc: "FullYear",
            navStep: 1
        }, {
            clsName: "years",
            navFnc: "FullYear",
            navStep: 10
        }],
        isLeapYear: function (a) {
            return a % 4 === 0 && a % 100 !== 0 || a % 400 === 0
        },
        getDaysInMonth: function (a, b) {
            return [31, r.isLeapYear(a) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][b]
        },
        validParts: /dd?|DD?|mm?|MM?|yy(?:yy)?/g,
        nonpunctuation: /[^ -\/:-@\[\u3400-\u9fff-`{-~\t\n\r]+/g,
        parseFormat: function (a) {
            var b = a.replace(this.validParts, "\x00").split("\x00"),
                c = a.match(this.validParts);
            if (!b || !b.length || !c || 0 === c.length) throw new Error("Invalid date format.");
            return {
                separators: b,
                parts: c
            }
        },
        parseDate: function (d, e, f) {
            function g() {
                var a = this.slice(0, m[j].length),
                    b = m[j].slice(0, a.length);
                return a === b
            }
            if (!d) return b;
            if (d instanceof Date) return d;
            "string" == typeof e && (e = r.parseFormat(e));
            var h, i, j, k = /([\-+]\d+)([dmwy])/,
                m = d.match(/([\-+]\d+)([dmwy])/g);
            if (/^[\-+]\d+[dmwy]([\s,]+[\-+]\d+[dmwy])*$/.test(d)) {
                for (d = new Date, j = 0; j < m.length; j++) switch (h = k.exec(m[j]), i = parseInt(h[1]), h[2]) {
                    case "d":
                        d.setUTCDate(d.getUTCDate() + i);
                        break;
                    case "m":
                        d = l.prototype.moveMonth.call(l.prototype, d, i);
                        break;
                    case "w":
                        d.setUTCDate(d.getUTCDate() + 7 * i);
                        break;
                    case "y":
                        d = l.prototype.moveYear.call(l.prototype, d, i)
                }
                return c(d.getUTCFullYear(), d.getUTCMonth(), d.getUTCDate(), 0, 0, 0)
            }
            m = d && d.match(this.nonpunctuation) || [], d = new Date;
            var n, o, p = {}, s = ["yyyy", "yy", "M", "MM", "m", "mm", "d", "dd"],
                t = {
                    yyyy: function (a, b) {
                        return a.setUTCFullYear(b)
                    },
                    yy: function (a, b) {
                        return a.setUTCFullYear(2e3 + b)
                    },
                    m: function (a, b) {
                        if (isNaN(a)) return a;
                        for (b -= 1; 0 > b;) b += 12;
                        for (b %= 12, a.setUTCMonth(b) ; a.getUTCMonth() !== b;) a.setUTCDate(a.getUTCDate() - 1);
                        return a
                    },
                    d: function (a, b) {
                        return a.setUTCDate(b)
                    }
                };
            t.M = t.MM = t.mm = t.m, t.dd = t.d, d = c(d.getFullYear(), d.getMonth(), d.getDate(), 0, 0, 0);
            var u = e.parts.slice();
            if (m.length !== u.length && (u = a(u).filter(function (b, c) {
                return -1 !== a.inArray(c, s)
            }).toArray()), m.length === u.length) {
                var v;
                for (j = 0, v = u.length; v > j; j++) {
                    if (n = parseInt(m[j], 10), h = u[j], isNaN(n)) switch (h) {
                        case "MM":
                            o = a(q[f].months).filter(g), n = a.inArray(o[0], q[f].months) + 1;
                            break;
                        case "M":
                            o = a(q[f].monthsShort).filter(g), n = a.inArray(o[0], q[f].monthsShort) + 1
                    }
                    p[h] = n
                }
                var w, x;
                for (j = 0; j < s.length; j++) x = s[j], x in p && !isNaN(p[x]) && (w = new Date(d), t[x](w, p[x]), isNaN(w) || (d = w))
            }
            return d
        },
        formatDate: function (b, c, d) {
            if (!b) return "";
            "string" == typeof c && (c = r.parseFormat(c));
            var e = {
                d: b.getUTCDate(),
                D: q[d].daysShort[b.getUTCDay()],
                DD: q[d].days[b.getUTCDay()],
                m: b.getUTCMonth() + 1,
                M: q[d].monthsShort[b.getUTCMonth()],
                MM: q[d].months[b.getUTCMonth()],
                yy: b.getUTCFullYear().toString().substring(2),
                yyyy: b.getUTCFullYear()
            };
            e.dd = (e.d < 10 ? "0" : "") + e.d, e.mm = (e.m < 10 ? "0" : "") + e.m, b = [];
            for (var f = a.extend([], c.separators), g = 0, h = c.parts.length; h >= g; g++) f.length && b.push(f.shift()), b.push(e[c.parts[g]]);
            return b.join("")
        },
        headTemplate: '<thead><tr><th class="prev">' + h + '</th><th colspan="5" class="datepicker-switch"></th><th class="next">' + i + "</th></tr></thead>",
        contTemplate: '<tbody><tr><td colspan="7"></td></tr></tbody>',
        footTemplate: '<tfoot><tr><th colspan="7" class="today"></th></tr><tr><th colspan="7" class="clear"></th></tr></tfoot>'
    };
    r.template = '<div class="datepicker"><div class="datepicker-days"><table class=" table-condensed">' + r.headTemplate + "<tbody></tbody>" + r.footTemplate + '</table></div><div class="datepicker-months"><table class="table-condensed">' + r.headTemplate + r.contTemplate + r.footTemplate + '</table></div><div class="datepicker-years"><table class="table-condensed">' + r.headTemplate + r.contTemplate + r.footTemplate + "</table></div></div>", a.fn.datepicker.DPGlobal = r, a.fn.datepicker.noConflict = function () {
        return a.fn.datepicker = n, this
    }, a(document).on("focus.datepicker.data-api click.datepicker.data-api", '[data-provide="datepicker"]', function (b) {
        var c = a(this);
        c.data("datepicker") || (b.preventDefault(), c.datepicker("show"))
    }), a(function () {
        a('[data-provide="datepicker-inline"]').datepicker()
    })
}(window.jQuery);
