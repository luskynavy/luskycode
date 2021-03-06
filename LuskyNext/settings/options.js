function saveOptions(e) {
  e.preventDefault();
  browser.storage.local.set({
    holeLimit: document.querySelector("#holeLimit").value,
  });
}

function restoreOptions() {
  chrome.storage.local.get(null,
    function(object) {
      document.getElementById("holeLimit").value = object["holeLimit"] || 10;        
    }
  );
}

document.addEventListener("DOMContentLoaded", restoreOptions);
document.querySelector("form").addEventListener("submit", saveOptions);