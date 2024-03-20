using TXTextControl.Web.MVC.DocumentViewer;

var builder = WebApplication.CreateBuilder(args);

// adding CORS policy to allow all origins
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
			   builder =>
			   {
				   builder.AllowAnyOrigin()
						  .AllowAnyMethod()
						  .AllowAnyHeader();
			   });
});

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

// adding CORS middleware
app.UseCors();

app.UseTXDocumentViewer();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
