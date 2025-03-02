//Launch with : node filesMain.js

const fs = require('fs');

const directoryPath = '.';

// Use fs.readdirSync to read the contents of the directory synchronously
const dirEnt = fs.readdirSync(directoryPath, { withFileTypes: true } );

const filesNames = dirEnt
    .filter(dirent => dirent.isFile())
    .map(dirent => dirent.name);

const folderNames = dirEnt
    .filter(dirent => !dirent.isFile())
    .map(dirent => dirent.name);

console.log('Files and folders in the directory:', dirEnt.map(dirent => dirent.name));

console.log('Folders in the directory:', folderNames);

console.log('Files  in the directory:', filesNames);