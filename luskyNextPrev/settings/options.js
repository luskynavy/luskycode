function saveOptions(e) {
  e.preventDefault();
  browser.storage.local.set({
    holeLimit: document.querySelector("#holeLimit").value,
	fastJump: document.querySelector("#fastJump").value
  });
}

function restoreOptions() {

  chrome.storage.local.get(null,
    function(object)
    {
        document.getElementById("holeLimit").value = object["holeLimit"];        
        document.getElementById("fastJump").value = object["fastJump"];
  });
  /*function setCurrentChoice(result) {
    document.querySelector("#holeLimit").value = result.holeLimit || 10;
	document.querySelector("#holeLimit").value = result.holeLimit || 10;
  }

  function onError(error) {
    console.log(`Error: ${error}`);
  }

  var getting = browser.storage.local.get("holeLimit");
  getting.then(setCurrentChoice, onError);*/
}

document.addEventListener("DOMContentLoaded", restoreOptions);
document.querySelector("form").addEventListener("submit", saveOptions);