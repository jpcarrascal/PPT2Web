// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function adjust_size() {
    var wid = $(".slide, .active").width();
    var hei = $(".slide, .active").height();
    var imghei = $(".slide-img").first().height();
    var imgwid = $(".slide-img").first().width();
    var deckwid = $("#deck").width();
    var deckhei = $("#deck").height();
    if (imghei > deckhei) {
        $(".slide-img").height(deckhei);
        $(".slide-img").width("auto");
        console.log("IT'S HAPPENING!");
    }
    else if (imgwid < $("body").width()) {
        $(".slide-img").height("100%");
        $(".slide-img").width("auto");
    }
    else {
        $(".slide-img").width("100%");
        $(".slide-img").height("auto");
    }
    console.log("slide height: " + hei + " deck height: " + deckhei + " img height: " + imghei);
}

function update_buttons(current, total) {
    if (current == 1) {
        //&#9664; <br />PREV
        $("#prev").find(".nav-text:first").html("<span class='times'>&times;</span><br />FIRST");
        console.log("At FIRST one.");
    }
    else if (current == total) {
        //&#9654;<br />NEXT
        $("#next").find(".nav-text:first").html("<span class='times'>&times;</span><br />LAST");
        console.log("At LAST one.");
    }
    else
    {
        if ($("#prev").find(".nav-text:first").html() != "&#9664; <br />PREV")
            $("#prev").find(".nav-text:first").html("&#9664;<br />PREV");
        if ($("#next").find(".nav-text:first").html() != "&#9654;<br />NEXT")
            $("#next").find(".nav-text:first").html("&#9654;<br />NEXT");
    }
}

function adjust_text_size() {
    $(".text").each(function (i) {
        //var nlines = a.value.split("<br />").length;
    });
}

$(document).ready(function () {

    // Prevent image download.
    $('img').mousedown(function (e) {
        if (e.button == 2) {
            return false;
        }
    });

    adjust_size();

    $(window).resize(function () {
        adjust_size();
    });

    $(".nav-button").on('click', function () {
        var current = parseInt($("#deck").attr("current-slide"));
        var total = parseInt($("#deck").attr("total-slides"));
        var canMove = false;
        var moveTo = current;
        if ($(this).attr("id") == "prev") {
            if (current > 1) {
                moveTo = current - 1;
                canMove = true;
            }
        }
        else {
            if (current < total) {
                moveTo = current + 1;
                canMove = true;
            }
        }
        if (canMove) {
            $("#slide-" + current).removeClass("active");
            $("#slide-" + current).addClass("inactive");
            $("#slide-" + moveTo).removeClass("inactive");
            $("#slide-" + moveTo).addClass("active");

            $("#text-" + current).removeClass("active");
            $("#text-" + current).addClass("inactive");
            $("#text-" + moveTo).removeClass("inactive");
            $("#text-" + moveTo).addClass("active");

            $("#deck").attr("current-slide", moveTo);
            console.log("Moving to slide " + moveTo);
            update_buttons(moveTo, total);
        }
    });
});
