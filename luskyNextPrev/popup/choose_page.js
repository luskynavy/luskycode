var testUrlReturn;

function onclick(e) {
  var holeLimit, fastJump, dataStep;
  
  dataStep = +e.currentTarget.getAttribute('data-step');
  console.log("onclick dataStep " + dataStep);
  
  chrome.storage.local.get(null,
    function(object)
    {
      holeLimit = object["holeLimit"] || 10;      
      fastJump = object["fastJump"] || 10;
	  //console.log("onclick hole limit " + holeLimit);
      //console.log("onclick fast jump " + +fastJump);	  
	  onStorageLoaded(holeLimit, fastJump, dataStep);
  });
}

function onStorageLoaded(holeLimit, fastJump, dataStep) {
  console.log("onStorageLoaded hole limit " + holeLimit);
  console.log("onStorageLoaded fast jump " + fastJump);
  console.log("onStorageLoaded dataStep " + dataStep);
  
  var dataStepSign;
  if (dataStep >= 0) {
	if (dataStep > 1)
		dataStep = +fastJump;
	dataStepSign = 1;
  } else {
	if (dataStep < -1)
	  dataStep = -fastJump;	
	dataStepSign = -1;
  }  

  //convert the increment to integer and use it to increment the last number in url of current tab
  chrome.tabs.query({active: true, currentWindow: true}, function(tabs) {
	//console.log("hole limit " + holeLimit);
	for (i = 0; i < holeLimit; i++) {	  
	  var nextUrl = getAndIncrementLastNumber(tabs[0].url, dataStep + dataStepSign * i);
	  TestUrl(nextUrl);
	  console.log(nextUrl + ' ' + testUrlReturn);
	  if (testUrlReturn == 1)
		break;
	}
    chrome.tabs.update(tabs[0].id, {url: nextUrl});	
  });
}

function TestUrl(strURL) {

    var req = new XMLHttpRequest();
    req.open("GET", strURL, false); //false for synchronous, deprecated, should be synchronous
    req.onreadystatechange=function() {
        if (req.readyState == 4) {
            if (req.status == 200)
            {
                //console.log("Url found!");
				testUrlReturn = 1;
            } else if (req.status == 404) {
				//console.log("URL doesn't exist!")				
				testUrlReturn = 0;
			} else {
				//console.log("Error: Status is " + req.status)				
				testUrlReturn = 0;
			}
        }
    }
    req.send();
}

var elts = document.getElementsByClassName('menu-item');

for(var i=0; i<elts.length; i++) {
  elts[i].addEventListener('click', onclick);
}

/**
* Get and increment by increment the last number of the url string
* @param str URL String encoded
* @param increment integer increment to be added
*/
function getAndIncrementLastNumber(str, increment) {
  return str.replace(/(\D*)(\d+)(\D*)$/, function(s, p1, p2, p3) {
    var l = p2.length, s = '' + (+p2+increment), n = l - s.length;
	if (n < 0)
	{
		n = 0;
	}
    return p1 + '0'.repeat(n) + s + p3;
  });
}
