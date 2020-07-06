# DetectorAPI

Service detection api build with .net core 3.1 with swagger and unit tests. Api documentation can be found under swagger.

# Deployment with Docker
  Start Detector API containers
    > docker-compose up
    
   It’s accessible on port 2288 of the detectorapi container.
   When using detectorapi, visit http://localhost:2288/swagger in your browser.
   
  > Please see the docker.png screenshot at the root level under /Screenhots folder. 

# Swagger
When the project boots up, swagger shows up immediately with all API information. Swagger is also configured for versioning.
> Please see the swagger.png screenshot at the root level under /Screenhots folder.   

# Sample input 
> Please see the input.png screenshot at the root level under /Screenhots folder.  

# Sample output
> Please see the output.png screenshot at the root level under /Screenhots folder.

# Unit tests
Unit tests has no external dependency and can be run independently.

# Logging
Events are logged to a file using serilog. Log's are generated at the root level under /Log folder for each day seperately.
> Please see the log.png screenshot at the root level under /Screenhots folder. 
