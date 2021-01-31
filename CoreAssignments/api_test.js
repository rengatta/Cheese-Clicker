const http = require('http');
const hostname = '127.0.0.1';
const port = 3000;

const server = http.createServer((req, res) => {
  res.statusCode = 200;
  res.setHeader('Content-Type', 'text/plain');
  res.end('CSE 4500-01 API Test Program');
});

server.listen(port, hostname, () => {
    console.log(`Server running at http://${hostname}:${port}/`);
    
    console.log("Start of the API Test program.");

    const fetch = require('node-fetch');

    //san bernardino coords - Latitude: 34.115784, Longitude: 	-117.302399
    fetch('https://api.sunrise-sunset.org/json?lat=34.115784&lng=-117.302399')
    .then(res => res.text())
    .then(text => console.log(text))


});