var gender = gender || {};


gender.actions = {
    ToggleMainMenu: function () {
        var $button = $(".menu-button");
        $button.toggleClass("active");

        var $menu = $button.next();
        var animationSpeed = 500;
        var sliderWidth = $menu.width();

        //check if slider is collapsed
        var isCollapsed = $button.hasClass("active");

        //minus margin or positive margin
        var sign = (isCollapsed) ? "+" : "-";
        $menu.animate({ "margin-right": sign + "=" + sliderWidth }, animationSpeed);
    },

    RemoveAnimation: function() {
        var $me = $("#site-icon");
        $me.removeClass("fa-spin");
    },

    Counter: function ($me) {
        var currentValue = parseInt($me.text(),10);
        var total = $me.data("total");

        if(currentValue < total) {
            $me.text(currentValue + 1);
            setTimeout(function() {
                gender.actions.Counter($me);
            }, 10);
        }
    },

    AddSpinner: function () {
        var $content = $(".page-wrapper");
        $content.html("<div id=\"waiting\"><i class=\"fa fa-venus-mars fa-spin\"></i></div>");
        $content.removeClass("fade-out").addClass("fade-in");

    },

    AjaxWorker: function(url, callback) {

        gender.actions.AddSpinner();
        $.ajax({
                url: url
            })
            .done(function(data) {
                gender.actions.AddSpinner();
                var $content = $(".page-wrapper");
                $content.html(data);
                callback();
            });
    }
}

gender.listners = {
    MenuButton: function () {
        $(".menu-button").on("click", function () {
            gender.actions.ToggleMainMenu();
        });
    },

    StatisticsAnimation: function () {
        setTimeout(function() {
            $("[data-action='counter']").each(function(index) {
                var $me = $(this);
                gender.actions.Counter($me);
            });
        }, 500);
    },

    StatisticsGenderIconAnimation: function() {
        $($(".gender-icons-container i").get().reverse()).each(function (index) {
            var $me = $(this);
            setTimeout(function() {
                $me.removeClass("visibility-hidden");
            }, index * 10);
        });
    }
}


gender.init = function () {
    gender.listners.MenuButton();
    gender.listners.StatisticsAnimation();
}


$(function () {
    "use strict";
    gender.init();
});
