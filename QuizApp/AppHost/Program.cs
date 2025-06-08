using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var identity = builder.AddProject<QuizApp_Identity>("identity");
var api = builder.AddProject<QuizApp_API>("back");
var bff = builder.AddProject<QuizApp_BFF>("middle")
.WithReference(identity)
.WithReference(api);
var client = builder.AddProject<QuizApp_BlazorWASM>("front")
    .WithReference(bff)
    .WithExternalHttpEndpoints();
builder.Build().Run();
