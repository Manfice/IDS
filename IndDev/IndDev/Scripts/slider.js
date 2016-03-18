//Start slider code
var imgCount = 4;
var showDuration = 10;
var curImg;
var vect = new Array(imgCount + 10);

function Load() {
    curImg = 0;
    StartValue();
    for (var i = 1; i < imgCount; i++) {
        vect[i] = document.getElementById("img" + (i + 1));
        $("#D" + (i - 1)).removeClass("activeDot");
        $("#Title" + (i - 1)).addClass("hidden");
        $("#Title" + (i - 1)).removeClass("showed");
        $("#Descr" + (i - 1)).addClass("hidden");
    }
    $("#D" + curImg).addClass("activeDot");
    $("#Title" + curImg).removeClass("hidden");
    $("#Title" + curImg).addClass("showed");
    $("#Descr" + curImg).removeClass("hidden");

    window.moveOnTimer = setInterval(Timer, showDuration * 1000);
}

function Timer() {
    curImg++;
    if (curImg === imgCount) {
        curImg = 0;
    }
    Effect();
}

function StartValue() {
    vect[curImg] = document.getElementById("img1");
    vect[curImg].style.opacity = 1;
    vect[curImg].style.visibility = "visible";
}

function MoveNext() {
    curImg++;
    if (curImg === imgCount) {
        curImg = 0;
    }
    Effect();

    clearInterval(moveOnTimer);
    window.moveOnTimer = setInterval(Timer, showDuration * 1000);
}
function MovePriv() {
    curImg--;
    if (curImg === -1) {
        curImg = imgCount - 1;
    }
    Effect();
    clearInterval(moveOnTimer);
    window.moveOnTimer = setInterval(Timer, showDuration * 1000);
}

function Effect() {
    for (var i = 0; i < imgCount; i++) {
        vect[i].style.opacity = 0;
        vect[i].style.visibility = "hidden";
        $("#D" + i).removeClass("activeDot");
        $("#Title" + i).addClass("hidden");
        $("#Title" + i).removeClass("showed");
        $("#Descr" + i).addClass("hidden");
    }
    vect[curImg].style.opacity = 1;
    vect[curImg].style.visibility = "visible";
    $("#D" + curImg).addClass("activeDot");
    $("#Title" + curImg).removeClass("hidden");
    $("#Title" + curImg).addClass("showed");
    $("#Descr" + curImg).removeClass("hidden");
}

function DotClick(dot) {
    curImg = parseInt(dot);
    for (var i = 0; i < imgCount; i++) {
        vect[i].style.opacity = 0;
        vect[i].style.visibility = "hidden";
        $("#D" + i).removeClass("activeDot");
        $("#Title" + i).addClass("hidden");
        $("#Title" + i).removeClass("showed");
        $("#Descr" + i).addClass("hidden");
    }
    vect[curImg].style.opacity = 1;
    vect[curImg].style.visibility = "visible";
    $("#D" + curImg).addClass("activeDot");
    $("#Title" + curImg).removeClass("hidden");
    $("#Title" + curImg).addClass("showed");
    $("#Descr" + curImg).removeClass("hidden");

    clearInterval(moveOnTimer);
}