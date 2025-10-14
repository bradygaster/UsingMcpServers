var builder = DistributedApplication.CreateBuilder(args);

var tools = builder.AddProject<Projects.TranslationTools>("translationtools");

builder.AddProject<Projects.TranslationApi>("translationapi")
       .WithReference(tools)
       .WaitFor(tools);

builder.Build().Run();
