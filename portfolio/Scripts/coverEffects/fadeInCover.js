// JavaScript Document

var coverVeilId   = "cover-veil";
var coverHeaderId = "cover-header";
var leftBarId     = "cover-left-bar";

setTimeout("colorFadeInOut(leftBarId,    0,   16777215, 3000)", 500);
setTimeout("colorFadeInOut(coverHeaderId,       0, 16777215, 3000)",    1500);
setTimeout("coverFadeIn(coverVeilId)",      4000);
/*
setTimeout("entryMessageFadeOut()",   5000);
setTimeout("thoughtspringsFadeIn()", 10000);
setTimeout("navIlluminate('nav1')",  16000);
setTimeout("navIlluminate('nav2')",  17000);
setTimeout("navIlluminate('nav3')",  18000);
*/
// setTimeout("menuFadeIn()",           18000);

function headerFadeInOut(id){
	colorFadeInOut()
}
	
function coverFadeIn(id){
	    opacityFadeInOut(coverVeil, 100, 0, 5000);
}

function entryMessageFadeOut(id){
    opacityFadeInOut(id, 0, 100, 5000);
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