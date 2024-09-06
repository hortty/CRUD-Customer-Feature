using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Customers.Application.Services;
using Customers.Domain.Interfaces;
using Customers.Infrastructure.Context;
using Customers.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Customers.Application.Validators;
using FluentValidation;
using Customers.Domain.Dtos;
using Customers.Domain.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// var config = new MapperConfiguration(cfg => {
    
//     cfg.AddMaps("Customers.Domain");
// });

// var mapper = config.CreateMapper();

// builder.Services.AddSingleton(mapper);


builder.Services.AddAutoMapper(typeof(CustomerProfile).Assembly);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
// builder.Services.AddScoped<IUserService, UserService>();

// Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
// builder.Services.AddScoped<IUserRepository, UserRepository>();

// Validators
builder.Services.AddTransient<IValidator<CreateCustomerDto>, CreateCustomerDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateCustomerDto>, PutCustomerDtoValidator>();
builder.Services.AddTransient<IValidator<GetCustomerDto>, GetCustomerDtoValidator>();
builder.Services.AddTransient<IValidator<DeleteCustomerDto>, DeleteCustomerDtoValidator>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
