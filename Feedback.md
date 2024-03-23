### Feedback

*Please add below any feedback you want to send to the team*

### Caching 

From the original requirement:
"We will like to have a cache layer to cache the response from the Provided API because the API is slow and fails a lot. We will like to call the API and in case of failure try to use the cached response. The cache should use the Redis container provided in the docker-compose.yaml"

I did it partially. I'm not fully getting this 'We will like to call the API and in case of failure try to use the cached response'. Using retry policy fully compensate this flaw, so I'm doing basic cache-aside approach. 

### Execution Tracking

From the original requirement:
"We want to track the execution time of each request done to the service and log the time in the Console.
By default, we set the loggers to log in to the Console, so you only need to worry where to put the Logger in the code."

I've also added OpenTelemetry metrics for requests and log them to console every 5min

### Provided API

From the original requirement:
"We know that [**Provided API**](http://localhost:7172/swagger/index.html) may have some configuration issues, and we will like them to be found and fixed, if possible."

During the development I've noticed "File not found movies-db.json inside the container" in container logs. 
Further I found "amovies-db.json" inside the container, however, I found that this file name is hardcoded.