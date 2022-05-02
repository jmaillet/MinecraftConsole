using MineCraftConsole.Server.Startup;

WebApplication.CreateBuilder(args)
  .RegisterServices()
  .Build()
  .ConfigureRequestPipeline()
  .Run();


