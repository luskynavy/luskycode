# Generate html from postman requests collection

1. Get docgen from here : https://github.com/thedevsaddam/docgen/releases

2. Export collection from Postman (RMB on Devcamper API->Export) to "dc.postman_collection.json".

3. Generate index.html from postman export:
```sh
docgen build -i dc.postman_collection.json -o index.html
```
