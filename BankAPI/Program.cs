using BankAPI.Config;
using BankAPI.Data;
using BankAPI.Repository;
using BankAPI.Repository.IRepository;
using BankAPI.Services;
using IBM.Data.DB2.Core;
using IBM.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});


// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICustomerRepository,CustomerRepository>();
builder.Services.AddScoped<IAccountRepository,AccountRepository>();
builder.Services.AddScoped<IDepositRepository,DepositRepository>();
builder.Services.AddScoped<IWithdrawalRepository,WithdrawalRepository>();
builder.Services.AddScoped<IBillRepository,BillRepository>();
builder.Services.AddScoped<ITransactionRepository,TransactionRepository>();
builder.Services.AddScoped<IP2PRepository,P2PRepository>();


// Services 

//builder.Services.AddScoped<CustomerService>();


//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseMySql(builder.Configuration.GetConnectionString("SqlConnection"),
//        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SqlConnection")));
//});


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
}); ;
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
