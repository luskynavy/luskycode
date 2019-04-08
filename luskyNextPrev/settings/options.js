function saveOptions(e) {
  e.preventDefault();  
  browser.storage.local.set({
    holeLimit: document.querySelector("#holeLimit").value,
    fastJump: document.querySelector("#fastJump").value,
    darkMode: document.querySelector("#darkMode").checked	
  });
}

function restoreOptions() {
  chrome.storage.local.get(null,
    function(object) {
      document.getElementById("holeLimit").value = object["holeLimit"] || 10;
      document.getElementById("fastJump").value = object["fastJump"] || 10;
      document.getElementById("darkMode").checked = object["darkMode"]=="true" ? true : false || false;
    }
  );
}

document.addEventListener("DOMContentLoaded", restoreOptions);
document.querySelector("form").addEventListener("submit", saveOptions);