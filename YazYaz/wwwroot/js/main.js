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
      wordsCount = 0;
      typedEntries = 0;

      $('h2').replaceWith("<h2>" + localizerClickBox + "</h2>");//reset header
      $( ".alert" ).hide(); // Alerti gizle
      $('#userInput').val(''); // Text area temizle
      $('#resultRank').html("");
      $('#resultImage').html("");
    tempWords = genWords(); // Yeni kelimeler
    wordLib = genLib();
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
    $(".resetBtn").click(function() {
      clearInterval(t);
      seconds = 0;
      timer = 0;
    });

    // Timer baslat
    if(timer == 0 && event.which!=13 && stopped == 0) {
        $('h2').replaceWith("<h2>" + localizerElapsed + " " + seconds/10 + " " + localizerSeconds + "</h2>");
      timer = 1;
      t = setInterval(function() {startTime()}, 100);
      timer = 1;
    }

}

// timer
function startTime () {
    seconds = seconds + 1;
    $('h2').replaceWith("<h2>" + localizerElapsed + " " + seconds/10 + " " + localizerSeconds + "</h2>");
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
    // oyuncu son kelimeyi yazmadıysa atEnd arttırır
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

    // eğer kullanıcı doğru harfe basarsa ilgili char doğru olarak işaretlenir
    // diğer halde yanlış olarak işaretlenir
    if (String.fromCharCode(event.which) == wordLib[currWord][currIndex]) {
        errorArray[currIndex] = 0;
        errorLib[currWord] = errorArray;
        currIndex++;
        typedEntries++;

        // kullanıcı son doğru harfi girdiğinde StopTime çağrılır
        if(currWord == wordsCount - 1 && currIndex == wordLib[currWord].length) {
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

    if(currIndex == wordLib[currWord].length){
            currWord++;
            currIndex = 0;
            space = 1;
    }
}

// silme işlevini kotarmak için ayrı bir fonksiyon
function BackSpace(event) {
    if(event.which == 8) {
        if(currIndex == 0 && currWord == 0){
            return;
        }
        if(atEnd > 0){
            atEnd--;
            return;
        }

        if(space == 1 && currIndex == 0){
            currWord--;
            currIndex = wordLib[currWord].length;
            space = 0;

        }
        else if (space == 0 && currIndex == 0) {
            space = 1;
            return;
        }

        currIndex--;

        if(errorLib[currWord][currIndex] == 1){
            numErrors--;
        }
    }
}

// programı durdurur
function StopTime(){
    stopped = 1;
    clearInterval(t);
    var netWPM = calcNetWPM();
    netWPM = Math.round(netWPM).toString();
    $('h2').replaceWith("<h2>" + localizerTypingSpeedIs + " " + netWPM + " " + localizerWordsPerMinute + " " + localizerErrors + " " + numErrors + " " + localizerTime + " " + seconds/10);
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
            customAlert(localizerSavedDatabase, "success");
        },
        "error": function () {
            customAlert(localizerErrorDatabase, "danger");
        }
    };

    $.ajax(settings).done(function (response) {
        console.log(response);
    });

    showImage(netWPM);

    seconds = 0;
    timer = 0;
}

function showImage(wpm) {
    var rank;
    if (wpm < 24) {
        rank = 'F';
        $('#resultImage').append('<img src="../img/rank_f.png" width="450" height="250"/>');
    }
    else if (wpm > 25 && wpm < 30) {
        rank = 'D';
        $('#resultImage').append('<img src="../img/rank_d.png" width="450" height="250"/>');
    }
    else if (wpm >= 30 && wpm < 40) {
        rank = 'C';
        $('#resultImage').append('<img src="../img/rank_c.gif" width="450" height="250"/>');
    }
    else if (wpm >= 40 && wpm < 54) {
        rank = 'B';
        $('#resultImage').append('<img src="../img/rank_b.gif" width="450" height="250"/>');
    }
    else if (wpm >= 55) {
        rank = 'A';
        $('#resultImage').append('<img src="../img/rank_a.jpg" width="450" height="250"/>');
    }

    $('#resultRank').append('<h3>Rank: <span class="text-info">' + rank + '</span></h3>')
}

function customAlert(text, type) {
    $('#result').html(text)
    if ($('#result').hasClass("alert")) {
        $('#result').removeClass(["alert", "alert-success", "alert-danger", "alert-info"])
    }
    $('#result').addClass("alert alert-" + type)
    $('#result').show()
}

// dakika başına kelimesini hesaplamak için gerekli algoritma
// http://www.speedtypingonline.com/typing-equations
function calcNetWPM() {
  var grossWPM = (typedEntries / 5) / (seconds / 10 / 60);
  var netWPM = grossWPM - (numErrors / seconds / 10 / 60);
  return Math.round(netWPM * 100) / 100
}
