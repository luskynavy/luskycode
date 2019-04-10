var elts = document.getElementsByClassName('menu-item');

for(var i = 0; i < elts.length; i++) {
  elts[i].addEventListener('click', onclick);
}

chrome.storage.local.get(null,
  function(object)
  {
    holeLimit = object["holeLimit"] || 10;
    fastJump = object["fastJump"] || 10;
    darkMode = object["darkMode"] || 0;
    onStorageLoaded(holeLimit, fastJump, darkMode);
});

function onclick(e) {
  var dataStep;
  
  dataStep = +e.currentTarget.getAttribute('data-step');
  //console.log("onclick dataStep " + dataStep);
  
  var dataStepSign;
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
  
  updateTab(dataStep, dataStepSign);  
}



function onStorageLoaded(holeLimit, fastJump, darkMode) {
  //console.log("onStorageLoaded hole limit " + holeLimit);
  //console.log("onStorageLoaded fast jump " + fastJump);
  //console.log("onStorageLoaded darkMode " + darkMode);
  
  if (darkMode == true)
  {
    document.getElementById('pagestyle').setAttribute('href', 'choose_page_dark.css');
  }
  document.getElementById('fastforward').setAttribute('title', '+' + fastJump);
  document.getElementById('fastbackward').setAttribute('title', '-' + fastJump);
}
