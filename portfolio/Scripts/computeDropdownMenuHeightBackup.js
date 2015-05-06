// JavaScript Document
// This function fires at the time a page is loaded and each time the browser window is resized.
// The purpose of this function is to set the height of the dropdown menu lists to match the content (article) window height,
// so that dropdown menu lists that are shorter than the content window appear without scroll bars and dropdown menu 
// lists that are longer than the content window appear with scroll bars (as a result of having the style value
// "overflow: auto"

window.onresize =  modifyAttributes;
window.onload   = modifyAttributes;


function modifyAttributes(){
	computeDropdownMenuHeight();
    computeAsideLeftContentHeight()	;
}

//window.onresize =  computeAsideLeftContentHeight;
//window.onload = computeAsideLeftContentHeight;

function computeDropdownMenuHeight(){
  var article = document.getElementById("mainContent");
  // var articleHeight = parseInt(window.getComputedStyle(article,null).getPropertyValue("height"));
  var maxDropdownHeight = article.clientHeight;
//  alert("maxDropdownHeight = " + maxDropdownHeight);
  var dropdownMenu = document.getElementsByClassName("dropdownMenu");
  var i=0;
  while (i< dropdownMenu.length){
	// alert("before " + dropdownMenu[i].clientHeight);  
    dropdownMenu[i].style.height = "";
   // alert("after " + dropdownMenu[i].clientHeight);  
	i = i+1;
  }
  i = 0;
  while (i< dropdownMenu.length){
	// The stylesheet for the dropdown menus has "display = none" so that this attribute can be set to "display = block" when
	// the menu name is hovered over. Since "display = none" both hides the dropdown menu and does not reserve space for the
	// dropdown menu, this Javascript function must set "display = block" so that the height of the dropdown menu can be obtained.
	dropdownMenu[i].style.display="block";
	// While obtaining the height of the dropdown menu, we don't want it to be visible in the browser.
	dropdownMenu[i].style.visibility="hidden";
	// The dropdown menu height can now be compared with the article box height in the browser, which changes automatically
	// when the pages is first accessed and whenever the browser window is resized 
    //alert("dropdown[" + i + "] = " + dropdownMenu[i].clientHeight + " maxDropdownHeight = " + maxDropdownHeight);
    if ((dropdownMenu[i].clientHeight) > maxDropdownHeight) {
	  // if the height of the dropdown menu exceeds the height of the article box, we want to set the height of the
	  // dropdown box to be the same as the	height of the article box; since "overflow: auto" is set for all dropdown menus in
	  // a stylesheet, if a dropdown menu exceeds the height of the article box, a scroll bar will automatically appear.
	  // Also, before we do set the height we need to remove the style attributes set by this function above, as they will
	  // override the external stylesheet styles required for the CSS3 "hover" to work.
      dropdownMenu[i].style.display="";
      dropdownMenu[i].style.visibility="";		  
	  dropdownMenu[i].style.height=maxDropdownHeight + "px";
     //dropdownMenu[i].style.height = (articleHeight + articlePaddingTop + articlePaddingBottom) + "px";
	}
	else {
	  // if the height of the dropdown menu does not exceed the height of the article box, we must remove all inline style
	  // attributes set above by this function so that the mouse hover effect works as specified in an external stylesheet
	 dropdownMenu[i].style.display="";
     dropdownMenu[i].style.visibility="";
	}
    i=i+1;
	}
	
  /*var article = document.getElementById("mainContent");
  var asideLeftTitle = document.getElementById("asideLeftTitle");
  var footer = document.getElementById("footer");
  var articleHeight = article.clientHeight;
  var asideLeftTitleHeight = asideLeftTitle.clientHeight;
  var footerHeight = footer.clientHeight 
  asideLeftTitle.style.bottom = (((articleHeight - asideLeftTitleHeight)/2) + footerHeight) + "px";
  var halfAsideLeftTitleWidth = asideLeftTitle.clientWidth/2;
  var asideLeft = document.getElementById("aside-left");
  var halfAsideLeftWidth = asideLeft.clientWidth/2;
  asideLeftTitle.style.left = (-halfAsideLeftTitleWidth + halfAsideLeftWidth) + "px";*/
}

function computeAsideLeftContentHeight(){
  var article = document.getElementById("mainContent");
  var asideLeftTitle = document.getElementById("asideLeftTitle");
  var footer = document.getElementById("footer");
  var articleHeight = article.clientHeight;
  var asideLeftTitleHeight = asideLeftTitle.clientHeight;
  var footerHeight = footer.clientHeight 
  asideLeftTitle.style.bottom = (((articleHeight - asideLeftTitleHeight)/2) + footerHeight) + "px";
  var halfAsideLeftTitleWidth = asideLeftTitle.clientWidth/2;
  var asideLeft = document.getElementById("aside-left");
  var halfAsideLeftWidth = asideLeft.clientWidth/2;
  asideLeftTitle.style.left = (-halfAsideLeftTitleWidth + halfAsideLeftWidth) + "px";
}
