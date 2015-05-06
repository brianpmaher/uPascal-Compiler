// JavaScript Document

var mainBoxEntry = document.getElementById("mainBoxEntry");
var entryText    = document.getElementById("entryText");
var mainBox      = document.getElementById("mainBox");
var mainBoxVeil  = document.getElementById("mainBoxVeil");

setTimeout("entryMessageFadeIn()",       0);
setTimeout("entryMessageFadeOut()",   5000);
setTimeout("thoughtspringsFadeIn()", 10000);
setTimeout("navIlluminate('nav1')",  16000);
setTimeout("navIlluminate('nav2')",  17000);
setTimeout("navIlluminate('nav3')",  18000);
// setTimeout("menuFadeIn()",           18000);

function entryMessageFadeIn(){
	mainBoxEntry.style.visibility = "visible";
    mainBox.style.visibility      = "hidden"
    opacityFadeInOut("mainBoxVeil", 100, 0, 5000);
}

function entryMessageFadeOut(){
    opacityFadeInOut("mainBoxVeil", 0, 100, 5000);
}

function thoughtspringsFadeIn() {
   mainBoxVeil.style.opacity = "1";
   mainBoxEntry.style.visibility = "hidden";
   entryText.style.visibility = "hidden";
   mainBox.style.visibility      = "visible";
   opacityFadeInOut("mainBoxVeil", 100, 0, 8000);
}

function navIlluminate(id){
	var nav = document.getElementById(id);
	colorFadeInOut(id, 0, 5000, 2000);
}