// JavaScript Document

function colorFadeInOut(id, colorStart, colorEnd, milliseconds) {
    //speed for each frame
    var speed = Math.round(milliseconds / 400);
    var timer = 0;
	var colorRange;
	document.getElementById(id).style.visibility = "visible";
	//var color= "#000000";
	
    //determine the direction for the blending, if start and end are the same nothing happens
    if(colorEnd > colorStart) {
		colorRange = milliseconds/(colorEnd - colorStart);
        for(i = 50; i <= 256; i++) {
	        color = i + 256*i + 65536*i;
            setTimeout("changeColor(" + color + ",'" + id + "')",(timer * speed));
            timer++;
        }
	    for(i = 0; i <= 100; i++) {
		    color = 16777215 - (i + 256*i + 65536*i);
            setTimeout("changeColor(" + color + ",'" + id + "')",(timer * speed));
            timer++;
        }			
    } else if(colorStart > colorEnd) {
		colorRange = (colorStart - colorEnd)/millisecnods;
        for(i = 256; i >= 50; i--){
		    color = i + 256*i + 65536*i;
            setTimeout("changeColor(" + color + ",'" + id + "')",(timer * speed));
            timer++;
        }
    }
}

//change the color for different browsers
function changeColor(color, id) {
    var object = document.getElementById(id).style;
    object.color = "#" + color.toString(16);
	//alert(object.height);
} 