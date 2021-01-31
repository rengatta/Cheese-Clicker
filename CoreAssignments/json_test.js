const http = require('http');
const hostname = '127.0.0.1';
const port = 3000;

const server = http.createServer((req, res) => {
  res.statusCode = 200;
  res.setHeader('Content-Type', 'text/plain');
  res.end('CSE 4500-01 JSON Test Program');
});

server.listen(port, hostname, () => {
    console.log(`Server running at http://${hostname}:${port}/`);
    
    console.log("Start of the JSON Test program.");
    
    const json_test = {
      class_name: 'CSE 4500',
      section: 2,
      professor: 'David Turner'
    };

    const json_string = JSON.stringify(json_test);
    console.log("JSON.stringify: " + json_string);
    const json_parse = JSON.parse(json_string);
    
    
    process.stdout.write("JSON.parse: ");
    console.log(json_parse);
    console.log("typeof json_parse: " + typeof(json_parse));

});