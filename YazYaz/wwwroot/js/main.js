var wordsCount;
var tempWords = genWords(); // ilk yüklenmede kelimelerin oluşturulması
var wordLib = genLib(); // tempWords'un 2D char array hali

// site açıldığında yürütülecek işlemler
$(document).ready(function(){
  $( ".alert" ).hide();

  $(".resetBtn").click(function(){
    // reset
    currWord = 0;
    currIndex = 0;
    numErrors = 0;
    space = 0;
    errorLib = [];
    errorArray = [];
    seconds = 0;
    stopped = 0;

    $('h2').replaceWith("<h2>Click the textbox, and copy the content on the paragraph.</h2>");//reset header
    $( ".alert" ).hide(); // Alerti gizle
    $('#userInput').val(''); // Text area temizle

    tempWords = genWords(); // Yeni kelimeler
    wordLib = []; // empty the 2D char array
    wordLib = genLib();//refill the 2D char array
  });

  //display an alert if the document is clicked
  //while timer is still running
  $(document).click(function(e) {
    if(seconds > 0){
      $( ".alert" ).show("fast");
    }
  });
});

function genWords() {
    var words = paragraph.split(" ");
    wordsCount = words.length
    return words;
}

var timer = 0;
var seconds = 0;
var t;
var stopped = 0;
function Timer(event){
    // Timer temizle
    $(".resetBtn").click(function(){
      clearInterval(t);
      seconds = 0;
      timer = 0;
    });

    // Timer baslat
    if(timer == 0 && event.which!=13 && stopped == 0){
      $('h2').replaceWith("<h2>Time elapsed: " + seconds/10 + " seconds.</h2>");
      timer = 1;
      t = setInterval(function() {startTime()}, 100);
      timer = 1;
    }

}

// timer
function startTime () {
    seconds = seconds + 1;
    $('h2').replaceWith("<h2>Time elapsed: " + seconds/10 + " seconds.</h2>");
}

function genLib () {
    var charArray = [];
    for(var i = 0; i < wordsCount; i++){
        charArray[i] = tempWords[i].split('');
    }
    return charArray;
}

// index.cshtml'den hem timer'i hemde errors'u çağırır
function CallBoth (event){
    Errors(event);
    Timer(event);
}

var currWord = 0; // wordLib'deki ilk index
var currIndex = 0; // bir index daha, konumu belirlemek icin
var numErrors = 0;
var space = 0; // boşluk gelip gelmemesi gerektiğini saptıyor
var errorLib = []; // her kelimedeki hata sayısını saklar
var errorArray = []; // belirli bir indexdeki hatayı saklar
var atEnd = 0; // kelimenin sonuna gelindiğinde artar 
var typedEntries = 0; // yazılan toplam karakter sayısı (WPM hesaplamak için)

// yazarken hataları sayar
function Errors(event){
    if(stopped == 1){
        return;
    }
    //increments atEnd and returns if the user continues to type when they have not
    //finished the last word
    if(atEnd > 0){
        atEnd++;
        return;
    }

    if(space == 1){
        if(event.which == 32){
            space = 0;
            return;
        }
    }

    //if the user types the correct letter
    //the index of the char will be marked as correct
    //otherwise marked incorrect
    if(String.fromCharCode(event.which) == wordLib[currWord][currIndex]){
      errorArray[currIndex] = 0;
      errorLib[currWord] = errorArray;
      currIndex++;
      typedEntries++;

      //once the user has correctly typed the last character
      //the program is stopped with StopTime
      if(currWord == wordsCount - 1 && currIndex == wordLib[currWord].length){ //@change 4 to #words - 1
          StopTime();
          return;
      }
    }
    else if (String.fromCharCode(event.which) != wordLib[currWord][currIndex]) {
        errorArray[currIndex] = 1;
        errorLib[currWord] = errorArray;
        if(space == 0){
            currIndex++;
        }
        numErrors++;
        typedEntries++;
    }

    //increases currWord and resets currIndex
    //when the end of a word is reached
    if(currIndex == wordLib[currWord].length){
            currWord++;
            currIndex = 0;
            space = 1;
    }
}

//separate function to handle backspace events
function BackSpace(event){
    if(event.which == 8){
        //returns if a backspace is entered at the start of text
        if(currIndex == 0 && currWord == 0){
            return;
        }
        //decreases atEnd and returns when the end has been reached
        if(atEnd > 0){
            atEnd--;
            return;
        }

        //if a space is supposed to be pressed
        //move back a character and set the index to the length of the previous word
        if(space == 1 && currIndex == 0){
            currWord--;
            currIndex = wordLib[currWord].length;
            space = 0;

        //if a space is not to be pressed
        //but the current character is the start of a word
        //make the next character to be pressed to be the spacebar
        }else if(space == 0 && currIndex == 0){
            space = 1;
            return;
        }

        currIndex--;

        //reduces the number of errors if the character that was backspaced was wrong
        if(errorLib[currWord][currIndex] == 1){
            numErrors--;
        }
    }
}

//'stops' the program
function StopTime(){
    stopped = 1;
    clearInterval(t);
    var netWPM = calcNetWPM();
    netWPM = Math.round(netWPM).toString();
    $('h2').replaceWith("<h2>Your typing speed in net WPM: " + netWPM + " words per minute." + "Errors: " + numErrors + " Time:" + seconds/10);
    $('h2').css('color', 'blue');

    customAlert("Saving...", "info");

    var settings = {
        "url": uri,
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json",
        },
        "data": JSON.stringify({ "time": (seconds / 10).toString(), "speed": netWPM }),
        "success": function () {
            customAlert("Saved!", "success");
        },
        "error": function () {
            customAlert("Error!", "danger");
        }
    };

    $.ajax(settings).done(function (response) {
        console.log(response);
    });

    seconds = 0;
    timer = 0;
}

function customAlert(text, type) {
    $('#result').html(text)
    if ($('#result').hasClass("alert")) {
        $('#result').removeClass(["alert", "alert-success", "alert-danger", "alert-info"])
    }
    $('#result').addClass("alert alert-" + type)
    $('#result').show()
}

//calculate the net WPM using the formula provided
//in http://www.speedtypingonline.com/typing-equations
function calcNetWPM(){
  var grossWPM = (typedEntries / 5) / (seconds / 10 / 60);
  var netWPM = grossWPM - (numErrors / seconds / 10 / 60);
  return Math.round(netWPM * 100) / 100
}
