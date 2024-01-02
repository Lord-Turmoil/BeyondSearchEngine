# Beyond Search Engine

> Copyright (C) Beyond 2023, Tony's Studio 2023
>
> Relics of BUAA 2023 Autumn *System Analysis and Design* course.

---

## Overview

It is the search engine API service for Beyond Scholar.

Powered by [ASP.NET Core](https://dotnet.microsoft.com/en-us/), Beyond Search Engine is a cross-platform, high-performance, cloud-ready Web API. With MVC pattern, everything became so clear and organized, not to mention C# itself is an opiniated language.

We get all data from [OpenAlex](https://openalex.org/), and indexed them to our database - [Elasticsearch](https://www.elastic.co/guide/en/elasticsearch/reference/current/index.html). For our server, we have one 8 core 16 GB master server and two 2 core 4 GB slave servers, with ASP.NET Core project deployed only on master.

---

## Features

- Fast response with Elasticsearch integration.
- Hot update on hundred millions of data.
- Cache support with Redis.

---

## Pitfalls

- Failed to index some of OpenAlex's data, as some fields are `null` or missing.
- Huge amount of data took too much time to index before optimizations applied.
- The response is not consistent. Some DTO are crafted ugly and messy.

---

## Summary

It was an amazing experience working with ASP.NET Core and Elasticsearch. Here are some trails left behind.

At the very beginning, you need to [Setup Elasticsearch With Kibana](https://www.tonys-studio.top/posts/Setup-Elasticsearch-with-Kibana/). Then, if you need to move your data from a relational database, see how to [Ingest Data From MySQL to Elasticsearch](https://www.tonys-studio.top/posts/Ingest-Data-from-MySQL-to-Elasticsearch/). Next, as your first step towards cross-platform development with .NET, you can skim over [Cross-Platform Development With .NET Core](https://www.tonys-studio.top/posts/Cross-platform-Development-with-NET-Core/). At last, [Empower ASP.NET Core With Elasticsearch](https://www.tonys-studio.top/posts/Empower-ASP-NET-Core-with-Elasticsearch/), you can.

> If you encountered some performance issue when indexing large amount of data, you can reach out to [Bulk Task Optimization in C#](https://www.tonys-studio.top/posts/Bulk-Task-Optimization-in-C/).

---

## Epilogue

There's another backend project written in Python with Django. I have to say, "ASP.NET Core superior, Django inferior." ASP.NET Core gains land slide advantages against Django, whether in performance, deploy complexity, or productivity.

ASP.NET Core is cloud-ready, yet Django is not. Thus all problems of the backend deployment failure comes from Django project, and Django is much harder to configure than the other.

As for speed, Python takes no chance. Even with muti-process of uWSGI and clusters, Django is easily overwhelmed by 1000 concurrent requests. While ASP.NET Core hosted by Kestrel can take more than 10000 requests with no failure.

<p style="text-align:center"><i>Life is short, I use C#.</i></p>