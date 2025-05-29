var builder = DistributedApplication.CreateBuilder(args);



var db = builder.AddContainer("subs-db", "postgres", "latest")
    .WithEnvironment("POSTGRES_PASSWORD", "1234")
    .WithEnvironment("POSTGRES_DB", "subs")
    .WithEnvironment("DB_USER", "postgres")
    .WithEndpoint(port: 5432, targetPort: 5432)
    .WithBindMount($"{Environment.GetEnvironmentVariable("HOME")}/docker_data","/var/lib/postgresql/data");

// var frontend = builder.AddContainer("subs-frontend", "frontend", "latest")
//     .WithHttpEndpoint(port: 3000, targetPort: 3000);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.NetSubs_ApiService>("subs-apiservice")
    .WithEnvironment("DB_HOST", "localhost")
    .WithEnvironment("DB_USER", "postgres")
    .WithEnvironment("DB_PASSWORD", "1234")
    .WaitFor(db);


builder.Build().Run();
