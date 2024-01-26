/******************************************************************
                          Utility functions
 ******************************************************************/
/*
	getCharCode: returns the Unicode key code of the pressed event.
	Handles differences between IE and NS/Mozilla.
*/
function getCharCode(e) { // e is a KEYPRESS event
	var k = -1;

	/* Quoted from www.dardenhome.com/js/x247537.htm
	"In Netscape, the key code of the key that triggered the event
	is available in the which property of the Event object that is
	passed to the event handler as an argument. The equivalent
	value in MSIE is available in the keyCode property of the
	event object referenced by the window.event property." */

	if (e && e.which){
		k = e.charCode; // NS
		if (k == 0) // equalize differences in BS
			k = 8;
	}

	else if (window.event && window.event.keyCode){
		k = window.event.keyCode; // IE
	}
	return k;
}


/******************************************************************
                        Disable Validation
******************************************************************/
function CheckForDisableValidation(event) {
    var myKeyCode;
    myKeyCode = event.charCode || event.keyCode;

    switch (myKeyCode) {
        // Space
        case 0:
            return true;
            break;
        // Backspace 	
        case 8:
            return true;
            break;
        // Tab 	
        case 9:
            return true;
            break;
        // Enter 	
        case 13:
            return true;
            break;
        // Space
        case 32:
            return true;
            break;
        // Arrow Left
        case 37:
            return true;
            break;
        // Arrow Up
        case 38:
            return true;
            break;
       // Arrow R
        case 39:
            return true;
            break;
        // Arrow Down
        case 40:
            return true;
            break;
        // Delete
        case 46:
            return true;
            break;
        default: return false;
    }

    return false;
}

/******************************************************************
 A set of small isXXXX functions that perform range checking on
 Unicode codes
******************************************************************/
function isNumericCode(code) {
	return code > 47 && code < 58;
}

function isGreekCapitalCode(code) {
	return code > 912 && code < 940;
}

function isLatinCapitalCode(code) {
	return code > 64 && code < 91;
}

function isCapitalCode(code) {
	return isGreekCapitalCode(code) || isLatinCapitalCode(code);
}

function isLatinLowerCode(code) {
	return code > 96 && code < 123;
}

function isPunctCode(code) {
    return code == 32 || code == 47 || code == 40 || code == 41 || code == 45;
}

/******************************************************************
                      Main part. Event Handlers.
 ******************************************************************/
function PositiveIntInputOnly(e) { // KEYPRESS event
// returns true if 0-9 or BS hit, or can't get key value; otherwise false

	var k = getCharCode(e);

	return k <= -1 || isNumericCode(k) || k == 8;			
} // positiveIntInputOnly()

function AnyIntInputOnly(e) { // KEYPRESS event
// returns true if 0-9 or BS hit, or can't get key value; otherwise false

    var k = getCharCode(e);

    return k <= -1 || isNumericCode(k) || k == 8 || k == 45;
} // anyIntInputOnly()


function FloatInputOnly(e) { // KEYPRESS event
// returns true if 0-9 or BS hit, or can't get key value; otherwise false

	var k = getCharCode(e);
  
	return k <= -1 || isNumericCode(k) || k == 8 || k == 46 || k == 44;			
} // FloatInputOnly()


function StringLoginInputOnly(e) { // KEYPRESS event
// returns true if 0-9 or BS or a-z hit, or can't get key value; otherwise false

	var k = getCharCode(e);

	return k <= -1 || isNumericCode(k) || k == 8 || isLatinLowerCode(k);
} // StringLoginInputOnly()


function NameInputOnly(e) { // KEYPRESS event
// returns true if Á-Ù or A-Z or "Ú" or "Û" or spacebar or "-" or "/" or BS hit, or can't get key value; otherwise false

	var k = getCharCode(e);

	return k <= -1 || isCapitalCode(k) || k == 8 || k == 45 || k == 47 || k==32;
} // NameInputOnly()


function StringInputOnly(e) { // KEYPRESS event
// returns true if 0-9 or Á-Ù or A-Z or BS or spacebar or "." or "(" or ")" or "-" or "/" hit, or can't get key value; otherwise false

	var k = getCharCode(e);
	return k <= -1 || k==46 || k==44 || isCapitalCode(k) || k == 8 || isNumericCode(k) || isPunctCode(k);
} // NameInputOnly()

function StringLowerInput(e) { // KEYPRESS event
// returns true if 0-9 or Á-Ù or Ü-þ or A-Z or BS or spacebar or ";" or "." or "(" or ")" or "-" or "/" hit, or can't get key value; otherwise false

	var k = getCharCode(e);
	return k <= -1 || k==46 || isCapitalCode(k) || k == 8 || isNumericCode(k) || isPunctCode(k) || (k >= 902 && k <=974) ||
isLatinLowerCode(k) || k==59;
}
