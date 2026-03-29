# HackerNewsBestStories

This project implements a RESTFul API using ASP.NET Core to retrieve the best stories from the Hacker News API and then sort them by score in descending order.

The API is designed with efficiency and scalability in mind.

## Features

- Retrieves the best stories from Hacker News API.
- Fetches individual stories in parallel.
- Exposes a single endpoint to display the details of the best N stories.
- Includes basic input validations with unit testing.
- Caches Hacker News API responses to reduce external API calls and avoid risking overloading of the Hacker News API.
- HTTP client separation: All HTTP calls to Hacker News are handled by a dedicated HackerNewsHttpClient service, making the code more modular and testable.
- Request validation using Data Annotations and custom validation logic.

## API Endpoints

`GET /api/stories/beststories?n={numberOfStories}`

- `numberOfStories`: An integer representing the number of top stories to retrieve. Defaults to 10. Maximum allowed is 200.

## Example Response

```json
[
  {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "postedBy": "ismaildonmez",
    "time": "2019-10-12T13:43:01+00:00",
    "score": 1716,
    "commentCount": 572
  },
  { ... },
  { ... },
  { ... },
]
```

### Assumptions Made

- The Hacker News API endpoints (`beststories.json` and `item/{id}.json`) return data in the expected JSON format.
- The Hacker News API is always available.

### Improvements and Changes considered is given more time

- **Caching**: Replace in memory cache (`IMemoryCache`) with a better caching solution like Redis to improve performance and scalability.
- **Rate Limiting**: implement a robust rate limiting policy to protect our endpoint (`/api/stories/getbeststories`) from abuse and to ensure a fair usage.
- **Asynchronous Story Fetching**: while stories are currently fetched in parallel, if the list of IDs is very large, it would be better to fetch all of the best stories ID in smaller batches asynchronously.
- **Error handling**: a more granular error logging when a Story Details fetch failed.
- **Performance Monitoring**: implement monitoring logs and integrate it with  Application Insights or any other monitoring tool to gather metrics on API responses.
- **More Unit Tests**: add more testing scenarios and better structure of the test cases.

## How to Run the Application

### .NET Version

- This project was built with **.NET 10 (LTS)**

### Steps to run the application

1.  **Clone the repository:**

    ```bash
    git clone https://github.com/moisesmartinez/HackerNewsBestStories.git
    cd HackerNewsApi
    ```

2.  **Navigate to the project directory:**

    ```bash
    cd HackerNewsBestStories\\HackerNewsBestStories.API
    ```

3.  **Run the application:**

    ```bash
    dotnet run
    ```

    The API will typically run on `http://localhost:5031`. You can use Postman to test the API.


