// JavaScript Document

//  Note that parameter id is a string that is the name of some id in the window object, such as id = "fadeInDiv"
function opacityFadeInOut(id, opacityStart, opacityEnd, milliseconds) {
    //speed for each frame
    var speed = Math.round(milliseconds / 100);
    var timer = 0;

    //determine the direction for the blending, if start and end are the same nothing happens
    if(opacityStart > opacityEnd) {
	        for(i = opacityStart; i >= opacityEnd; i--) {
			// The concatenated id below is a string value, such as fadeInDiv. It must be encolosed in quotes
			// to make it a string parameter for the call to changeOpacity
            setTimeout("changeOpacity(" + i + ",'" + id + "')",(timer * speed));
            timer++;
        }
    } else if(opacityStart < opacityEnd) {
        for(i = opacityStart; i <= opacityEnd; i++)
            {
            setTimeout("changeOpacity(" + i + ",'" + id + "')",(timer * speed));
            timer++;
        }
    }
}

// change the opacity for different browsers
// parameter id contains the string passed to it, for example fadeInDiv
function changeOpacity(opacity, id) {
    var object = document.getElementById(id).style;
    object.opacity = (opacity / 100);
    /*object.MozOpacity = (opacity / 100);
    object.KhtmlOpacity = (opacity / 100);
    object.filter = "alpha(opacity=" + opacity + ")";*/
} 