"use strict";
var Modal = (function () {
    function Modal(modal) {
        this.modal = modal;
        this.isIE = (new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})").exec(navigator.userAgent) != null) ? true : false;
        this.ieVersion = (new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})").exec(navigator.userAgent) != null) ? parseFloat(RegExp.$1) : -1;
        this.opened = false;
        this.timer = 0;
        this.dialog = modal.querySelector(".modal-dialog");
        this.init();
    }
    Modal.prototype.open = function () {
        this._open();
    };
    Modal.prototype.close = function () {
        this._close();
    };
    Modal.prototype.init = function () {
        //if (this.options.content && typeof this.options.content !== "undefined") {
        //	this.setContent(this.options.content);
        //}
        this.dismiss();
        this.keydown();
        this.trigger();
        if (!(this.isIE && this.ieVersion < 9)) {
            this.resize();
        }
    };
    Modal.prototype._open = function () {
        var that = this;
        //if (this.options.backdrop) {
        //	this.createOverlay();
        //} else { 
        //	this.overlay = null; 
        //}
        document.body.classList.add("modal-open");
        this.modal.classList.add("show");
        clearTimeout(parseInt(that.modal.getAttribute("data-timer")));
        this.timer = setTimeout(function () {
            if (that.overlay !== null) {
                that._resize();
                that.overlay.classList.add("in");
            }
            that.modal.classList.add("in");
            that.modal.setAttribute("aria-hidden", "false");
        }, 0); //that.options.duration/2);
        this.modal.setAttribute("data-timer", that.timer.toString());
        this.opened = true;
    };
    Modal.prototype._close = function () {
        var that = this;
        this.modal.classList.remove("in");
        this.modal.setAttribute("aria-hidden", "true");
        if (this.overlay) {
            this.overlay.classList.remove("in");
        }
        document.body.classList.remove("modal-open");
        clearTimeout(parseInt(that.modal.getAttribute("data-timer")));
        this.timer = setTimeout(function () {
            that.modal.classList.remove("show");
            that.removeOverlay();
        }, 0); //that.options.duration/2);
        this.modal.setAttribute("data-timer", that.timer.toString());
        this.opened = false;
    };
    Modal.prototype.setContent = function (content) {
        this.modal.querySelector(".modal-content").innerHTML = content;
    };
    Modal.prototype.createOverlay = function () {
        var backdrop = document.createElement("div"), overlay = document.querySelector(".modal-backdrop");
        backdrop.setAttribute("class", "modal-backdrop fade");
        if (overlay) {
            this.overlay = overlay;
        }
        else {
            this.overlay = backdrop;
            document.body.appendChild(backdrop);
        }
    };
    Modal.prototype.removeOverlay = function () {
        var overlay = document.querySelector(".modal-backdrop");
        if (overlay !== null && typeof overlay !== "undefined") {
            document.body.removeChild(overlay);
        }
    };
    Modal.prototype.keydown = function () {
        var that = this;
        //document.addEventListener("keydown", (event:KeyboardEvent) => {
        //	if (that.options.keyboard && event.which == 27) {
        //		that.close();
        //	}
        //}, false);
    };
    Modal.prototype.trigger = function () {
        var that = this;
        var triggers = document.querySelectorAll("[data-toggle=\"modal\"]"), tgl = triggers.length, i = 0;
        for (i; i < tgl; i++) {
            triggers[i].addEventListener("click", function (event) {
                var b = event.target, s = b.getAttribute("data-target") && b.getAttribute("data-target").replace("#", "")
                    || b.getAttribute("href") && b.getAttribute("href").replace("#", "");
                if (document.getElementById(s) === that.modal) {
                    that.open();
                }
            });
        }
    };
    Modal.prototype._resize = function () {
        var that = this;
        var overlay = (this.overlay || document.querySelector(".modal-backdrop")), dim = { w: document.documentElement.clientWidth + "px", h: document.documentElement.clientHeight + "px" };
        setTimeout(function () {
            if (overlay !== null && /in/.test(overlay.className)) {
                overlay.style.height = dim.h;
                overlay.style.width = dim.w;
            }
        }, 0); //that.options.duration/2);
    };
    Modal.prototype.resize = function () {
        var that = this;
        window.addEventListener("resize", function () {
            setTimeout(function () {
                that._resize();
            }, 100);
        }, false);
    };
    Modal.prototype.dismiss = function () {
        var that = this;
        this.modal.addEventListener("click", function (e) {
            var target = e.target;
            if (target.parentNode.getAttribute("data-dismiss") === "modal" || target.getAttribute("data-dismiss") === "modal" || e.target === that.modal) {
                e.preventDefault();
                that.close();
            }
        });
    };
    return Modal;
}());
exports.Modal = Modal;
//# sourceMappingURL=modal.js.map