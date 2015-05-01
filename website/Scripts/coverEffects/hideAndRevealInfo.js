// JavaScript Document

var help = document.getElementById("help");
var video = document.getElementById("video");
var author = document.getElementById("author");
var helpInfo = document.getElementById("helpInfo");
var videoInfo = document.getElementById("videoInfo");
var authorInfo = document.getElementById("authorInfo");

help.onmouseover = revealHelpInfo;
video.onmouseover = revealVideoInfo;
author.onmouseover = revealAuthorInfo;
helpInfo.onmouseout = hideHelpInfo;
videoInfo.onmouseout = hideVideoInfo;
authorInfo.onmouseout = hideAuthorInfo;


function revealHelpInfo(){
	video.style.visibility = "hidden";
    author.style.visibility = "hidden";
 	helpInfo.style.visibility = "visible";
 	helpInfo.style.overflow = "auto";
	helpInfo.style.height = "100px";
}

function revealVideoInfo(){
	help.style.visibility = "hidden";
    author.style.visibility = "hidden";
	videoInfo.style.visibility = "visible";
	videoInfo.style.overflow = "auto";
	videoInfo.style.height = "100px";
}

function revealAuthorInfo(){
	help.style.visibility = "hidden";
    video.style.visibility = "hidden";
	authorInfo.style.visibility = "visible";
	authorInfo.style.overflow = "auto";
	authorInfo.style.height = "100px";
}

function hideHelpInfo(){
	video.style.visibility = "visible";
    author.style.visibility = "visible";
 	helpInfo.style.visibility = "hidden";
 	helpInfo.style.overflow = "hidden";
	helpInfo.style.height = "0px";
}

function hideVideoInfo(){
	help.style.visibility = "visible";
    author.style.visibility = "visible";
	videoInfo.style.visibility = "hidden";
	videoInfo.style.overflow = "hidden";
	videoInfo.style.height = "0px";
}

function hideAuthorInfo(){
	help.style.visibility = "visible";
    video.style.visibility = "visible";
	authorInfo.style.visibility = "hidden";
	authorInfo.style.overflow = "hidden";
	authorInfo.style.height = "0px";
}
