document.addEventListener("click", function(e) {
  if (!e.target.classList.contains("page-choice")) {
    return;
  }
  
  if (e.target.textContent == "Next")
  {
    chrome.tabs.query({active: true, currentWindow: true}, function(tabs) {
      chrome.tabs.update(tabs[0].id, {url: getAndIncrementLastNumber(tabs[0].url)});
    });
  }
  else
  {
    chrome.tabs.query({active: true, currentWindow: true}, function(tabs) {
      chrome.tabs.update(tabs[0].id, {url: getAndDecrementLastNumber(tabs[0].url)});
    });
  }
});

/**
* Get and increment the last number of the url string
* @param {function(string)} str - URL String encoded
*/
function getAndIncrementLastNumber(str) {
    return str.replace(/(\D*)(\d+)(\D*)$/, function(s, p1, p2, p3) {
        var l = p2.length, s = '' + (+p2+1), n = l - s.length;
		return p1 + '0'.repeat(n) + s + p3;
    });
}

/**
* Get and decrement the last number of the url string
* @param {function(string)} str - URL String encoded
*/
function getAndDecrementLastNumber(str) {
    return str.replace(/(\D*)(\d+)(\D*)$/, function(s, p1, p2, p3) {
        var l = p2.length, s = '' + (+p2 - 1), n = l - s.length;
		return p1 + '0'.repeat(n) + s + p3;
    });
}
