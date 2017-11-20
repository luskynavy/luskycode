function onclick(e) {    
  //convert the increment to integer and use it to increment the last number in url of current tab
  chrome.tabs.query({active: true, currentWindow: true}, function(tabs) {
    chrome.tabs.update(tabs[0].id, {url: getAndIncrementLastNumber(tabs[0].url, +e.target.getAttribute('data-step'))});	
  });
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
