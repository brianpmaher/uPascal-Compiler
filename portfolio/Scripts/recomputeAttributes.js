// JavaScript Document

// The set of functions in this JavaScript document change certain element attributes of the active page
// each time a window is loaded or resized. Since apparently only one function can be associated with the
// window.onresize and window.onload events, a single function, modifyAttributes, is invoked on the
// occurrence of either of these events. That function calls the other functions that modify the
// desired attributes.

window.onresize = modifyAttributes;
window.onload   = modifyAttributes;

function modifyAttributes(){
  // Function modifyAttributes calls other functions that modify element attributes.
  computeDropdownMenuHeight();
  computeAsideLeftContentHeight();
  prettyPrint();
}

function computeDropdownMenuHeight(){  
  // This function modifies the height attribute of	the ul containing the dropdwon menus to match the current 
  // main content pane of the page, which changes in height upon loading and resizing page events. The ul height
  // of each dropdown menu is examined. If it is less than the height of the main content pane, it is left as is.
  // If it is larger than the height of the main content pane, its height is set equal to the height of the main
  // content pane. Since a stylesheet sets the overflow attribute to "auto" for all ul's, the effect is that a
  // scrollbar becomes visible for ul's with intrinsic height larger than the main content pane so that users can
  // scroll to see the entire associated dropdown menu.
  var article = document.getElementById("mainContent");
  var maxDropdownHeight = article.clientHeight;
  var dropdownMenu = document.getElementsByClassName("dropdownMenu");
  
  // This first loop is intended to remove the style.height attribute from the dropdown menus embedded directly in the
  // HTML ul dropdown tags as a result of the previous call to this function, so that their heights can be recomputed from the
  // new size of the main content pane (as a result of an onload or resize event).
  var i=0;
  while (i< dropdownMenu.length){
    dropdownMenu[i].style.height = "";
	i = i+1;
  }

  // This second loop determines the height of each dropdown menu and compares that height to the height of the main content
  // pane.  If the dropdown menu height is less than or equal to the main content pane height, the dropdown menu height remains unchanged.
  // If the dropdown menu height is greater than the main content pane height, the height of the dropdown menu is set equal to the
  // main content pane height. Since each dropdown menu has the "overflow" attribute set to "auto" by a stylesheet, such a menu 
  // item will show up with a vertical scroll bar to allow the user to see all menu items.
  i = 0;
  while (i< dropdownMenu.length){
	// The stylesheet for the dropdown menus has "display = none" so that this attribute can be set to "display = block" when
	// the menu name is hovered over. Since "display = none" both hides the dropdown menu and does not reserve space in the HTML document
	//  for the dropdown menu, this Javascript function must set "display = block" so that the height of the dropdown menu can be obtained.
	dropdownMenu[i].style.display="block";
	// As the height of the current dropdown menu is being obtained, the drodoon menu itself should not be visible in the page.
	dropdownMenu[i].style.visibility="hidden";
	// The dropdown menu height can now be compared with the main content pane height in the page.
    if ((dropdownMenu[i].clientHeight) > maxDropdownHeight) {
	  // if the height of the dropdown menu exceeds the height of the main content pane, its height is set equal to the height of 
	  // the main content pane. Before this is done, however, the display and visibility attributes, set to "block" and "hidden"
	  // above, must be removed (when these attributes are set by JavaScript, they appear as style attributes directly in the associated
	  // element tag in the HTML document, and thus override any external stylesheet settings for these attributes; at this point, the
	  // external stylesheet attributes should be allowed to be in force, so the Javascript set attributes are removed). This will allow
	  // the external stylesheet "hover" event to cause the dropdown menus to appear as designed.
	  dropdownMenu[i].style.display="";
      dropdownMenu[i].style.visibility="";		  
	  dropdownMenu[i].style.height=maxDropdownHeight + "px";
	}
	else {
	  // if the height of the dropdown menu does not exceed the height of the article box, it remains unchanged.
	  // However, the display and visibility attributes, set to "block" and "hidden"
	  // above, must be removed (when these attributes are set by JavaScript, they appear as style attributes directly in the associated
	  // element tag in the HTML document, and thus override any external stylesheet settings for these attributes; at this point, the
	  // external stylesheet attributes should be allowed to be in force, so the Javascript set attributes are removed). This will allow
	  // the external stylesheet "hover" event to cause the dropdown menus to appear as designed.
	 dropdownMenu[i].style.display="";
     dropdownMenu[i].style.visibility="";
	}
    i=i+1;
  }
}

function computeAsideLeftContentHeight(){
  // This function positions the rotated (vertical) text in the left-aside pane to be centered in that pane both horizontally and vertically.
  
  // The next few lines center the text bar (that is rotated -90 degrees by the CSS "rotate" function in an external style sheet) vertically 
  // in relation to the main content pane (demarked by the <article> page element) so that the rotated text will also appear centered in relation
  // to the main content pane.
  var article = document.getElementById("mainContent");
  var asideLeftTitle = document.getElementById("asideLeftTitle");
  var footer = document.getElementById("footer");
  var articleHeight = article.clientHeight;
  var asideLeftTitleHeight = asideLeftTitle.clientHeight;
  var footerHeight = footer.clientHeight 
  asideLeftTitle.style.bottom = (((articleHeight - asideLeftTitleHeight)/2) + footerHeight) + "px";
  
  // The remaining lines set the width of the pane that holds the text to be rotated to the height of the main content bar, so that when the
  // pane that holds the text is rotated, its rotated height will (i.e., its original width) will match the height of the main content bar.
  // The text is centered in this pane so that it appears to be centered in relation to the main content pane.
  var asideLeft = document.getElementById("aside-left");
  asideLeftTitle.style.width = asideLeft.clientHeight + "px";
  var halfAsideLeftTitleWidth = asideLeftTitle.clientWidth/2;
  var halfAsideLeftWidth = asideLeft.clientWidth/2;
  asideLeftTitle.style.textAlign = "center";
  asideLeftTitle.style.left = (-halfAsideLeftTitleWidth + halfAsideLeftWidth) + "px";
}