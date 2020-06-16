var wrapper = document.getElementById("signature-pad");

var clearButton = wrapper.querySelector("[data-action=clear]");
var changeColorButton = wrapper.querySelector("[data-action=change-color]");
var undoButton = wrapper.querySelector("[data-action=undo]");
var savePNGButton = wrapper.querySelector("[data-action=save-png]");

var canvas = wrapper.querySelector("canvas");



function getMouseOffset(_e, _el) {
    let xpos, ypos;
    if (typeof _e.offsetX === 'undefined') { // ff hack
        // dans ce cas, jQuery facilite l'appel d'offset
        xpos = _e.pageX - $(_el).offset().left;
        ypos = _e.pageY - $(_el).offset().top;
    } else {
        xpos = _e.offsetX;
        ypos = _e.offsetY;
    }
    return { x: xpos, y: ypos };
}



var signaturePad = new SignaturePad(canvas, {
    // It's Necessary to use an opaque color when saving image as JPEG;
    // this option can be omitted if only saving as PNG or SVG
    backgroundColor: 'rgb(255, 255, 255)'
  

});

// Adjust canvas coordinate space taking into account pixel ratio,
// to make it look crisp on mobile devices.
// This also causes canvas to be cleared.
function resizeCanvas() {
  
  
    wrapper = document.getElementById("signature-pad");
    canvas = wrapper.querySelector("canvas");

    // When zoomed out to less than 100%, for some very strange reason,
    // some browsers report devicePixelRatio as less than 1
    // and only part of the canvas is cleared then.
    var ratio = Math.max(window.devicePixelRatio || 1, 1);

    // This part causes the canvas to be cleared
    canvas.width = canvas.offsetWidth * ratio;

    canvas.height = canvas.offsetHeight * ratio;


    canvas.getContext("2d").scale(ratio, ratio);

    // This library does not listen for canvas changes, so after the canvas is automatically
    // cleared by the browser, SignaturePad#isEmpty might still return false, even though the
    // canvas looks empty, because the internal data of this library wasn't cleared. To make sure
    // that the state of this library is consistent with visual state of the canvas, you
    // have to clear it manually.
    signaturePad.clear();
}

// On mobile devices it might make more sense to listen to orientation change,
// rather than window resize events.
window.onresize = function () {
    resizeCanvas();
};

function download() {


    if (signaturePad.isEmpty()) {
        //alert("Please provide a signature first.");
    } else {
        var dataURL = signaturePad.toDataURL();

        ///   download(dataURL, "signature.png");

        var blob = dataURLToBlob(dataURL);

        var url = window.URL.createObjectURL(blob);
        var hidden = document.getElementById("hidBlob");

        //imgDemo.src = dataURL;
        hidden.value = dataURL;

    }


}

// One could simply use Canvas#toBlob method instead, but it's just to show
// that it can be done using result of SignaturePad#toDataURL.
function dataURLToBlob(dataURL) {
    // Code taken from https://github.com/ebidel/filer.js
    var parts = dataURL.split(';base64,');
    var contentType = parts[0].split(":")[1];
    var raw = window.atob(parts[1]);
    var rawLength = raw.length;
    var uInt8Array = new Uint8Array(rawLength);

    for (var i = 0; i < rawLength; ++i) {
        uInt8Array[i] = raw.charCodeAt(i);
    }

    return new Blob([uInt8Array], { type: contentType });
}

clearButton.addEventListener("click", function (event) {
    signaturePad.clear();
});




