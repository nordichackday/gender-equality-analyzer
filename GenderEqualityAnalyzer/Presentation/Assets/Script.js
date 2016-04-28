var gender = gender || {};


gender.actions = {
    ToggleMainMenu: function ($me) {
        var animationSpeed = 500;
        var sliderWidth = $me.width();

        //check if slider is collapsed
        var isCollapsed = $me.prev().hasClass("active");

        //minus margin or positive margin
        var sign = (isCollapsed) ? "+" : "-";
        $me.animate({ "margin-right": sign + "=" + sliderWidth }, animationSpeed);
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




    }
}

gender.listners = {
    MenuButton: function () {
        $(".menu-button").on("click", function () {
            var $me = $(this);
            $me.toggleClass("active");
            gender.actions.ToggleMainMenu($me.next());
        });
    },

    StatisticsAnimation: function() {
        $("[data-action='counter']").each(function (index) {
            var $me = $(this);
            gender.actions.Counter($me);
        });
    }
}


gender.init = function () {
    gender.listners.MenuButton();
    setTimeout(function() {
        gender.listners.StatisticsAnimation();
    }, 1500);
   
    setTimeout(function() {
        gender.actions.RemoveAnimation();
    }, 2000);
}


$(function () {
    "use strict";
    gender.init();
});
