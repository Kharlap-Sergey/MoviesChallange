### Feedback

*Please add below any feedback you want to send to the team*

### Caching 

From the original requirement 
"We will like to have a cache layer to cache the response from the Provided API because the API is slow and fails a lot. We will like to call the API and in case of failure try to use the cached response. The cache should use the Redis container provided in the docker-compose.yaml"

I did it partially. I'm not fully getting this 'We will like to call the API and in case of failure try to use the cached response'. Using retry policy fully compensate this flaw, so I'm doing basic cache-aside approach. 

### Execution Tracking

I've also added OpenTelemetry metrics for requests and log them to console every 5min