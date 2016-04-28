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
    }
}

gender.listners = {
    MenuButton: function () {
        $(".menu-button").on("click", function () {
            var $me = $(this);
            $me.toggleClass("active");
            gender.actions.ToggleMainMenu($me.next());
        });
    }
}


gender.init = function () {
    gender.listners.MenuButton();
    setTimeout(function() {
        gender.actions.RemoveAnimation();
    }, 2000);
}


$(function () {
    "use strict";
    gender.init();
});
