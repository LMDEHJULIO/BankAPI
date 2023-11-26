using BankAPI.Config;
using BankAPI.Data;
using IBM.Data.DB2.Core;
using IBM.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
 


// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});



//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseMySql(builder.Configuration.GetConnectionString("SqlConnection"),
//        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SqlConnection")));
//});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//var dbConnectionString = builder.Configuration.GetConnectionString("Db2Connection");

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseDb2(dbConnectionString, db2Options => { 

//    }));
builder.Services.AddAutoMapper(typeof(MappingConfig));





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
