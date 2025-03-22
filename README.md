# Evaluation Instructions

## Introduction

The purpose of this exercise is to evaluate your approach to creating a very simple backend application using the language of your choice. We expect you to spend a couple of hours or less to complete this, and it will be used to evaluate how you approach a technical task.

The diagram below illustrates what it is that you will need to build. In simple terms, we expect you to build a very simple application that connects to a static XML API and transforms it to a JSON response. There are more detailed instructions provided below.

![Component Overview](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/MiddlewareNewZealand/evaluation-instructions/main/images/components.puml)

## Sequence

1. Read these instrucitons carefully.
2. Develop a simple API (see [Evaluation Details](#-Evaluation-details))
3. Publish your code to a source repository that we can access
4. Provide us with a link to this code at least a day before you come to see us

## Evaluation details

- Create an application using any libraries you wish
- When a request is received by the application, make another request to the existing static backend xml service - see curl examples below.

  ```bash
  curl https://raw.githubusercontent.com/MiddlewareNewZealand/evaluation-instructions/main/xml-api/1.xml
  ```

  ```bash
  curl https://raw.githubusercontent.com/MiddlewareNewZealand/evaluation-instructions/main/xml-api/2.xml
  ```

- Transform the backend xml response into a json format that matches the [supplied OpenAPI specification](./openapi-companies.yaml) and return it to the client
- Ensure that the application code handles errors from the xml service appropriately
- Ensure that the application code is tested appropriately

## Things to consider

When you come to see us you will need to consider the following so that we can chat about what you have done.

- You will need to tell us how you went about creating your application and why you created it in the way that you did
- Think about how you would deploy the application into a production environment
- Think about how your application is documented - remember that your audience (us) are technical and should be able to run your code

## Resources

- [Your API OpenAPI specification](./openapi-companies.yaml)
- [Existing XML API OpenAPI specification](./xml-api/openapi-xml.yaml)
- Existing XML API curl example

  ```bash
  curl https://raw.githubusercontent.com/MiddlewareNewZealand/evaluation-instructions/main/xml-api/1.xml
  ```

  ```bash
  curl https://raw.githubusercontent.com/MiddlewareNewZealand/evaluation-instructions/main/xml-api/2.xml
  ```

# Pete's solution

## Notes

With no requirement to use a given technology or coding style, I have opted for a .NET solution using Uncle Bob's Clean Code guidelines as they are familiar, the tools are to hand and the purpose of the exercise is not to learn something new or try something exotic.  Clearly this is fluid and can vary on a project-by-project basis.

The solution was developed using Visual Studio, but will run outside of that with the .NET SDK installed (eg. on a GNU/Linux machine or running vscode).

The solution was developed in TDD fashion, starting with a 'walking skeleton' and a failing end-to-end integration test.  See the commit history for the order in which I tackled things.  Commits were straight-to-`main`, no feature branching or any of that overhead.

Consider this a *Minimum Viable Product* - it meets the stated requirements but I have drawn the line at making a robust or extensible solution since this is a demo.

I will also direct you to the README for another coding test that I did, [https://github.com/pete-restall/CodingTests](https://github.com/pete-restall/CodingTests), which gives more insight into the thought process of how I tackle a problem and come up with assumptions in lieu of requirements.

## Building

No binaries are included, so a build will be required by yourselves.  This is accomplished by opening in Visual Studio, or via the command-line:
```bash
dotnet build src/EvaluationApi.sln
```

## Running

Running the project should be as simple as hitting `F5` in Visual Studio (make sure the `https` launch configuration has been set), or running the following:
```bash
dotnet run src/EvaluationApi.sln --project src/EvaluationApi/EvaluationApi.csproj
```

## Testing

Testing the project depends on your test runner, but pretty much it's using the built-in Visual Studio commands or running the following:
```bash
dotnet test src/EvaluationApi.sln
```
*Note that this demo runs the integration tests using HTTPS and assumes that your dev machine's SSL certificate has been trusted; if not then you will need to do that before running the tests - this is not necessary before running the curl commands below, however*

The `curl` examples below assume a `dotnet run ...` in an open terminal that exposes `http://localhost:5237`, and the `curl` commands are run in another terminal (or equivalent browser interaction if you wish).

### OpenAPI / Swagger
The Swagger output should conform to your requirements, with a couple of points of note:
1. I do have an additional `500` response as part of the 'handles errors ... appropriately'.  There are also possibilities for responses beyond those documented by Swagger, but I decided plugging in pipeline middleware to make for a more robust solution was beyond the scope of the exercise.
2. Error handling in general could be *a lot* better and would benefit from more exhaustive testing, but I have chosen to leave as-is due to the amount of time I've already spent on this - there is sufficient material to provide a talking point.
3. The names of the schema elements also do not match (eg. `Error` vs `ErrorResponse`) but I made the assumption that you care more about the semantics than getting an exact character match:
```bash
curl http://localhost:5237/swagger/v1/swagger.json
```

### Querying a company
The following:
```bash
curl -i http://localhost:5237/companies/1
```
Produces output similar to:
```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Sat, 22 Mar 2025 21:24:11 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"id":1,"name":"MWNZ","description":"..is awesome"}
```

### Generating an error
The following:
```bash
curl -i http://localhost:5237/companies/42
```
Produces output similar to:
```
HTTP/1.1 404 Not Found
Content-Type: application/json; charset=utf-8
Date: Sat, 22 Mar 2025 21:25:59 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"error":"RestEase.ApiException","error_description":"GET \"https://raw.githubusercontent.com/MiddlewareNewZealand/evaluation-instructions/main/xml-api/42.xml\" failed because response status code does not indicate success: 404 (Not Found)."}
```

## Deploying

As per the initial requirements, thought was put into deployment but not a great amount of effort - this is a rabbit hole and depends on many factors.  However, I have produced a sample `Dockerfile` that illustrates one way this solution can be containerised and deployed.

An automated build / test / package / deploy pipeline could also have been constructed (I have previously done this with GitHub Actions, Azure DevOps, GitLab, Travis, TFS, TeamCity, Octopus, Terraform, ... lots of ways) and I'm happy to talk it through.

## Thank you
