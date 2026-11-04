using EventRegistrationAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// 1) Turn on controllers (the classes inside the Controllers folder)
builder.Services.AddControllers();

// 2) CORS: the web page lives in the FrontEnd folder, which is a different "address"
//    than this API. A browser only lets the page call the API if the API allows it here.
//    (AllowAnyOrigin is fine for a school project. A real system would list the exact address.)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// 3) Create ONE DataStore for the whole app. It keeps the events, students and
//    registrations in memory and saves them to data.json so nothing is lost.
//    Controllers receive it automatically through their constructor.
builder.Services.AddSingleton(new DataStore(Path.Combine(builder.Environment.ContentRootPath, "data.json")));

var app = builder.Build();

app.UseCors();           // must come before MapControllers
app.MapControllers();    // connects the [Route] attributes in the controllers to URLs like /api/events

// A friendly message if you open http://localhost:5005 in the browser
app.MapGet("/", () => "Event Registration API is running. Try http://localhost:5005/api/events");

app.Run();
